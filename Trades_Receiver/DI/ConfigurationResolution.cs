using Azure.Messaging.ServiceBus;
using CommonModels;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Trades_Receiver.DAL.Repositories;
using TradesProcessor.DAL.Polly;
using TradesProcessor.Interfaces;
using TradesProcessor.Models;
using TradesProcessor.Models.Configuration;
using TradesProcessor.Service;

namespace TradesProcessor.DI
{
    static class ConfigurationResolution
    {
        public static void ResolveConfigurationOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SqlConnectionSettings>(configuration.GetSection("SQLConnections"));

            services.Configure<ServiceBusConfig>(configuration.GetSection("ServiceBus"));

            services.Configure<TradesRepositorySettings>(configuration.GetSection("TradesRepository"));
        }

        public static void ConfigureLogging(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(builder => builder.AddSerilog(
                 new LoggerConfiguration()
                 .ReadFrom.Configuration(configuration)
                 .CreateLogger()))
                 .BuildServiceProvider();
        }

        public static void BindDependancies(IServiceCollection services)
        {
            services.AddSingleton((serviceProvider) =>
            {
                var options = serviceProvider.GetService<IOptions<ServiceBusConfig>>()!.Value;
                return new ServiceBusClient(options.ConnectionString);
            });

            services.AddScoped<IPollyRetryPolicy, PollyRetryPolicy>();
            services.AddScoped<IPollyConnectionFactory, PollySqlConnectionFactory>();
            services.AddScoped<ITradesRepository, TradesRepository>();
            services.AddScoped<TradeProcessorService>();
        }
    }
}