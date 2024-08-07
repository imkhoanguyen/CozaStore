using System.ComponentModel.DataAnnotations;

namespace CozaStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool? IsDelete { get; set; } = false;
        public ICollection<Product>? Products { get; set; }
    }

}
