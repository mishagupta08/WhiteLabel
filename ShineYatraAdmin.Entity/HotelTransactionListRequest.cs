using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class HotelTransactionListRequest
    {
        public string action { get; set; }
        public string txn_id { get; set; }
        public string member_id { get; set; }
        public string status { get; set; }
        public string check_in_date { get; set; }
        public string check_out_date { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }
}
