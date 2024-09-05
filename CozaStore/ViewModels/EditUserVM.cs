using CozaStore.Models;
using CozaStore.ValidationAttr;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CozaStore.ViewModels
{
    public class EditUserVM
    {
        public required AppUser AppUser { get; set; }
        [AllowedExtensions(new string[] { ".jpg", ".jpeg" })]
        public IFormFile? Image { get; set; }
        [ValidateNever]
        public IEnumerable<Address> AddressList { get; set; } = [];
    }
}
