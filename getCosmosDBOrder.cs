using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sherwin.CosmosDBOrder.Shared;
using Sherwin.CosmosDBOrder.Model;

namespace Sherwin.CosmosDBOrder
{
    public class getCosmosDBOrder
    {
        [FunctionName("getCosmosDBOrder")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string order_id = req.Query["order_id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            order_id = order_id ?? data?.order_id;

            string responseMessage = string.IsNullOrEmpty(order_id)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {order_id}. This HTTP triggered function executed successfully.";
            Order order = await Program.getOrder(order_id);
            var opt = new System.Text.Json.JsonSerializerOptions() { WriteIndented = true };
            string orderResponse = System.Text.Json.JsonSerializer.Serialize<Order>(order, opt);
            
            if (orderResponse != null){
                return new OkObjectResult(orderResponse);
            }
                
            return new BadRequestObjectResult(responseMessage);
        }
    }
}
