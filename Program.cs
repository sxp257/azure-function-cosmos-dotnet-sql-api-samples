namespace Sherwin.CosmosDBOrder.Shared
{
    using System;
    using System.Text.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Configuration;
    //using Newtonsoft.Json;
    using Sherwin.CosmosDBOrder.Model;

    internal class Program
    {
        private static readonly string CosmosDatabaseId = "iom";
        private static readonly string headerContainerId = "order_hdr";
        private static readonly string lineContainerId = "order_line";

        private static Database cosmosDatabase = null;

        public static async Task<Order> getOrder(string orderId)
        {
            try
            {

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddJsonFile("C:\\Training\\AzureCosmosDBDotNetV3\\appSettings.json")
                    .Build();
                CosmosClientOptions clientOptions = new CosmosClientOptions();
                clientOptions.ConsistencyLevel = ConsistencyLevel.Eventual;
                clientOptions.ConnectionMode = ConnectionMode.Gateway;
                string endpoint = configuration["EndPointUrl"];
                if (string.IsNullOrEmpty(endpoint))
                {
                    throw new ArgumentNullException("Please specify a valid endpoint in the appSettings.json");
                }
                string authKey = configuration["AuthorizationKey"];
                if (string.IsNullOrEmpty(authKey) || string.Equals(authKey, "Super secret key"))
                {
                    throw new ArgumentException("Please specify a valid AuthorizationKey in the appSettings.json");
                }
                using (CosmosClient client = new CosmosClient(endpoint, authKey, clientOptions))
                {
                    return await Program.RunDemoAsync(client, orderId);
                }
            }
            catch (CosmosException cre)
            {
                Console.WriteLine(cre.ToString());
                return null;
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                return null;
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                //Console.ReadKey();
            }


        }

        private static async Task<Order> RunDemoAsync(CosmosClient client, string orderId)
        {
            cosmosDatabase = client.GetDatabase(CosmosDatabaseId);
            Container headerContainer = await Program.GetContainerSync(cosmosDatabase, headerContainerId);

            //string orderId = "OE0211101A794337";
            return await Program.QueryOrderHdrWithSqlParameters(headerContainer, orderId);

        }

        private static async Task<Container> GetContainerSync(Database database, string containerId)
        {
            return database.GetContainer(containerId);

        }
        private static async Task<Order> QueryOrderHdrWithSqlParameters(Container container, String orderId)
        {
            QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.order_id = @id")
               .WithParameter("@id", orderId);
            using (FeedIterator<OrderHeader> resultSetIterator = container.GetItemQueryIterator<OrderHeader>(
                query,
                requestOptions: new QueryRequestOptions()
                {
                    MaxItemCount = 1
                }
            ))
            {
                if (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<OrderHeader> response = await resultSetIterator.ReadNextAsync();
                    IEnumerable<OrderHeader> orderHeaderEnum = response.Resource;
                    OrderHeader orderheader = orderHeaderEnum.First<OrderHeader>();
                    Order order = Order.newOrder();
                    order.order = orderheader;
                    if (response.Diagnostics != null)
                    {
                        Console.WriteLine($"\nQueryOrderHdrWithSqlParameters:\nQueryWithContinuationTokens Diagnostics: {response.Diagnostics.ToString()}");
                    }
                    Container lineContainer = await Program.GetContainerSync(cosmosDatabase, lineContainerId);
                    return await Program.QueryOrderLineWithSqlParameters(lineContainer, orderId, order);

                }
                return null;
            }
        }

        private static async Task<Order> QueryOrderLineWithSqlParameters(Container container, String orderId, Order order)
        {
            Console.WriteLine(":: Executing QueryOrderLineWithSqlParameters :: ");
            QueryDefinition query = new QueryDefinition("SELECT * FROM c WHERE c.order_id = @id")
               .WithParameter("@id", orderId);
            using (FeedIterator<OrderLine> resultSetIterator = container.GetItemQueryIterator<OrderLine>(
                query
            ))
            {
                while (resultSetIterator.HasMoreResults)
                {
                    FeedResponse<OrderLine> response = await resultSetIterator.ReadNextAsync();
                    IEnumerable<OrderLine> orderLineEnum = response.Resource;
                    Console.WriteLine(":: Executing QueryOrderLineWithSqlParameters before foreach:: ");
                    foreach (OrderLine orderLine in orderLineEnum)
                    {
                        order.AddLines(orderLine);
                    }
                    if (response.Diagnostics != null)
                    {
                        Console.WriteLine($"\nQueryWithSqlParameters Diagnostics: {response.Diagnostics.ToString()}");
                    }
                }

            }
            
                var opt = new JsonSerializerOptions() { WriteIndented = true };
                string strJson = JsonSerializer.Serialize<Order>(order, opt);
                Console.WriteLine(strJson);
                return order;
        }

    }
}