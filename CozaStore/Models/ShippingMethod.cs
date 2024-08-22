namespace CozaStore.Models
{
    public class ShippingMethod
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Cost { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
