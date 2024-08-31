namespace CozaStore.Helpers
{
    public class OrderParams : PaginationParams
    {
        public string? SearchString { get; set; }
        public string? SortOrder { get; set; }
        public int SelectedShipping { get; set; }
        public int SelectedPayment { get; set; } = -1;
        public int SelectedStatus { get; set; } = -1;
        public DateTime? DateStart { get; set; } = null;
        public DateTime? DateEnd { get; set; } = null;
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }
    }
}
