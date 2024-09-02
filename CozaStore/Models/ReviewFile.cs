using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
	public class ReviewFile
	{
        public int Id { get; set; }
        public string? PublicId { get; set; }
        public required string Url { get; set; }

        //nav
        public int ReviewId { get; set; }
        [ForeignKey("ReviewId")]
        [ValidateNever]
        public Review? Review { get; set; }
    }
}
