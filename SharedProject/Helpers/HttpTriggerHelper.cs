using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SharedProject.Helpers
{
    public static class HttpTriggerHelper
    {
        public static IDictionary<string, string> GetQuery(HttpRequestData req)
        {

            bool has_queries = req.FunctionContext.BindingContext.BindingData.TryGetValue("Query", out object queries_obj);


            IDictionary<string, string> result = new Dictionary<string, string>();

            if (has_queries)
            {
                var queries = JsonConvert.DeserializeObject<IDictionary<string, string>>(queries_obj.ToString());

                foreach (var item in queries)
                {
                    result.Add(item.Key, item.Value.ToString());
                }

            }

            return result;

        }

        public async static Task<T> GetBodyAsync<T>(HttpRequestData req)
        {

            if (req.Body is not null)
            {
                var result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(req.Body);

                return result;
            }
            else
            {
                return default(T);
            }
        }

        public async static Task<string> GetBodyStringAsync(HttpRequestData req)
        {

            if (req.Body is not null)
            {
                string result_str = string.Empty;
                using (StreamReader streamReader = new StreamReader(req.Body))
                {
                    result_str = await streamReader.ReadToEndAsync();
                }

                return result_str;
            }
            else
            {
                return null;
            }
        }


        public static async Task<HttpResponseData> CreateResponseAsync(this HttpRequestData request, HttpStatusCode statusCode, object data)
        {

            var result = request.CreateResponse(statusCode);

            result.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await result.WriteStringAsync(JsonConvert.SerializeObject(data));

            return result;
        }

        public static async Task<HttpResponseData> CreateResponseAsync(this HttpRequestData request, HttpStatusCode statusCode, string message)
        {

            var result = request.CreateResponse(statusCode);

            result.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await result.WriteStringAsync(message);

            return result;
        }

    }
}
