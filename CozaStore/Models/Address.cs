using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class Address
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Phone { get; set; }
        public required string SpecificAddress { get; set; }
        public bool isDefault { get; set; } = false;

        //nav 
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public AppUser? AppUser { get; set; }
    }
}
