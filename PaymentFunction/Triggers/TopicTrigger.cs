using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PaymentFunction
{
    public class TopicTrigger
    {
        private readonly ILogger _logger;

        public TopicTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TopicTrigger>();
        }

        [Function("TopicReceiver")]
        public void Run([ServiceBusTrigger("topic-test2", "shopeeorder", Connection = "subscription")] string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

        }


        [Function("TopicReceiver2")]
        public void Run2([ServiceBusTrigger("topic-test2", "shopeeorderupdate", Connection = "subscription")] string mySbMsg)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

        }
    }
}
