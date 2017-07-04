namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// hold Company Otp Setting
    /// </summary>
    public class CompanyCommissionSetting
    {
        /// <summary>
        /// gets or sets action
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// gets or sets cmp_setting_id
        /// </summary>
        public string cmp_setting_id { get; set; }

        /// <summary>
        /// gets or sets company_id
        /// </summary>
        public string company_id { get; set; }

        /// <summary>
        /// gets or sets perpaid percentage
        /// </summary>
        public int prepaid_margin{ get; set; }

        /// <summary>
        /// gets or sets postpaid percentage
        /// </summary>
        public int postpaid_margin{ get; set; }

        /// <summary>
        /// gets or sets dth percentage
        /// </summary>
        public int dth_margin { get; set; }
    }
}
