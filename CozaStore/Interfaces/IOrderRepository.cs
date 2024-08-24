using CozaStore.Models;

namespace CozaStore.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Task<Order?> GetAsync(int id);
    }
}
