using System.Collections.Generic;

namespace ShineYatraAdmin.Entity
{
    public class CompanyFund
    {
        /// <summary>
        /// gets or sets action
        /// </summary>
        public string action { get; set; }
        public int service_id { get; set; }
        public string txn_type { get; set; }
        public string company_id { get; set; }
        public string member_id { get; set; }
        public double amount { get; set; }
        public string deposit_mode { get; set; }
        public string remarks { get; set; }
        public string domain_name { get; set; }
        public string request_token { get; set; }
        public int txn_id { get; set; }
        public string txn_date { get; set; }              
        public string status { get; set; }        
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string sub_service_name { get; set; }
        public int cancel_request_id { get; set; }
        public TransactionDetail TransactionDetail { get; set; }

        /// <summary>
        /// gets or sets search list
        /// </summary>
        public IList<KeyValuePair> DepositModeList { get; set; }

        /// <summary>
        /// method to assign setting 
        /// </summary>
        public void AssignDepositModeList()
        {
            this.DepositModeList = new List<KeyValuePair>();

            this.DepositModeList.Add(new KeyValuePair
            {
                Id = "0",
                Value = "Please Select"
            });

            this.DepositModeList.Add(new KeyValuePair
            {
                Id = "NEFT/RTGS",
                Value = "NEFT/RTGS"
            });

            this.DepositModeList.Add(new KeyValuePair
            {
                Id = "IMPS",
                Value = "IMPS"
            });

            this.DepositModeList.Add(new KeyValuePair
            {
                Id = "CASH",
                Value = "CASH"
            });

            this.DepositModeList.Add(new KeyValuePair
            {
                Id = "CHEQUE",
                Value = "CHEQUE"
            });

            this.DepositModeList.Add(new KeyValuePair
            {
                Id = "OTHERS",
                Value = "OTHERS"
            });

        }
    }
}
