using CozaStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CozaStore.ViewModels
{
    public class UserCreateVM
    {
        public AppUser AppUser { get; set; } = new AppUser();
        public required string Password { get; set; }
        public IFormFile? Image { get; set; }
        [ValidateNever]
        public IEnumerable<AppRole> RoleList { get; set; } = [];
        [Required]
        public string? RoleId { get; set; }

    }
}
