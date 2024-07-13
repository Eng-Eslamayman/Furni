using Furni.DataAccess.Persistence;
using Furni.Models.Common;
using Furni.Web.Areas.Admin.Components;
using Furni.Web.Helpers;
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
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IEmailBodyBuilder, EmailBodyBuilder>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
            // Register TwoFactorNoticeViewComponent
            services.AddScoped<TwoFactorNoticeViewComponent>();


            // Settings
            //services.Configure<FacebookSetting>(builder.Configuration.GetSection(nameof(FacebookSetting)));
            //services.Configure<GoogleSetting>(builder.Configuration.GetSection(nameof(GoogleSetting)));
            services.Configure<STMPSetting>(builder.Configuration.GetSection(nameof(STMPSetting)));
            services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


            // Add External Login Configuration to Facebook
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? string.Empty;
                options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? string.Empty;
            });

            // Add External Login Configuration to Google
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? string.Empty;
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? string.Empty;
            });

            // Add Log Out User when needed 
            //services.Configure<SecurityStampValidatorOptions>(options => 
            //                            options.ValidationInterval = TimeSpan.Zero);


            // Auto Validate to AntiForgeryToken
            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            );


            return services;
        }
    }
}
