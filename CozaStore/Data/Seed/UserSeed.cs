using CozaStore.Models;
using Microsoft.AspNetCore.Identity;

namespace CozaStore.Data.Seed
{
    public class UserSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, DataContext context)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            if (context.AppUser.ToList().Count >= 1) return;


            var users = new List<AppUser>()
            {
                new AppUser
                {
                    FullName = "Admin",
                    UserName = "admin",
                    Email = "itk21sgu@gmail.com",
                    PhoneNumber = "0123456789",
                    Image = "/img/avatar.png",
                    Gender = 1
                },
                new AppUser
                {
                    FullName = "Customer",
                    UserName = "customer",
                    Email = "khoasgu01@gmail.com",
                    PhoneNumber = "0987654321",
                    Image = "/img/avatar.png",
                    Gender = 1
                },
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Admin_123");
            }
            
        }
    }
}
