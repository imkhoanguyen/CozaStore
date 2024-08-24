namespace CozaStore.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        ISizeRepository SizeRepository { get; }
        IColorRepository ColorRepository { get; }

        IProductRepository ProductRepository { get; }
        IVariantRepository VariantRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IShippingRepository ShippingRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task<bool> Complete();
    }
}
