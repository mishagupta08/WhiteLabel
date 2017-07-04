using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class CompanyFund
    {
        /// <summary>
        /// gets or sets action
        /// </summary>
        public string action { get; set; }
        public int service_id { get; set; }
        public string txn_type{ get; set; }
        public string company_id { get; set; }
        public string  member_id{ get; set; }
        public float amount { get; set; }
        public string deposit_mode { get; set; }
        public string remarks{ get; set; }
        public int cancel_request_id { get; set; }
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
