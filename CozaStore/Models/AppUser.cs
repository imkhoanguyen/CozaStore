using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public int Gender { get; set; }
        public string? Image { get; set; }

        [NotMapped]
        public string? Role { get; set; }

        //nav
        public List<Address> AddressList { get; set; } = [];
    }
}
