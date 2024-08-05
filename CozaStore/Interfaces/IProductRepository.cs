    using CozaStore.Entities;
using CozaStore.Helpers;

namespace CozaStore.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetAllProductsAsync(string searchString, int page);
        void AddProduct(Product product);
    }
}
