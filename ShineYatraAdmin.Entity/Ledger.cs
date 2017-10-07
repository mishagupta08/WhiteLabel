using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class DistributorLedger
    {
        public int wallet_txn_id { get; set; }
        public DateTime wallet_txn_date { get; set; }
        public DateTime statement_date { get; set; }
        public int service_id { get; set; }
        public string ledger_name { get; set; }
        public string op_ledger_name { get; set; }
        public int credit { get; set; }
        public int debit { get; set; }
        public int balance { get; set; }
        public string remarks { get; set; }
        public string service_name { get; set; }
        public string drcr { get; set; }
        public int ref_txn_id { get; set; }
    }

    public class DistributorLedgerRequest
    {
        public string action { get; set; }
        public string Ledger_id { get; set; }
        public string From_date { get; set; }
        public string To_date { get; set; }
    }

}
