using CozaStore.Data.Enum;
using CozaStore.Interfaces;
using CozaStore.Models;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        public OrderRepository(DataContext context)
        {
            _context = context;
        }
        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public async Task<Order?> GetAsync(int id)
        {
            return await _context.Orders.Include(x=>x.OrderItemList)
                .Include(x=>x.ShippingMethod).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public void UpdatePaymentStatus(int orderId, int paymentStatus)
        {
            var orderFromDb = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (orderFromDb != null)
            {
                orderFromDb.PaymentStatus = paymentStatus;
            }
        }

        public void UpdateStatus(int orderId, int orderStatus)
        {
            var orderFromDb = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
            }
        }

        public void UpdateStripePaymentId(int orderId, string stripeSessionId, string stripePaymentIntentId)
        {
            var orderFromDb = _context.Orders.FirstOrDefault(x=>x.Id==orderId);
            if (orderFromDb != null)
            {
                orderFromDb.StripePaymentIntentId = stripePaymentIntentId;
                orderFromDb.StripeSessionId = stripeSessionId;
            }
        }
    }
}
