using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<PagedList<Order>> GetAllAsync(string seachString, int page)
        {
            var query = _context.Orders.Include(x=>x.ShippingMethod).AsQueryable();
            if(!seachString.IsNullOrEmpty())
            {
                query = query.Where(x=>x.FullName.ToLower().Contains(seachString.ToLower())
                || x.Id == int.Parse(seachString) || x.Phone.Contains(seachString));
            }

            if (page < 1) page = 1;
            return await PagedList<Order>.CreateAsync(query, page, 10);
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
