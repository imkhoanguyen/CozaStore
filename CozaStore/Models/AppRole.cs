using Microsoft.AspNetCore.Identity;

namespace CozaStore.Models
{
    public class AppRole : IdentityRole
    {
        public required string Description { get; set; } 
    }
}
