using CozaStore.Data;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.Services;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            // Add services to the container.
            services.AddControllersWithViews();
            services.AddDbContext<DataContext>(option =>
                option.UseSqlServer(config.GetConnectionString("DefaultConnection")));


            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IVariantRepository, VariantRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

			services.AddSignalR();

			return services;
        }
    }
}
