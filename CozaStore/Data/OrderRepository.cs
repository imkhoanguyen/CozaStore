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
    }
}
