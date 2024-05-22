using Furni.DataAccess.Persistence;
using Furni.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

namespace Furni.Web
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // Add services to the container.
            services.AddControllersWithViews();

            services.AddDatabaseDeveloperPageExceptionFilter();

            // Identity Registeration
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultUI()
				.AddDefaultTokenProviders()
				.AddSignInManager<SignInManager<ApplicationUser>>();

            services.Configure<IdentityOptions>(options =>
            {
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                //options.Password.RequiredUniqueChars = 1;
                options.User.RequireUniqueEmail = true;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                //options.Lockout.MaxFailedAccessAttempts = 3;
            });

            // Add AutoMapper
            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            // Add Expressive Annotation
            services.AddExpressiveAnnotations();
            // Services
            services.AddTransient<IImageService, ImageService>();
            // Settings
            //services.Configure<STMPSetting>(builder.Configuration.GetSection(nameof(STMPSetting)));
            //services.Configure<FacebookSetting>(builder.Configuration.GetSection(nameof(FacebookSetting)));
            //services.Configure<GoogleSetting>(builder.Configuration.GetSection(nameof(GoogleSetting)));

            // Auto Validate to AntiForgeryToken
            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            );

            return services;
        }
    }
}
