using Microsoft.AspNetCore.Identity;
using PhotoBank.Models;

namespace PhotoBank.Utilities
{
    public class RoleInitializer
    {
        public async Task InitializeDbAsync(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            if (await roleManager.FindByNameAsync(WebConstants.AdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole( WebConstants.AdminRole));
            }
            if (await roleManager.FindByNameAsync(WebConstants.AuthorRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole( WebConstants.AuthorRole));
            }

            if (await userManager.FindByEmailAsync(WebConstants.AdminEmail) == null)
            {
                User admin = new User
                {
                    Email = WebConstants.AdminEmail,
                    UserName = WebConstants.AdminEmail
                };
                var result = await userManager.CreateAsync(admin, 
                    WebConstants.AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, WebConstants.AuthorRole);
                }
            }
        }
    }
}
