using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SharedProject.Helpers;

namespace PaymentFunction.Triggers
{
    public class HttpTriggers
    {
        private readonly ILogger _logger;

        public HttpTriggers(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTriggers>();

        }


        [Function("HttpTriggers")]
        public async Task<HttpResponseData> Test(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var request_dic = HttpTriggerHelper.GetQuery(req);

            request_dic.TryGetValue("code", out string code);

            var bus_client = new ServiceBusClient("Endpoint=sb://sb-odw-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=vA3H8Jj9K+a7k/KpjwcMYstYzZs57Rvg+WP1QykosC8=");


            var sender = bus_client.CreateSender("topic-test2");

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int i = 1; i <= 2; i++)
            {
                ServiceBusMessage message = new ServiceBusMessage($"message {i}")
                {
                    Subject = "update",
                   
                };

                // try adding a message to the batch
                if (!messageBatch.TryAddMessage(message))
                {
                    // if it is too large for the batch
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }

            try
            {
                // Use the producer client to send the batch of messages to the Service Bus topic
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {5} messages has been published to the topic.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await bus_client.DisposeAsync();
            }

            var response = await req.CreateResponseAsync(HttpStatusCode.OK, "전송완료");

            return response;
        }
    }
}
