using Azure.Messaging.ServiceBus;
using ItemFunction.Options;
using ItemSDK.Interfaces;
using ItemSDK.Models;
using ItemSDK.Services;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace ItemFunction
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

                     services.AddDbContext<ItemContext>(e =>
                     {
                         e.UseCosmos(
                             context.Configuration.GetSection("Item")["AccountEndPoint"],
                             context.Configuration.GetSection("Item")["AccountKey"],
                             context.Configuration.GetSection("Item")["DatabaseName"]
                         );
                     });

                   
                     services.AddTransient<IItemService, ItemService>();

                     services.Configure<ServiceBusOption>(context.Configuration.GetSection("ServiceBus"));

                     services.AddHttpClient();
                     services.AddLogging();


                 })
                 .Build();

            await host.RunAsync();
        }
    }
}