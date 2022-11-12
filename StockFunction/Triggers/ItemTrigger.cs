using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace StockFunction
{
    public class ItemTrigger
    {
        private readonly ILogger _logger;

        public ItemTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ItemTrigger>();
        }

        [Function("ItemInsertedTrigger")]
        public void Run([ServiceBusTrigger("item", "inserted", Connection = "Warehouse")] string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
