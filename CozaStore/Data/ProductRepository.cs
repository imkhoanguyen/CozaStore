using CozaStore.Models;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using CozaStore.Helpers.Enum;

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

        public async Task<PagedList<Product>> GetAllProductsAsync(string sortOrder, string searchString, int categoryId, int status, string priceRange, int sizeId, int colorId, int page, int pageSize)
        {
            var query = _context.Products
                    .Include(x => x.Variants)
                    .Include(x => x.Images)
                    .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchString)
                    || x.Id.ToString() == searchString
                    || x.Variants.Any(v => v.Id.ToString() == searchString));
            }

            if (colorId > 0) query = query.Where(x => x.Variants.Any(x => x.ColorId == colorId));
            if (sizeId > 0) query =  query.Where(x => x.Variants.Any(x => x.SizeId == sizeId));

            if (!string.IsNullOrEmpty(priceRange))
            {
                query = priceRange switch
                {
                    "$0-$50" => query.Where(x => x.Price >= 0 && x.Price <= 50),
                    "$50-$100" => query.Where(x => x.Price > 50 && x.Price <= 100),
                    "$100-$150" => query.Where(x => x.Price > 100 && x.Price <= 150),
                    "$150-$200" => query.Where(x => x.Price > 150 && x.Price <= 200),
                    "$200+" => query.Where(x => x.Price > 200),
                    _ => query
                };
                
            }

            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(x => x.Name),
                "name" => query.OrderBy(x => x.Name),
                "id_desc" => query.OrderByDescending(x => x.Id),
                "id" => query.OrderBy(x => x.Id),
                "price_desc" => query.OrderBy(x => x.Price),
                "price" => query.OrderByDescending(x => x.Price),
                "status_desc" => query.OrderByDescending(x => x.Status == (int)ProductStatus.Public).ThenBy(x => x.Status == (int)ProductStatus.Private),
                "status" => query.OrderBy(x => x.Status == (int)ProductStatus.Public).ThenBy(x => x.Status == (int)ProductStatus.Private),
                _ => query.OrderByDescending(x => x.Id)
            };

            if (page < 1) page = 1;

            return await PagedList<Product>.CreateAsync(query, page, 10);
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await _context.Products
            .Include(x => x.Images)
            .Include(x => x.Variants)
            .Include(x=>x.ProductCategories)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product?> GetProductDetailAsync(int id)
        {
            return await _context.Products
                .Include(x => x.Images)
                .Include(x => x.Variants)
                    .ThenInclude(v => v.Color)
                .Include(x => x.Variants)
                    .ThenInclude(v => v.Size)
                .Include(x => x.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void ToggleProductStatus(Product product)
        {
            var productFromDb = _context.Products.FirstOrDefault(x=>x.Id == product.Id);
            if (productFromDb != null)
            {
                if(productFromDb.Status == (int)ProductStatus.Deleted) 
                    productFromDb.Status = (int)ProductStatus.Private;
                else 
                    productFromDb.Status = (int)ProductStatus.Deleted;
            } 
        }

        public void UpdateProduct(Product product)
        {
            var productFromDb = _context.Products.FirstOrDefault(x =>x.Id == product.Id);
            if (productFromDb != null)
            {
                productFromDb.Name = product.Name;
                productFromDb.Description = product.Description;
                productFromDb.Price = product.Price;
                productFromDb.PriceSell = product.PriceSell;
                productFromDb.Quantity = product.Quantity;
                productFromDb.IsFeatured = product.IsFeatured;
                productFromDb.Status = product.Status;

            }
        }
    }
}
