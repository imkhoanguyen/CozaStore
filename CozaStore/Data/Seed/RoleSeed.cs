using CozaStore.Models;
using Microsoft.AspNetCore.Identity;

namespace CozaStore.Data.Seed
{
    public class RoleSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            if (roleManager.Roles.Any()) return;

            var roles = new List<AppRole>
            {
                new() { Name = "Admin", Description="Manage the entire system " },
                new() { Name = "Customer", Description="View, buy and pay for products" },
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }
}
