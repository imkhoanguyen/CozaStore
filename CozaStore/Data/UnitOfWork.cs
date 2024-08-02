using CozaStore.Interfaces;

namespace CozaStore.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;
        public UnitOfWork(DataContext context, ICategoryRepository categoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
        }
        public ICategoryRepository CategoryRepository => _categoryRepository;

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
