using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StockFunction.Options;
using StockSDK.Interfaces;
using StockSDK.Models;
using StockSDK.Services;
using System.Threading.Tasks;

namespace StockFunction
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = new HostBuilder()
                 .ConfigureAppConfiguration(configurationBuilder =>
                 {
                     configurationBuilder.AddCommandLine(args);

                 })
                 .ConfigureFunctionsWorkerDefaults()
                 .ConfigureServices((context, services) =>
                 {
                     string ee = context.Configuration.GetConnectionString("Stock");

                     services.AddDbContext<StockContext>(e =>
                     {
                         e.UseSqlServer(context.Configuration.GetConnectionString("Stock"),b => b.MigrationsAssembly("StockFunction"));
                     });

                     services.AddTransient<IStockService, StockService>();
                     services.AddTransient<IStockHistoryService, StockHistoryService>();

                     //services.Configure<ServiceBusOption>(context.Configuration.GetSection("ServiceBus"));

                     services.AddHttpClient();
                     services.AddLogging();


                 })
                 .Build();

            await host.RunAsync();
        }
    }
}