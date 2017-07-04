namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// hold Company Sms Setting
    /// </summary>
    public class CompanySmsSetting
    {
        /// <summary>
        /// gets or sets action
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// gets or sets sms_api_integrated
        /// </summary>
        public string sms_api_integrated { get; set; }

        /// <summary>
        /// gets or sets sms_api_url
        /// </summary>
        public string sms_api_url { get; set; }

        /// <summary>
        /// gets or sets sms_api_username
        /// </summary>
        public string sms_api_username { get; set; }

        /// <summary>
        /// gets or sets sms_api_password
        /// </summary>
        public string sms_api_password { get; set; }

        /// <summary>
        /// gets or sets sms_api_sender_id
        /// </summary>
        public string sms_api_sender_id { get; set; }

        /// <summary>
        /// gets or sets company_id
        /// </summary>
        public string company_id { get; set; }

        /// <summary>
        /// gets or sets company setting id
        /// </summary>
        public string cmp_setting_id { get; set; }
    }
}
