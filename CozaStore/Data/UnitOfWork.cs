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
        private readonly IProductRepository _productRepository;
        public UnitOfWork(DataContext context, ICategoryRepository categoryRepository,
            ISubCategoryRepository subCategoryRepository, ISizeRepository sizeRepository, 
            IColorRepository colorRepository, IProductRepository productRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
            _productRepository = productRepository;
        }
        public ICategoryRepository CategoryRepository => _categoryRepository;
        public ISubCategoryRepository SubCategoryRepository => _subCategoryRepository;

        public ISizeRepository SizeRepository => _sizeRepository;

        public IColorRepository ColorRepository => _colorRepository;

        public IProductRepository ProductRepository => _productRepository;

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
