using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Serilog;
using Stock_API.Interfaces;
using Stock_API.Models;
using Stock_API.Service;
using Stock_API.ServiceBus;
using Stock_API.Validation;

namespace Stock_API.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((hostingContext, loggerConfig) =>
                 loggerConfig.ReadFrom.Configuration(hostingContext.Configuration));

            // Add services to the container.
            var serviceBusConfig = builder.Configuration.GetSection("ServiceBus");
            builder.Services.Configure<ServiceBusConfig>(serviceBusConfig);

            builder.Services.AddSingleton((serviceProvider) =>
            {
                var options = serviceProvider.GetService<IOptions<ServiceBusConfig>>()!.Value;
                return new ServiceBusClient(options.ConnectionString);
            });

            builder.Services.AddScoped<IModelStateErrorMapper, ModelStateErrorMapper>();

            builder.Services.AddScoped<IStockService, StockService>();

            builder.Services.AddScoped<IServiceBusPublisher, ServiceBusPublisher>();

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(a =>
            {
                a.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.Run();
        }
    }
}