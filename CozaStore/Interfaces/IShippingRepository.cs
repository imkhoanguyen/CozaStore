using CozaStore.Helpers;
using CozaStore.Models;

namespace CozaStore.Interfaces
{
    public interface IShippingRepository
    {
        Task<PagedList<ShippingMethod>> GetAllShippingMethodsAsync(ShippingParams shippingParams);
        void Add(ShippingMethod shippingMethod);
        void Update(ShippingMethod shippingMethod);
        void Delete(ShippingMethod shippingMethod);
        Task<ShippingMethod?> GetAsync(int id);
        Task<IEnumerable<ShippingMethod>> GetAllAsync(); // without delete
        Task<IEnumerable<ShippingMethod>> GetAllContainDeleteAsync();
    }
}
