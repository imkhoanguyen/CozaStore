using CozaStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CozaStore.ViewModels
{
    public class EditUserVM
    {
        public required AppUser AppUser { get; set; }
        [ValidateNever]
        public IFormFile? Image { get; set; }
        [ValidateNever]
        public IEnumerable<Address> AddressList { get; set; } = [];
    }
}
