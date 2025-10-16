using AuthService.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Data
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedAsync(UserManager<IdentityAppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // 1. Ensure role exist
            string[] roles = ["Admin", "User"];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Ensure admin user exist
            var adminEmail = "admin@pgbff.com";
            var adminPwd = "admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser != null)
            {
                // User already exists, no need to create
                return;
            }
            adminUser = new IdentityAppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                IsActive = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPwd);
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
