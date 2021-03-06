﻿namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// Holds complete user detail
    /// </summary>
    public class UserDetail
    {        
        public string action { get; set; }
        public string password { get; set; }   
        public string mobile_number { get; set; }
        public string email { get; set; }
        public int kit_id { get; set; }                   
        /// <summary>
        /// gets or sets member_id
        /// </summary>
        public string member_id { get; set; }

        /// <summary>
        /// gets or sets mobileNo
        /// </summary>
        public string mobileNo { get; set; }

        /// <summary>
        /// gets or sets ledger_id
        /// </summary>
        public string ledger_id { get; set; }

        /// <summary>
        /// gets or sets user_type
        /// </summary>
        public string user_type { get; set; }
        

        /// <summary>
        /// gets or sets doj
        /// </summary>
        public string doj { get; set; }

        /// <summary>
        /// gets or sets kitid
        /// </summary>
        public string kitid { get; set; }

        /// <summary>
        /// gets or sets role_id
        /// </summary>
        public string emailId { get; set; }

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
        public double wallet_balance { get; set; }

        /// <summary>
        /// gets or sets company_wallet_balance
        /// </summary>
        public double company_wallet_balance { get; set; }
       
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
        /// get and set service id
        /// </summary>
        public int service_id { get; set; }
        /// <summary>
        /// get and set allotted group id
        /// </summary>
        public int comp_group_id { get; set; }

        /// <summary>
        /// get and set allotted grup name
        /// </summary>
        public string comp_group_name { get; set; }

        /// <summary>
        /// get and set category
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// get and set sub category
        /// </summary>
        public string sub_category { get; set; }

    }
}
