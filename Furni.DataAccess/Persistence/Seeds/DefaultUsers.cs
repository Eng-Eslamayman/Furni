using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Furni.DataAccess.Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new()
            {
                UserName = "admin",
                FullName = "Admin",
                Email = "admin@furnihuture.com",
                EmailConfirmed = true,
                CreatedOn = DateTime.UtcNow.AddMonths(-3) // Set to 3 months in the past
            };

            var user = await userManager.FindByEmailAsync(admin.Email);
            if (user is null)
            {
                await userManager.CreateAsync(admin, "P@SSword123");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);

                await userManager.AddClaimAsync(admin, new Claim("Access", "Initial"));
                await userManager.AddClaimAsync(admin, new Claim("Access", "Extended"));
                await userManager.AddClaimAsync(admin, new Claim("Access", "Manager")); // Explicitly deny access if needed

                // Add CreatedDate claim for probation period checks
                await userManager.AddClaimAsync(admin, new Claim("CreatedDate", admin.CreatedOn.ToString("o")));
            }
        }

    }

}
