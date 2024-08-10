    using CozaStore.Models;
using CozaStore.Helpers;

namespace CozaStore.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetAllProductsAsync(string sortOrder, string searchString, int categoryId, int status, string priceRange, int sizeId, int colorId, int page, int pageSize);
        void AddProduct(Product product);

        Task<Product?> GetProductAsync(int id);

        void UpdateProduct(Product product);

        void ToggleProductStatus(Product product);

        Task<Product?> GetProductDetailAsync(int id);

    }
}
