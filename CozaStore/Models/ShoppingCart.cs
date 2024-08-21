using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public decimal Price { get; set; }
        //navigate property
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public AppUser? AppUser { get; set; }

        //
        [NotMapped]
        [ValidateNever]
        public Size? Size { get; set; }
        [NotMapped]
        [ValidateNever]
        public Color? Color { get; set; }
    }
}
