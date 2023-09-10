using CommonModels;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TradesProcessor.DI;
using TradesProcessor.Service;

namespace TradesProcessor.App
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = AppStartup();

            var service = ActivatorUtilities.CreateInstance<TradeProcessorService>(host.Services);

            await service.ProcessTrades();
        }

        static IHost AppStartup()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    ConfigurationResolution.ConfigureLogging(services, context.Configuration);

                    ConfigurationResolution.ResolveConfigurationOptions(services, context.Configuration);

                    ConfigurationResolution.BindDependancies(services);
                })
                .UseSerilog()
                .Build();

            return host;
        }
    }
}