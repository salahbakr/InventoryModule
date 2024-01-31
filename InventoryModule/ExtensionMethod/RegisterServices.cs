using InventoryModule.AutoMapper;
using InventoryModule.ExceptionHandling;
using InventoryModule.Interfaces;
using InventoryModule.Repository;
using InventoryModule.Services;
using System.Runtime.CompilerServices;

namespace InventoryModule.ExtensionMethod
{
    public static class RegisterServices
    {
        public static IServiceCollection AddRegisteredServices(this IServiceCollection services) 
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IShelfRepository), typeof(ShelfRepository));
            services.AddScoped(typeof(IItemRepository), typeof(ItemRepository));
            services.AddScoped(typeof(IRequestRepository), typeof(RequestRepository));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddTransient(typeof(ICategoryService), typeof(CategoryService));
            services.AddTransient(typeof(IShelfService), typeof(ShelfService));
            services.AddTransient(typeof(IItemService), typeof(ItemsService));
            services.AddTransient(typeof(IRequestService), typeof(RequestService));
            services.AddTransient(typeof(IOrderService), typeof(OrderService));

            //services.AddExceptionHandler<GlobalExceptionHandling>();

            return services;
        }
    }
}
