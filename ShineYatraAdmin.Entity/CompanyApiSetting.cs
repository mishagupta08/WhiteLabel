namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// hold api setting detail
    /// </summary>
    public class CompanyApiSetting
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
    }
}
