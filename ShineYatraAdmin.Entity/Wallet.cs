using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class WalletRequest
    {
        public string action { get; set; }
        public string member_id { get; set; }
        public string company_id { get; set; }
        public string domain_name { get; set; }
    }

    public class WalletResponse
    {
        public float wallet_balance { get; set; }       
    }


}
