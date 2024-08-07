﻿using CozaStore.Models;
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
            var query = _context.Products.OrderByDescending(x=>x.Status == (int)ProductStatus.Public)
                .ThenByDescending(x=> x.Status == (int)ProductStatus.Private).ThenByDescending(x=>x.Id)
                .Include(x=> x.Variants)
                .Include(x=>x.Images)
                .AsQueryable();

            if(searchString != null)
            {
                query = query.Where(x=> x.Name.ToLower().Contains(searchString.ToLower())
                || x.Id.ToString() == searchString
                || x.Variants.Any(x=>x.Id.ToString() == searchString));
            }


            if (page < 1) page = 1;
            return await PagedList<Product>.CreateAsync(query, page, 10);
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await _context.Products
            .Include(x => x.Images)
            .Include(x => x.Category)
            .Include(x => x.Variants!)
                .ThenInclude(x => x.Color!)
            .Include(x => x.Variants!)
                .ThenInclude(x => x.Size!)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public void ToggleProductStatus(Product product)
        {
            var productFromDb = _context.Products.FirstOrDefault(x=>x.Id == product.Id);
            if (productFromDb != null)
            {
                if(productFromDb.Status == (int)ProductStatus.Deleted) productFromDb.Status = (int)ProductStatus.Private;
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
                productFromDb.DisplayPrice = product.DisplayPrice;
                productFromDb.IsFeatured = product.IsFeatured;
                productFromDb.Status = product.Status;
            }
        }
    }
}