using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CozaStore.Data
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly DataContext _context;

        public ShippingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(ShippingMethod shippingMethod)
        {
            _context.ShippingMethods.Add(shippingMethod);
        }

        public void Delete(ShippingMethod shippingMethod)
        {
            _context.ShippingMethods.Remove(shippingMethod);
        }

        public async Task<IEnumerable<ShippingMethod>> GetAllAsync()
        {
            return await _context.ShippingMethods.Where(x => !x.IsDelete).ToListAsync();
        }

        public async Task<IEnumerable<ShippingMethod>> GetAllContainDeleteAsync()
        {
            return await _context.ShippingMethods.ToListAsync();
        }

        public async Task<PagedList<ShippingMethod>> GetAllShippingMethodsAsync(ShippingParams shippingParams)
        {
            var query = _context.ShippingMethods.Where(x=>!x.IsDelete).AsQueryable();

            if(!shippingParams.SearchString.IsNullOrEmpty())
            {
                query = query.Where(x=>x.Name.ToLower().Contains(shippingParams.SearchString.ToLower()));
            }

            return await PagedList<ShippingMethod>.CreateAsync(query, shippingParams.PageNumber, shippingParams.PageSize);

        }

        public async Task<ShippingMethod?> GetAsync(int id)
        {
            return await _context.ShippingMethods.FindAsync(id);
        }

        public void Update(ShippingMethod shippingMethod)
        {
            var shippingFromDb = _context.ShippingMethods.FirstOrDefault(x=>x.Id == shippingMethod.Id);
            if (shippingFromDb != null)
            {
                shippingFromDb.Cost = shippingMethod.Cost;
                shippingFromDb.Name = shippingMethod.Name;
                shippingFromDb.Description = shippingMethod.Description;
            }
        }
    }
}
