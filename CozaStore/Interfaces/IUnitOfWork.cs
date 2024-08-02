namespace CozaStore.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }

        Task<bool> Complete();
    }
}
