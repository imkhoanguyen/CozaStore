using CozaStore.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal DisplayPrice { get; set; } // default price if no choose variant
        public int Status { get; set; } = (int)ProductStatus.Private;
        public bool IsFeatured { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
       

        //navigation property
        //category
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }

        //variant
        public ICollection<Variant>? Variants { get; set; }

        //Image
        public ICollection<Image>? Images { get; set; } = [];
        [NotMapped]
        public List<IFormFile> ImagesFile { get; set; } = [];


    }
}
