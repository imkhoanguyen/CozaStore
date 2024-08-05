using CozaStore.Entities;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task<PagedList<Product>> GetAllProductsAsync(string searchString, int page = 1)
        {
            var query = _context.Products
                .Include(x=> x.Variants)
                .Include(x=>x.Images)
                .AsQueryable();

            if(searchString != null)
            {
                query = query.Where(x=> x.Name.ToLower().Contains(searchString.ToLower())
                || x.Id.ToString() == searchString);
            }


            if (page < 1) page = 1;
            return await PagedList<Product>.CreateAsync(query, page, 10);
        }
    }
}
