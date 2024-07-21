using Furni.DataAccess;
using Furni.DataAccess.Persistence;
using Furni.DataAccess.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stripe;
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

			var roleManger = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManger = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			await DefaultRoles.SeedAsync(roleManger);
			await DefaultUsers.SeedAdminUserAsync(userManger);

			app.MapControllerRoute(
				name: "default",
				pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

			app.Run();
		}
	}
}
