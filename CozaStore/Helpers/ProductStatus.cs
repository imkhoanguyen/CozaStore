namespace CozaStore.Helpers
{
    public enum ProductStatus
    {
        Deleted = 0,
        Public = 1,
        Private = 2,
    }

    public class ProductStatusDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
