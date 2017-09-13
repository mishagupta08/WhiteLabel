using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class TransactionDetail
    {
        public string action { get; set; }

        /// <summary>
        /// Gets or sets username
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// gets or sets password
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Gets or sets txnData
        /// </summary>
        public string txnData { get; set; }

        /// <summary>
        /// gets or sets voucherNo
        /// </summary>
        public string voucherNo { get; set; }

        /// <summary>
        /// gets or sets amount
        /// </summary>
        public decimal amount { get; set; }

        public string remark { get; set; }
    }
}
