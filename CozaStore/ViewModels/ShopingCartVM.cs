using CozaStore.Models;

namespace CozaStore.ViewModels
{
    public class ShopingCartVM
    {
        public List<ShoppingCart> ShoppingCartList { get; set; } = [];
        public Order? Order { get; set; }
        public List<Address> ListAddress { get; set; } = [];
        public List<ShippingMethod> ListShippingMethod { get; set; } = [];
    }
}
