namespace CozaStore.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        ISizeRepository SizeRepository { get; }
        IColorRepository ColorRepository { get; }

        IProductRepository ProductRepository { get; }
        IVariantRepository VariantRepository { get; }
        Task<bool> Complete();
    }
}
