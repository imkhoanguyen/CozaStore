using System.ComponentModel.DataAnnotations;

namespace CozaStore.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name="Tên danh mục")]
        public required string Name { get; set; }

        //navtigation property
        public ICollection<SubCategory> SubCategories { get; set; } = [];
    }

}
