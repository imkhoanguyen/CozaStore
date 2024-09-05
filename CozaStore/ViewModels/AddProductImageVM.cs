using CozaStore.ValidationAttr;

namespace CozaStore.ViewModels
{
    public class AddProductImageVM
    {
        public int ProductId { get; set; }
        [AllowedExtensionsList(new string[] { ".jpg", ".jpeg" })]
        public List<IFormFile> Images { get; set; } = [];
    }
}
