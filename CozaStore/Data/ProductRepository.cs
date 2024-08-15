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

        public async Task<PagedList<Product>> GetAllProductsAsync(ProductParams productParams)
        {
            var query = _context.Products
                    .Include(x => x.Variants)
                    .Include(x => x.Images)
                    .AsQueryable();

            if (!string.IsNullOrEmpty(productParams.SearchString))
            {
                productParams.SearchString = productParams.SearchString.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(productParams.SearchString)
                    || x.Id.ToString() == productParams.SearchString
                    || x.Variants.Any(v => v.Id.ToString() == productParams.SearchString));
            }

            if (productParams.SelectedColor > 0) query = query.Where(x => x.Variants.Any(x => x.ColorId == productParams.SelectedColor));
            if (productParams.SelectedSize > 0) query = query.Where(x => x.Variants.Any(x => x.SizeId == productParams.SelectedSize));

            if (productParams.SelectedCategory != null && productParams.SelectedCategory.Count > 0)
            {
                query = query.Where(x => x.ProductCategories
                              .Any(pc => productParams.SelectedCategory.Contains(pc.CategoryId)));
            }

            if (!string.IsNullOrEmpty(productParams.SelectedPriceRange))
            {
                query = productParams.SelectedPriceRange switch
                {
                    "$0-$50" => query.Where(x => (x.Price >= 0 && x.Price <= 50) || (x.PriceSell >= 0 && x.PriceSell <= 50)
                    || x.Variants.Any(x => (x.Price >= 0 && x.Price <= 50) || (x.PriceSell >= 0 && x.PriceSell <= 50))),
                    "$50-$100" => query.Where(x => (x.Price > 50 && x.Price <= 100) || (x.PriceSell > 50 && x.PriceSell <= 100)
                    || x.Variants.Any(x => (x.Price > 50 && x.Price <= 100) || (x.PriceSell > 50 && x.PriceSell <= 100))),
                    "$100-$150" => query.Where(x => (x.Price > 100 && x.Price <= 150) || (x.PriceSell > 100 && x.PriceSell <= 150)
                    || x.Variants.Any(x => (x.Price > 100 && x.Price <= 150) || (x.PriceSell > 100 && x.PriceSell <= 150))),
                    "$150-$200" => query.Where(x => x.Price > 150 && x.Price <= 200),
                    "$200+" => query.Where(x => x.Price > 200 || x.PriceSell > 200 ||
                    x.Variants.Any(x => x.Price > 200 || x.PriceSell > 200)),
                    _ => query
                };
            }



            query = productParams.SortOrder switch
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

            if (productParams.Page < 1) productParams.Page = 1;

            return await PagedList<Product>.CreateAsync(query, productParams.Page, productParams.PageNumber);
        }

        public async Task<IEnumerable<Color?>> GetAvailableColorsAsync(int productId, int sizeId)
        {
            return await _context.Variants.Include(x=>x.Color).Include(x=>x.Size)
                    .Where(v => v.ProductId == productId && v.Size.Id == sizeId)
                    .Select(v => v.Color)
                    .Distinct()
                    .ToListAsync() ?? [];
        }

        public async Task<IEnumerable<Size?>> GetAvailableSizesAsync(int productId, int colorId)
        {
            return await _context.Variants.Include(x => x.Color).Include(x => x.Size)
                    .Where(v => v.ProductId == productId && v.Color.Id == colorId)
                    .Select(v => v.Size)
                    .Distinct()
                    .ToListAsync() ?? [];
        }

        public async Task<Variant?> GetPriceOfProductAsync(int productId, int colorId, int sizeId)
        {
            var variant = await _context.Variants
                .Include(x => x.Color)
                .Include(x => x.Size)
                .FirstOrDefaultAsync(v => v.ProductId == productId && v.SizeId == sizeId && v.ColorId == colorId) ?? null;

            return variant;

        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await _context.Products
            .Include(x => x.Images)
            .Include(x => x.Variants)
            .Include(x => x.ProductCategories)
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
            var productFromDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (productFromDb != null)
            {
                if (productFromDb.Status == (int)ProductStatus.Deleted)
                    productFromDb.Status = (int)ProductStatus.Private;
                else
                    productFromDb.Status = (int)ProductStatus.Deleted;
            }
        }

        public void UpdateProduct(Product product)
        {
            var productFromDb = _context.Products.FirstOrDefault(x => x.Id == product.Id);
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
