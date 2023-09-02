using Stock_API.Interfaces;
using Stock_API.Service;
using Stock_API.ServiceBus;

namespace Stock_API.DI
{
    public static class DependancyInjection
    {
        public static IServiceCollection BindDependancies(this IServiceCollection services)
        {
            services.AddScoped<IStockValidationService, StockValidationService>(); 
            
            services.AddScoped<IStockService, StockService>();

            services.AddScoped<IServiceBusPublisher, ServiceBusPublisher>();

            return services;
        }
    }
}
