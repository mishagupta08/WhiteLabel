using System.Collections.Generic;

namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// holds company detail
    /// </summary>
    public class Company
    {
        /// <summary>
        /// gets or sets member_id
        /// </summary>
        public string member_id { get; set; }

        /// <summary>
        /// gets or sets company_name
        /// </summary>
        public string company_name { get; set; }

        /// <summary>
        /// gets or sets company_website
        /// </summary>
        public string company_website { get; set; }

        /// <summary>
        /// gets or sets utility_domain
        /// </summary>
        public string utility_domain { get; set; }

        /// <summary>
        /// gets or sets company_add1
        /// </summary>
        public string company_add1 { get; set; }

        /// <summary>
        /// gets or sets company_add2
        /// </summary>
        public string company_add2 { get; set; }

        /// <summary>
        /// gets or sets company_add3
        /// </summary>
        public string company_add3 { get; set; }

        /// <summary>
        /// gets or sets company_city
        /// </summary>
        public string company_city { get; set; }

        /// <summary>
        /// gets or sets company_state
        /// </summary>
        public string company_state { get; set; }

        /// <summary>
        /// gets or sets company_zip
        /// </summary>
        public string company_zip { get; set; }

        /// <summary>
        /// gets or sets company_country
        /// </summary>
        public string company_country { get; set; }

        /// <summary>
        /// gets or sets company_phone
        /// </summary>
        public string company_phone { get; set; }

        /// <summary>
        /// gets or sets company_mobile
        /// </summary>
        public string company_mobile { get; set; }

        /// <summary>
        /// gets or sets company_fax
        /// </summary>
        public string company_fax { get; set; }

        /// <summary>
        /// gets or sets contact_person_name
        /// </summary>
        public string contact_person_name { get; set; }

        /// <summary>
        /// gets or sets contact_person_phone
        /// </summary>
        public string contact_person_phone { get; set; }

        /// <summary>
        /// gets or sets add_user_id
        /// </summary>
        public string add_user_id { get; set; }

        /// <summary>
        /// gets or sets active_status
        /// </summary>
        public string active_status { get; set; }

        /// <summary>
        /// gets or sets company_id
        /// </summary>
        public int company_id { get; set; }

        /// <summary>
        /// gets or sets company_logo
        /// </summary>
        public string company_logo { get; set; }

        /// <summary>
        /// gets or sets wallet_credit_limit.
        /// </summary>
        public string wallet_credit_limit { get; set; }

        /// <summary>
        /// gets or sets wallet_balance
        /// </summary>
        public string wallet_balance { get; set; }

        /// <summary>
        /// gets or sets action
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// gets or sets update user id
        /// </summary>
        public string update_user_id { get; set; }

        /// <summary>
        /// gets or sets cmp_setting_id
        /// </summary>
        public string cmp_setting_id { get; set; }        

        /// <summary>
        /// gets or sets Company service commission stucture
        /// </summary>
        public IList<CompanyCommissionGroup> commissionstucture { get; set; }

        /// <summary>
        /// gets or sets Company extra setttings
        /// </summary>
        public CompanySetting commissionSetting { get; set; }
    }
}
