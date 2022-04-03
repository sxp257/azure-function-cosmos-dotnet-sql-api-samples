namespace Sherwin.CosmosDBOrder.Model
{
    using System.Collections.Generic;
    internal class Order
    {
        public OrderHeader order { get; set; }
        public List<OrderLine> lines { get; private set; }
        public void AddLines(OrderLine line)
        {
            if (lines != null)
                lines.Add(line);
            else
            {
                lines = new List<OrderLine>();
                lines.Add(line);
            }
        }

        public static Order newOrder()
        {
            return new Order();
        }

    }
}