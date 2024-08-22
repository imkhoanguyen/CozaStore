using CozaStore.Interfaces;
using CozaStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace CozaStore.Data
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly DataContext _context;

        public ShoppingCartRepository(DataContext context)
        {
            _context = context;
        }
        public void Add(ShoppingCart shoppingCart)
        {
            _context.ShoppingCarts.Add(shoppingCart);
        }

        public void Delete(ShoppingCart shoppingCart)
        {
            _context.ShoppingCarts.Remove(shoppingCart);
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string userId)
        {
            return await _context.ShoppingCarts.ToListAsync();
        }

        public async Task<ShoppingCart?> GetShoppingCartAsync(string userId, int productId, int sizeId, int colorId)
        {
            return await _context.ShoppingCarts.Include(x=>x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId
            && x.SizeId == sizeId && x.ColorId == colorId);
        }

        public async Task<ShoppingCart?> GetShoppingCartAsync(int cartId)
        {
            return await _context.ShoppingCarts.FindAsync(cartId);
        }

        public void UpdatteIncrease(ShoppingCart shoppingCart)
        {
            var shoppingCartFromDb = _context.ShoppingCarts.FirstOrDefault(x => x.UserId == shoppingCart.UserId 
            && x.ProductId == shoppingCart.ProductId && x.SizeId == shoppingCart.SizeId && x.ColorId == shoppingCart.ColorId);

            if (shoppingCartFromDb != null)
            {
                shoppingCartFromDb.Count += shoppingCart.Count;
                shoppingCartFromDb.Price += shoppingCart.Price;
            }
        }
    }
}
