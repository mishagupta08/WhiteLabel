namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// Holds complete user detail
    /// </summary>
    public class UserDetail
    {

        /// <summary>
        /// gets or sets member_id
        /// </summary>
        public string member_id { get; set; }

        /// <summary>
        /// gets or sets role_id
        /// </summary>
        public string role_id { get; set; }

        /// <summary>
        /// gets or sets company_id
        /// </summary>
        public string company_id { get; set; }

        /// <summary>
        /// gets or sets company_name
        /// </summary>
        public string company_name { get; set; }

        /// <summary>
        /// gets or sets user_name
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// gets or sets smart_card_no
        /// </summary>
        public string smart_card_no { get; set; }

        /// <summary>
        /// gets or sets pwd
        /// </summary>
        public string pwd { get; set; }

        /// <summary>
        /// gets or sets first_name
        /// </summary>
        public string first_name { get; set; }

        /// <summary>
        /// gets or sets last_name
        /// </summary>
        public string last_name { get; set; }

        /// <summary>
        /// gets or sets active_status
        /// </summary>
        public string active_status { get; set; }

        /// <summary>
        /// gets or sets ref_id
        /// </summary>
        public string ref_id { get; set; }

        /// <summary>
        /// gets or sets current commission flight_group_id
        /// </summary>
        public int flight_group_id { get; set; }

        /// <summary>
        /// gets or sets current commission hotel_group_id
        /// </summary>
        public int hotel_group_id { get; set; }

        /// <summary>
        /// gets or sets current commission bus_group_id
        /// </summary>
        public int bus_group_id { get; set; }

        /// <summary>
        /// gets or sets wallet_balance
        /// </summary>
        public float wallet_balance { get; set; }

        /// <summary>
        /// gets or sets credit
        /// </summary>
        public float credit { get; set; }

        /// <summary>
        /// gets or sets debit
        /// </summary>
        public float debit { get; set; }

        /// <summary>
        /// gets or sets credit_limit
        /// </summary>
        public float credit_limit { get; set; }
    }

    public class Member_Allotted_group
    {
        /// <summary>
        /// get and set member_id
        /// </summary>
        public int member_id { get; set; }
        /// <summary>
        /// get and set allotted flight group
        /// </summary>
        public string flight_group_id { get; set; }
        /// <summary>
        /// get and set allotted hotel group
        /// </summary>
        public string hotel_group_id { get; set; }
        /// <summary>
        /// get and set allotted bus group
        /// </summary>
        public string bus_group_id { get; set; }
    }
}
