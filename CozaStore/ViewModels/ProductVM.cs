using CozaStore.Helpers;
using CozaStore.Models;

namespace CozaStore.ViewModels
{
    public class ProductVM
    {
        public required PagedList<Product> Products { get; set; }
        public string? CurrentSort { get; set; }
        public string? NameSortParm { get; set; }
        public string? IdSortParm { get; set; }
        public string? PriceSortParm { get; set; }
        public string? StatusSortParm { get; set; }
        public string? CurrentFilter { get; set; }
        public int SelectedStatus { get; set; }
        public string? SelectedPriceRange { get; set; }
        public int SelectedColor { get; set; }
        public int SelectedCategory { get; set; }
        public int SelectedSize { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<ProductStatusDto>? Statuses { get; set; }
        public IEnumerable<Color>? Colors { get; set; }
        public List<string>? PriceRanges { get; set; }
        public IEnumerable<Size>? Sizes { get; set; }
    }
}
