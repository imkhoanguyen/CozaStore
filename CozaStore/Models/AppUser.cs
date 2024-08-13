using CozaStore.Helpers.Enum;
using Microsoft.AspNetCore.Identity;

namespace CozaStore.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public int Gender { get; set; }
        public string? Image { get; set; }
        public int Status { get; set; } = (int)UserStatus.Active;
    }
}
