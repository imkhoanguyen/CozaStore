using CozaStore.Models;
using Microsoft.AspNetCore.Identity;

namespace CozaStore.Data.Seed
{
    public class UserRoleSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, DataContext context)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            if (context.UserRoles.Any()) return;

            var adminUser = await userManager.FindByNameAsync("Admin");

            await userManager.AddToRoleAsync(adminUser, "Admin");

            var customerUser = await userManager.FindByNameAsync("Customer");

            await userManager.AddToRoleAsync(customerUser, "Customer");

        }
    }
}
