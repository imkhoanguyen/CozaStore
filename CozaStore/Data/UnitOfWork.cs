using CozaStore.Interfaces;

namespace CozaStore.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        public UnitOfWork(DataContext context, ICategoryRepository categoryRepository,
            ISubCategoryRepository subCategoryRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
        }
        public ICategoryRepository CategoryRepository => _categoryRepository;
        public ISubCategoryRepository SubCategoryRepository => _subCategoryRepository;


        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
