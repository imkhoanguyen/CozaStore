    using CozaStore.Models;
using CozaStore.Helpers;

namespace CozaStore.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetAllProductsAsync(string searchString, int page);
        void AddProduct(Product product);

        Task<Product?> GetProductAsync(int id);

        void UpdateProduct(Product product);

        void ToggleProductStatus(Product product);

    }
}
