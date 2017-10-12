namespace ShineYatraAdmin.Entity
{
    public class InsertServiceRechargeRequest
    {
        public string action { get; set; }
        public string domain_name { get; set; }
        public string request_token { get; set; }
        public string txn_type { get; set; }
        public string category { get; set; }
        public string member_id { get; set; }
        public float amount { get; set; }
        public string service_id { get; set; }
        public string sub_service_id { get; set; }
        public string account_number { get; set; }
        public string std_code { get; set; }
        public string recharge_number { get; set; }
        public string circle_name { get; set; }
        public string remarks { get; set; }
        public string my_info { get; set; }
    }

    public class InsertServiceRechargeResponse
    {
        public string txn_id { get; set; }        
    }
}
