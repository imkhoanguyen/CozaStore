using Microsoft.AspNetCore.Identity;

namespace CozaStore.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
