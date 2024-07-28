using Furni.DataAccess.Persistence;
using Furni.Models.Common;
using Furni.Web.Areas.Admin.Components;
using Furni.Web.Authorization;
using Furni.Web.Helpers;
using Furni.Web.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;
using ViewToHTML.Extensions;

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
            services.AddSingleton<IAuthorizationHandler, AdminProbationRequirementHandler>();
            // Register TwoFactorNoticeViewComponent
            services.AddScoped<TwoFactorNoticeViewComponent>();
            // Add View To HTML to Reports
            services.AddViewToHTML();


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

            // Hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();


            // Auto Validate to AntiForgeryToken
            services.AddMvc(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
            );

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login"; // Default path for login
                options.LogoutPath = "/Identity/Account/Logout"; // Default path for logout
                options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Default path for access denied
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("InitialAccessPolicy",
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(AppRoles.Admin)
                                        .RequireClaim("Access", "Initial");
                    });

                options.AddPolicy("ExtendedAccessPolicy",
                    policy => {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(AppRoles.Admin)
                                        .RequireClaim("Access", "Extended")
                                        .Requirements.Add(new AdminProbationRequirement(3));
                              });

                options.AddPolicy("ManagerOnly",
                    policy => {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole(AppRoles.Admin)
                                .RequireClaim("Access", "Manager");
                    });
            });



            


            return services;
        }
    }
}
