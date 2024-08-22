using CozaStore.Helpers;
using CozaStore.Models;

namespace CozaStore.Interfaces
{
    public interface IShippingRepository
    {
        Task<PagedList<ShippingMethod>> GetAllShippingMethodsAsync(string searchString, int page);
        void Add(ShippingMethod shippingMethod);
        void Update(ShippingMethod shippingMethod);
        void Delete(ShippingMethod shippingMethod);
        Task<ShippingMethod?> GetAsync(int id);
        Task<IEnumerable<ShippingMethod>> GetAllAsync();
    }
}
