namespace CozaStore.Models
{
    public class Color
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
