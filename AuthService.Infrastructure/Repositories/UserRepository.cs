using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository(UserManager<IdentityAppUser> userManager, RoleManager<IdentityRole> roleManager) : IUserRepository
    {
        public async Task<string> ChangePasswordAsync(string userEmail, string oldPassword, string newPassword)
        {
            var dbUser = await userManager.FindByEmailAsync(userEmail);
            if (dbUser == null) return "User not found!";

            var result = await userManager.ChangePasswordAsync(dbUser, oldPassword, newPassword);
            if (!result.Succeeded)
                return "Change password failed: " + string.Join(", ", result.Errors.Select(e => e.Description));

            return "";
        }

        public async Task<bool> CheckPasswordAsync(IUserInfo user, string password)
        {
            var dbUser = await userManager.FindByEmailAsync(user.Email);
            return dbUser != null && await userManager.CheckPasswordAsync(dbUser, password);
        }

        public async Task<string> DeactivateUser(IUserInfo userB)
        {
            var user = await userManager.FindByEmailAsync(userB.Email);
            if (user == null)
                return "User not found!";

            user.IsActive = false;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return "User deactivation failed: " + string.Join(", ", result.Errors.Select(e => e.Description));
            else
                return "";
        }

        public async Task<IUserInfo?> GetUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var roles = await userManager.GetRolesAsync(user);
            return new IdentityAppUserWrapper(user, roles);
        }

        public async Task<IUserInfo?> GetUserByGuidAsync(string userGuid)
        {
            var user = await userManager.FindByIdAsync(userGuid);
            if (user == null) return null;

            var roles = await userManager.GetRolesAsync(user);
            return new IdentityAppUserWrapper(user, roles);
        }


        public async Task<IUserInfo?> RegisterUserAsync(string email, string password)
        {
            var user = new IdentityAppUser
            {
                UserName = email,
                Email = email,
                IsActive = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return null;

            // Assign "User" role by default.
            if(!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            await userManager.AddToRoleAsync(user, "User");

            var roles = await userManager.GetRolesAsync(user);

            return new IdentityAppUserWrapper(user, roles);
        }
    }
}
