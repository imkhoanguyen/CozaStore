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
                    Gender = 1,
                    AddressList = new List<Address>()
                    {
                        new Address
                        {
                            FullName = "Nguyen Anh Khoa",
                            Phone = "0985576590",
                            SpecificAddress = "273 An Dương Vương – Phường 3 – Quận 5",
                        },
                        new Address
                        {
                            FullName = "Nguyen Van A",
                            Phone = "0159753852",
                            SpecificAddress = "105 Bà Huyện Thanh Quan – Phường Võ Thị Sáu – Quận 3",
                        },
                        new Address
                        {
                            FullName = "Nguyen Van B",
                            Phone = "0147258369",
                            SpecificAddress = "04 Tôn Đức Thắng – Phường Bến Nghé – Quận 1",
                        },
                    }
                },
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Admin_123");
            }
            
        }
    }
}
