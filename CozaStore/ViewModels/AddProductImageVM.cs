namespace CozaStore.ViewModels
{
    public class AddProductImageVM
    {
        public int ProductId { get; set; }
        public List<IFormFile> Images { get; set; } = [];
    }
}
