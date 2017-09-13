using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class FundRequestContainer
    {
        public string status { get; set; }
        public string txn_id { get; set; }

        public string action { get; set; }
        public string txn_type { get; set; }
        public string member_id { get; set; }
        public float amount { get; set; }

        public string remarks { get; set; }
        public string domain_name { get; set; }
        public string request_token { get; set; }

        public string ref_no { get; set; }
    }
}
