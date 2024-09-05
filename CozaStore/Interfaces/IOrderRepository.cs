using CozaStore.Helpers;
using CozaStore.Models;

namespace CozaStore.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Task<Order?> GetAsync(int id);
        void UpdateStripePaymentId(int orderId, string stripeSessionId, string stripePaymentIntentId);
        void UpdateStatus(int orderId, int orderStatus);
        void UpdatePaymentStatus(int orderId, int paymentStatus);
        Task<PagedList<Order>> GetAllAsync(OrderParams orderParams, string userId);
        Task<IEnumerable<Order>> GetAllAsync();
        void Delete(Order order);
    }
}
