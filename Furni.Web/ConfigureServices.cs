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

			services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultUI()
				.AddDefaultTokenProviders()
				.AddSignInManager<SignInManager<ApplicationUser>>();

			// Add AutoMapper
			services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));

            // Add Expressive Annotation
            services.AddExpressiveAnnotations();
            // Services
            services.AddTransient<IImageService, ImageService>();

            // Auto Validate to AntiForgeryToken
            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            );

            return services;
        }
    }
}
