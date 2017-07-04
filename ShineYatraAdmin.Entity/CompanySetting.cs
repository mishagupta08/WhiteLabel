namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// holds company setting detail
    /// </summary>
    public class CompanySetting
    {
        /// <summary>
        /// gets or sets company_id
        /// </summary>
        public string company_id { get; set; }

        /// <summary>
        /// gets or sets cmp_setting_id
        /// </summary>
        public string cmp_setting_id { get; set; }

        /// <summary>
        /// gets or sets web_login_api_enabled
        /// </summary>
        public string web_login_api_enabled { get; set; }

        /// <summary>
        /// gets or sets web_ewallet_api_enabled
        /// </summary>
        public string web_ewallet_api_enabled { get; set; }

        /// <summary>
        /// gets or sets web_pg_api_enabled
        /// </summary>
        public string web_pg_api_enabled { get; set; }

        /// <summary>
        /// gets or sets app_login_api_enabled
        /// </summary>
        public string app_login_api_enabled { get; set; }

        /// <summary>
        /// gets or sets app_ewallet_api_enabled
        /// </summary>
        public string app_ewallet_api_enabled { get; set; }

        /// <summary>
        /// gets or sets app_pg_api_enabled
        /// </summary>
        public string app_pg_api_enabled { get; set; }

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
        /// gets or sets otp_login_enabled
        /// </summary>
        public string otp_login_enabled { get; set; }

        /// <summary>
        /// gets or sets otp_login_service
        /// </summary>
        public string otp_login_service { get; set; }

        /// <summary>
        /// gets or sets email_id
        /// </summary>
        public string email_id { get; set; }

        /// <summary>
        /// gets or sets email_password
        /// </summary>
        public string email_password { get; set; }

        /// <summary>
        /// gets or sets master_postpaid_margin
        /// </summary>
        public int master_postpaid_margin { get; set; }

        /// <summary>
        /// gets or sets email_password
        /// </summary>
        public int master_prepaid_margin { get; set; }

        /// <summary>
        /// gets or sets email_password
        /// </summary>
        public int master_dth_margin { get; set; }
    }
}
