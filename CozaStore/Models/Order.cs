using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace CozaStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public decimal SubTotal { get; set; }
        public int PaymentMethod { get; set; }

        public DateTime OrderDate { get; set; }
        public int OrderStatus { get; set; }

        //address
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? SpecificAddress { get; set; }

        //shipping
        public decimal ShippingFee { get; set; }

        //navigate property
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public AppUser? AppUser { get; set; }

        public int ShippingMethodId { get; set; }
        [ForeignKey("ShippingMethodId")]
        [ValidateNever]
        public ShippingMethod? ShippingMethod { get; set; }

        List<OrderItem> OrderItemList { get; set; } = [];

        public decimal GetTotal()
        {
            return SubTotal - ShippingFee;
        }
    }
}
