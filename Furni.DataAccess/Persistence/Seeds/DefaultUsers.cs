﻿using Microsoft.AspNetCore.Identity;

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
            };

            var user = await userManager.FindByEmailAsync(admin.Email);
            if (user is null)
            {
                await userManager.CreateAsync(admin, "P@ssword123");
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}
