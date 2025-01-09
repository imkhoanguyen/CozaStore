namespace CozaStore.ViewModels
{
    public class DashboardVM
    {
        public IEnumerable<decimal> TotalPriceList { get; set; } = [];
        public IEnumerable<int> TotalOrderList { get; set; } = [];
        public decimal TotalPriceToday { get; set; }
        public int TotalUser { get; set; }
        public int TotalProduct { get; set; }
        public int TotalOrder { get; set; }
        public IEnumerable<TopProductVM> TopProductList { get; set; } = [];
    }

    public class TopProductVM
    {
        public required string Name { get; set; }
        public required string ImgUrl { get; set; }
        public int Count { get; set; }
    }
}
