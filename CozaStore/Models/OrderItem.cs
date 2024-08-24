using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required string? Size { get; set; }
        public string? Color { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public decimal GetTotal()
        {
            return Price * Count;
        }

        //nav
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order? Order { get; set; }
    }
}
