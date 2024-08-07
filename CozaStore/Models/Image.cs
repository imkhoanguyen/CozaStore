using CozaStore.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class Image
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public bool IsMain { get; set; } = false;
        public string? PublicId { get; set; } // cloudinary

        //navigation property
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }
    }
}
