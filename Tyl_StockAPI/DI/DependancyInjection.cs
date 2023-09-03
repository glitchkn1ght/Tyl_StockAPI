using Azure.Messaging.ServiceBus;
using CommonModels;
using Microsoft.Extensions.Options;
using Stock_API.Interfaces;
using Stock_API.Service;
using Stock_API.ServiceBus;

namespace Stock_API.DI
{
    public static class DependancyInjection
    {
        public static IServiceCollection BindDependancies(this IServiceCollection services)
        {
            services.AddSingleton((serviceProvider) =>
            {
                var options = serviceProvider.GetService<IOptions<ServiceBusConfig>>()!.Value;
                return new ServiceBusClient(options.ConnectionString);
            });

            services.AddScoped<ISymbolValidationService, SymbolValidationService>(); 
            
            services.AddScoped<IStockService, StockService>();

            services.AddScoped<IServiceBusPublisher, ServiceBusPublisher>();

            return services;
        }
    }
}
