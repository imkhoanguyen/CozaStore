using CozaStore.Models;

namespace CozaStore.Interfaces
{
    public interface IShoppingCartRepository
    {
        void Add(ShoppingCart shoppingCart);
        void UpdatteIncrease(ShoppingCart shoppingCart);
        Task<ShoppingCart?> GetShoppingCartAsync(string userId, int productId, int sizeId, int colorId);
        Task<ShoppingCart?> GetShoppingCartAsync(int cartId);
        Task<IEnumerable<ShoppingCart>> GetAllAsync(string userId);
        void Delete(ShoppingCart shoppingCart);
    }
}
