namespace CozaStore.Helpers
{
    public enum VariantStatus
    {
        Deleted = 0,
        Public = 1,
        Private = 2,
        Main = 3,
    }

    public class VariantStatusDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
