namespace CozaStore.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        //navtigation property
        public ICollection<SubCategory> SubCategories { get; set; } = [];
    }

}
