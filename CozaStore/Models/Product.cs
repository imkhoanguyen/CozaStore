using CozaStore.Data.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSell { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; } = (int)ProductStatus.Draft;
        public bool IsFeatured { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        public bool IsDelete { get; set; } = false;




        //navigation property
        //category
        public List<ProductCategory> ProductCategories { get; set; } = [];

        //variant
        public List<Variant> Variants { get; set; } = [];

        //Image
        public List<Image> Images { get; set; } = [];

        //Review
        public List<Review> Reviews { get; set; } = [];

    }
}
