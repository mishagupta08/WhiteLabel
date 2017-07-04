namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// Hold Login Detail
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets username
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// gets or sets password
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// gets or sets action
        /// </summary>
        public string action { get; set; }

        ///// <summary>
        ///// gets or sets company id
        ///// </summary>
        //public string company_id { get; set; }

        /// <summary>
        /// gets or sets domain name
        /// </summary>
        public string domain_name { get; set; }
    }
}