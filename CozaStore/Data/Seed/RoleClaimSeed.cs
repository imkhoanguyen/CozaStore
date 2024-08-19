using CozaStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data.Seed
{
    public class RoleClaimSeed
    {
        public static async Task SeedAsync(DataContext context, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            if (context.RoleClaims.Any()) return;

            var adminRole = await roleManager.FindByNameAsync("Admin");
            var adminClaims = ClaimStore.adminClaims.Select(claim=> new IdentityRoleClaim<string>
            {
                RoleId = adminRole.Id,
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue,
            }).ToList();

            await context.RoleClaims.AddRangeAsync(adminClaims);

            var customerUser = await roleManager.FindByNameAsync("Customer");
            var customerClaims = ClaimStore.customerClaims.Select(claim => new IdentityRoleClaim<string>
            {
                RoleId = customerUser.Id,
                ClaimType = claim.ClaimType,
                ClaimValue = claim.ClaimValue,
            }).ToList();

            await context.RoleClaims.AddRangeAsync(customerClaims);

            await context.SaveChangesAsync();
        }
    }
}
