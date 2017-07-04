namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// hold Company Otp Setting
    /// </summary>
    public class CompanyOtpSetting
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
        /// gets or sets otp_login_enabled
        /// </summary>
        public string otp_login_enabled { get; set; }

        /// <summary>
        /// gets or sets otp_login_service
        /// </summary>
        public string otp_login_service { get; set; }
    }
}
