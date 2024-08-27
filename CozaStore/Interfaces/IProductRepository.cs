    using CozaStore.Models;
using CozaStore.Helpers;

namespace CozaStore.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetAllProductsAsync(ProductParams productParams);
        void AddProduct(Product product);

        Task<Product?> GetProductAsync(int id);

        void UpdateProduct(Product product);

        void ToggleProductStatus(Product product);

        Task<Product?> GetProductDetailAsync(int id);

        Task<IEnumerable<Color?>> GetAvailableColorsAsync(int productId, int sizeId);
        Task<IEnumerable<Size?>> GetAvailableSizesAsync(int productId, int colorId);
        Task<Variant?> GetVariantOfProductAsync(int productId, int colorId, int sizeId);
        void UpdateQuantity(int productId, int quantity);

    }
}
