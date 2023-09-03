using CommonModels;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradesProcessor.DI;
using TradesProcessor.Service;

namespace TradesProcessor.App
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            await serviceProvider.GetService<TradeProcessorService>()!.ProcessTrades();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile($"appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

            ConfigurationResolution.ConfigureLogging(services, configuration);

            ConfigurationResolution.ResolveConfigurationOptions(services, configuration);

            ConfigurationResolution.BindDependancies(services);
        }
    }
}