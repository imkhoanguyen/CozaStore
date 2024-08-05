using CozaStore.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public int Status { get; set; } = (int)ImageStatus.Public;
        public string? PublicId { get; set; }

        //navigation property
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product? Product { get; set; }
    }
}
