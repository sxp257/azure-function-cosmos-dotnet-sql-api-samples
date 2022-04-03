namespace Sherwin.CosmosDBOrder.Model
{
    internal class OrderLine
    {
        public string id { get; set; }
        public string line_id { get; set; }
        public string order_id { get; set; }
        public string line_number { get; set; }
        public string lineUOM { get; set; }
        public string pos_line_sales_number { get; set; }
        public string status { get; set; }
        public string status_date { get; set; }
        public string quantity { get; set; }
        public string unit_cost { get; set; }
        public string promise_delivery_date { get; set; }
        public string required_delivery_date { get; set; }
        public string store { get; set; }
        public string pos_ord_line_pk_guid { get; set; }
        public string pos_ord_line_xmit_guid { get; set; }
        public string pos_ord_line_version_guid { get; set; }
        public string run_cycle { get; set; }
    }
}