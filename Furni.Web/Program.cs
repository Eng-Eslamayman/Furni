using Furni.DataAccess;
using Furni.DataAccess.Persistence;
using Furni.DataAccess.Persistence.Seeds;
using Hangfire.Dashboard;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Context;
using Stripe;
using System.Security.Claims;
using Furni.Web.Filters;
using Furni.Web.Background_Tasks;
using DocumentFormat.OpenXml.InkML;
namespace Furni.Web
{
	public class Program
	{
		public async static Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			// Add services to the container.
			builder.Services.AddDataAccessServices(builder.Configuration)
				.AddWebServices(builder);


            // Add Serilog
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
            builder.Host.UseSerilog();


            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
                app.UseExceptionHandler("/Customer/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
			}
            app.UseHttpsRedirection();
			app.UseStaticFiles();

			// Stripe Configuration
			StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
            app.UseStatusCodePagesWithReExecute("/Customer/Home/Error", "?id={0}");

            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

			using var scope = scopeFactory.CreateScope();

			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			await DefaultRoles.SeedAsync(roleManager);
			await DefaultUsers.SeedAdminUserAsync(userManager);



            // Hangfire 
            app.UseHangfireDashboard(
                "/Admin/hangfire"
                , new DashboardOptions
                {
                    DashboardTitle = "Furnihutre Dashboard",
                    IsReadOnlyFunc = (DashboardContext context) => true,
                    Authorization = new IDashboardAuthorizationFilter[]
                                        {
                                           new HangfireAuthorizationFilter("ManagerOnly")
                                        }
                }
                );

            

            // Serilog
            app.Use(async (context, next) =>
            {
                LogContext.PushProperty("UserID", context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                LogContext.PushProperty("UserName", context.User.FindFirst(ClaimTypes.Name)?.Value);

                await next();
            });

            app.MapControllerRoute(
				name: "default",
				pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            // Initialize Hangfire tasks
            var webHostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var emailBodyBuilder = scope.ServiceProvider.GetRequiredService<IEmailBodyBuilder>();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            var hangfireTasks = new HangfireTasks(webHostEnvironment, emailBodyBuilder, emailSender,unitOfWork);
            // Schedule CleanupIncompleteOrders to run hourly
            RecurringJob.AddOrUpdate(
                "CleanupIncompleteOrders",
                () => hangfireTasks.CleanupIncompleteOrders(),
                Cron.Minutely);

            // Schedule ProcessCartAdjustmentsAsync to run hourly
            RecurringJob.AddOrUpdate(
                "ProcessCartAdjustmentsAsync",
                () => hangfireTasks.ProcessCartAdjustmentsAsync(),
                Cron.Minutely);

            app.Run();
		}
    }
}
