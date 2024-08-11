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

    }
}
