using System;

namespace ShineYatraAdmin.Entity
{
    #region namespace

    using System.Xml.Serialization;

    #endregion namespace

    [XmlRoot(ElementName = "RechargeRequest")]
    public class RechargeRequest
    {
        [XmlElement(ElementName = "ProductId")]
        public string ProductId { get; set; }
        [XmlElement(ElementName = "MobileNumber")]
        public string MobileNumber { get; set; }
        [XmlElement(ElementName = "RequestId")]
        public string RequestId { get; set; }
        [XmlElement(ElementName = "Amount")]
        public string Amount { get; set; }
    }

    [XmlRoot(ElementName = "ServicesRequest")]
    public class ServicesRequest
    {
        [XmlElement(ElementName = "type")]
        public string type { get; set; }

        [XmlElement(ElementName = "format")]
        public string format { get; set; }

        [XmlElement(ElementName = "mode")]
        public string mode { get; set; }

        [XmlElement(ElementName = "spkey")]
        public string spkey { get; set; }

        [XmlElement(ElementName = "agentid")]
        public string agentid { get; set; }

        [XmlElement(ElementName = "account")]
        public string account { get; set; }

        [XmlElement(ElementName = "amount")]
        public float amount { get; set; }

        public double discount { get; set; }
        public double pg_amount { get; set; }

    }

    public class RechargeViewModel {   
        public float walletBalance { get; set; }     
        public string rechargeType { get; set; }
        public string status { get; set; }
    }

    public class ValidateTransaction
    {
        public string ipay_errorcode { get; set; }
        public string ipay_errordesc { get; set; }
        public string particulars { get; set; }

    }

    public class WalletBalance
    {
        public float Wallet { get; set; }
    }

    public class TransactionStatus
    {
        public string ipay_id { get; set; }
        public string agent_id { get; set; }
        public string opr_id { get; set; }
        public string account_no { get; set; }
        public string sp_key { get; set; }
        public float trans_amt { get; set; }
        public float charged_amt { get; set; }
        public float opening_bal { get; set; }
        public string req_dt { get; set; }
        public string status { get; set; }
        public string res_code { get; set; }
        public string res_msg { get; set; }
        public string ipay_errorcode { get; set; }
        public string ipay_errordesc { get; set; }
        public DateTime datetime { get; set; }

    }

    /// <summary>
    /// gets or sets ServiceDetail
    /// </summary>
    public class ServiceDetail
    {
        /// <summary>
        /// gets or sets service_type
        /// </summary>
        public string service_type { get; set; }

        /// <summary>
        /// gest or sets service_provider
        /// </summary>
        public string service_provider { get; set; }

        /// <summary>
        /// gets or sets service_desc
        /// </summary>
        public string service_desc { get; set; }

        /// <summary>
        /// gets or sets provider_key
        /// </summary>
        public string provider_key { get; set; }

        /// <summary>
        /// gets or sets margin
        /// </summary>
        public string margin { get; set; }
    }
}