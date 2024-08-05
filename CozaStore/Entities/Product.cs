using CozaStore.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal DisplayPrice { get; set; }
        public int Status { get; set; } = (int)ProductStatus.Private;
        public bool IsFeatured { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
       

        //navigation property
        //subcategory
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        [ValidateNever]
        public SubCategory? SubCategory { get; set; }

        //variant
        public ICollection<Variant>? Variants { get; set; }

        //Image
        public ICollection<Image>? Images { get; set; } = [];
        [NotMapped]
        public List<IFormFile> ImagesFile { get; set; } = [];


    }
}
