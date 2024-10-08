﻿using CozaStore.Interfaces;

namespace CozaStore.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IProductRepository _productRepository;
        private readonly IVariantRepository _variantRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShippingRepository _shippingRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;
        public UnitOfWork(DataContext context, ICategoryRepository categoryRepository,
             ISizeRepository sizeRepository, IColorRepository colorRepository,
             IProductRepository productRepository, IVariantRepository variantRepository, 
             IShoppingCartRepository shoppingCartRepository, IShippingRepository shippingRepository,
             IOrderRepository orderRepository, IReviewRepository reviewRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _shippingRepository = shippingRepository;
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }
        public ICategoryRepository CategoryRepository => _categoryRepository;

        public ISizeRepository SizeRepository => _sizeRepository;

        public IColorRepository ColorRepository => _colorRepository;

        public IProductRepository ProductRepository => _productRepository;

        public IVariantRepository VariantRepository => _variantRepository;

        public IShoppingCartRepository ShoppingCartRepository => _shoppingCartRepository;

        public IShippingRepository ShippingRepository => _shippingRepository;

        public IOrderRepository OrderRepository => _orderRepository;

		public IReviewRepository ReviewRepository => _reviewRepository;

		public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
