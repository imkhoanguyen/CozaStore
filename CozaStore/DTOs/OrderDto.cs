namespace CozaStore.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Shipping { get; set; }
        public int PaymentStatus { get; set; }
        public required string OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
