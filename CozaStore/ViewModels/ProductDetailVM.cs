using CozaStore.Helpers;
using CozaStore.Models;

namespace CozaStore.ViewModels
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal PriceSell { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Color> ColorList { get; set; } = [];
        public IEnumerable<Size> SizeList { get; set; } = [];
        public IEnumerable<string> ImageList { get; set; } = [];
        public PagedList<Review>? ReviewList { get; set; }
    }
}
