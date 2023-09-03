using Azure.Messaging.ServiceBus;
using CommonModels.Config;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            //services.AddAzureClients(builder =>
            //{
            //    builder.AddServiceBusClient("Endpoint=sb://tylstockapi.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yZVrxRFLW1ZPdFQIJNyVeYzOibHSZmJYR+ASbKl6qF0=");
            //});

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
            services.AddScoped<IPollyRetryPolicy, PollyRetryPolicy>();
            services.AddScoped<IPollyConnectionFactory, PollySqlConnectionFactory>();
            services.AddScoped<ITradesRepository, TradesRepository>();
            services.AddScoped<TradeProcessorService>();
        }
    }
}