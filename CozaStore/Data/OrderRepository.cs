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

        public async Task<PagedList<Order>> GetAllAsync(OrderParams orderParams)
        {
            var query = _context.Orders.Include(x => x.ShippingMethod).AsQueryable();
            if (!orderParams.SearchString.IsNullOrEmpty())
            {
                query = query.Where(x => x.FullName.ToLower().Contains(orderParams.SearchString.ToLower())
                || x.Id.ToString() == orderParams.SearchString || x.Phone.Contains(orderParams.SearchString)
                || x.SpecificAddress.ToString().Contains(orderParams.SearchString.ToLower())
                );
            }

            if(orderParams.SelectedShipping > 0)
            {
                query = query.Where(x => x.ShippingMethodId == orderParams.SelectedShipping);
            }

            if(orderParams.SelectedPayment != -1)
            {
                query = query.Where(x=>x.PaymentStatus == orderParams.SelectedPayment);
            }

            if(orderParams.SelectedStatus != -1)
            {
                query = query.Where(x => x.OrderStatus == orderParams.SelectedStatus);
            }

            if (orderParams.DateStart.HasValue && orderParams.DateEnd.HasValue)
            {
                query = query.Where(x => x.OrderDate >= orderParams.DateStart && x.OrderDate <= orderParams.DateEnd);
            }

            if (orderParams.PriceMin > 0 && orderParams.PriceMax > 0)
            {
                query = query.Where(x => (x.SubTotal + x.ShippingFee) >= orderParams.PriceMin && (x.SubTotal + x.ShippingFee) <= orderParams.PriceMax);
            }




            return await PagedList<Order>.CreateAsync(query, orderParams.PageNumber, orderParams.PageSize);
        }

        public async Task<Order?> GetAsync(int id)
        {
            return await _context.Orders.Include(x => x.OrderItemList)
                .Include(x => x.ShippingMethod).FirstOrDefaultAsync(x => x.Id == id);
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
            var orderFromDb = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (orderFromDb != null)
            {
                orderFromDb.StripePaymentIntentId = stripePaymentIntentId;
                orderFromDb.StripeSessionId = stripeSessionId;
            }
        }
    }
}
