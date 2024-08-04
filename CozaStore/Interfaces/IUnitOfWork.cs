namespace CozaStore.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        ISubCategoryRepository SubCategoryRepository { get; }
        ISizeRepository SizeRepository { get; }
        IColorRepository ColorRepository { get; }

        Task<bool> Complete();
    }
}
