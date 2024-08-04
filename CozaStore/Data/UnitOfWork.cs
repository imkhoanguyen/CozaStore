using CozaStore.Interfaces;

namespace CozaStore.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        public UnitOfWork(DataContext context, ICategoryRepository categoryRepository,
            ISubCategoryRepository subCategoryRepository, ISizeRepository sizeRepository, IColorRepository colorRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
        }
        public ICategoryRepository CategoryRepository => _categoryRepository;
        public ISubCategoryRepository SubCategoryRepository => _subCategoryRepository;

        public ISizeRepository SizeRepository => _sizeRepository;

        public IColorRepository ColorRepository => _colorRepository;

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
