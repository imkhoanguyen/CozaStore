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

        public void Delete(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<PagedList<Order>> GetAllAsync(OrderParams orderParams, string userId = "")
        {
            var query = _context.Orders.Include(x => x.ShippingMethod).AsQueryable();

            if(!userId.IsNullOrEmpty())
            {
                query = query.Where(x => x.UserId == userId);
            }

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

            query = orderParams.SortOrder switch
            {
                "id" => query.OrderBy(x => x.Id),
                "id_desc" => query.OrderByDescending(x => x.Id),
                "total" => query.OrderBy(x => x.SubTotal + x.ShippingFee),
                "total_desc" => query.OrderByDescending(x => x.SubTotal + x.ShippingFee),
                _ => query.OrderByDescending(x => x.Id)
            };


            return await PagedList<Order>.CreateAsync(query, orderParams.PageNumber, orderParams.PageSize);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(x => x.OrderItemList).ToListAsync();
        }

        public async Task<Order?> GetAsync(int id)
        {
            return await _context.Orders
                .Include(x=>x.AppUser)
                .Include(x => x.OrderItemList)
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
