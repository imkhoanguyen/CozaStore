using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
	public class Review
	{
        public int Id { get; set; }
        public required string Content { get; set; }
        public int Rating { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsDelete { get; set; } = false;
        //nav
        public required string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public AppUser? AppUser { get; set; }

        public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product? Product { get; set; }

    }
}
