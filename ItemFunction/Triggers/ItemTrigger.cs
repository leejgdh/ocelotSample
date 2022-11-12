using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using ItemFunction.Options;
using ItemSDK.Interfaces;
using ItemSDK.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedProject.Helpers;

namespace ItemFunction
{
    public class ItemTrigger
    {
        private readonly ILogger _logger;
        private IItemService _itemService;

        private ServiceBusOption _serviceBusOption;

        public ItemTrigger(
            ILoggerFactory loggerFactory,
            IItemService itemService,
            IOptionsSnapshot<ServiceBusOption> serviceBusOption
            )
        {
            _logger = loggerFactory.CreateLogger<ItemTrigger>();
            _itemService = itemService;
            _serviceBusOption = serviceBusOption.Value;

        }

        [Function("Insert")]
        public async Task<HttpResponseData> Insert([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route ="item")] HttpRequestData req)
        {
            var item = await HttpTriggerHelper.GetBodyAsync<Item>(req);

            var insertRes = await _itemService.InsertAsync(item);

            if (insertRes.IsSuccess)
            {
                //메시지 전송

                var serviceBusClient = new ServiceBusClient(_serviceBusOption.ConnectionString);

                var sender = serviceBusClient.CreateSender("item");

                ServiceBusMessage message = new ServiceBusMessage(JsonConvert.SerializeObject(new
                {
                    ItemId = insertRes.Result.ItemId,
                    VariationSku = insertRes.Result.VariationSku
                }))
                {
                    Subject = "inserted"
                };

                await sender.SendMessageAsync(message);
                await sender.DisposeAsync();
                await serviceBusClient.DisposeAsync();


                return await req.CreateResponseAsync(HttpStatusCode.Created, insertRes.Result);
            }
            else
            {
                return await req.CreateResponseAsync(HttpStatusCode.BadRequest, insertRes.Message);
            }
        }

        [Function("Update")]
        public async Task<HttpResponseData> Update([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "item/{id}")] HttpRequestData req)
        {
            var item = await HttpTriggerHelper.GetBodyAsync<Item>(req);

            var updateRes = await _itemService.UpdateAsync(item);

            if (updateRes.IsSuccess)
            {

                return await req.CreateResponseAsync(HttpStatusCode.Created, updateRes.Result);
            }
            else
            {
                return await req.CreateResponseAsync(HttpStatusCode.BadRequest, updateRes.Message);
            }
        }
    }
}
