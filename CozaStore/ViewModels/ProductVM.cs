using CozaStore.Data;
using CozaStore.DTOs;
using CozaStore.Helpers;
using CozaStore.Models;

namespace CozaStore.ViewModels
{
    public class ProductVM
    {
        public required PagedList<Product> Products { get; set; }
        //sort params
        public string? CurrentSort { get; set; }
        public string? NameSortParm { get; set; }
        public string? IdSortParm { get; set; }
        public string? PriceSortParm { get; set; }
        public string? StatusSortParm { get; set; }
        //search
        public string? CurrentFilter { get; set; }
        //select filter
        public string? SelectedPriceRange { get; set; }
        public int SelectedColor { get; set; }
        public int SelectedSize { get; set; }
        public List<int>? SelectedCategory { get; set; }
        //load combobox
        public IEnumerable<Category>? CategoryList { get; set; }
        public IEnumerable<Color>? ColorList { get; set; }
        public IEnumerable<string>? PriceRangeList { get; set; } = SD.PriceRangeList;
        public IEnumerable<Size>? SizeList { get; set; }
    }
}
