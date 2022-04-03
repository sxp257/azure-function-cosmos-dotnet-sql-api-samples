namespace Sherwin.CosmosDBOrder.Model
{
    internal class OrderHeader
    {
        public string id { get; set; }
        public string order_id { get; set; }
        public string store { get; set; }
        public string status { get; set; }
        public string status_date { get; set; }
        public string origin { get; set; }
        public string pos_control_number { get; set; }
        public string origin_date { get; set; }
        public string order_create_employee { get; set; }
        public string customer_id { get; set; }
        public string customer_contact { get; set; }
        public string currency { get; set; }
        public string total_order_lines { get; set; }
        public string ship_given_name { get; set; }
        public string ship_address1 { get; set; }
        public string ship_city { get; set; }
        public string ship_state { get; set; }
        public string ship_postal { get; set; }
        public string billing_given_name { get; set; }
        public string billing_address2 { get; set; }
        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_postal { get; set; }
        public string contact_phone { get; set; }
        public string order_required_date { get; set; }
        public string src_system_load_date { get; set; }
        public string pos_ord_hdr_pk_guid { get; set; }
        public string pos_ord_hdr_xmit_guid { get; set; }
        public string pos_ord_hdr_version_guid { get; set; }
        public string source_location { get; set; }
        public string run_cycle { get; set; }
    }
}