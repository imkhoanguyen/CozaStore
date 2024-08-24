using CozaStore.Data.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class Variant
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSell { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; } = (int)VariantStatus.Private;

        //navigation property
        //size
        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        [ValidateNever]
        public Size? Size { get; set; }


        //color
        public int ColorId { get; set; }
        [ForeignKey("ColorId")]
        [ValidateNever]
        public Color? Color { get; set; }

        //product
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }

    }
}
