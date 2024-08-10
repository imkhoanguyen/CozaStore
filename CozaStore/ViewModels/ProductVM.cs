using CozaStore.DTOs;
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
        public List<Category>? Categories { get; set; }
        public List<StatusDto>? Statuses { get; set; }
        public List<Color>? Colors { get; set; }
        public List<string>? PriceRanges { get; set; }
        public List<Size>? Sizes { get; set; }
    }
}
