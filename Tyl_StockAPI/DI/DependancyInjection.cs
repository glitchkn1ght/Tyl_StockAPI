using Stock_API.ServiceBus;

namespace Stock_API.DI
{
    public static class DependancyInjection
    {
        public static IServiceCollection BindDependancies(this IServiceCollection services)
        {
            services.AddScoped<IServiceBusPublisher, ServiceBusPublisher>();

            return services;
        }
    }
}
