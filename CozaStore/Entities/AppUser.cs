using Microsoft.AspNetCore.Identity;

namespace CozaStore.Entities
{
    public class AppUser : IdentityUser
    {
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
