using CozaStore.Models;

namespace CozaStore.ViewModels
{
    public class ShopingCartVM
    {
        public List<ShoppingCart> ShoppingCartList { get; set; } = [];
        public decimal Total { get; set; }
    }
}
