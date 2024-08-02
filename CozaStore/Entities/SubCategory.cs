using System.ComponentModel.DataAnnotations;

namespace CozaStore.Entities
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }

        //navtigation property
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
