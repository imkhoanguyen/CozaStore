namespace CozaStore.Entities
{
    public class Size
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsDelete { get; set; } = false; 
    }
}
