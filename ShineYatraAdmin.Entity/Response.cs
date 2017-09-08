﻿namespace ShineYatraAdmin.Entity
{
    #region namespace

    using System.Collections.Generic;

    #endregion namespace

    /// <summary>
    /// hold api response detail
    /// </summary>
    public class Response
    {
        /// <summary>
        /// gets or sets status
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// gets or sets response value
        /// </summary>
        public string ResponseValue { get; set; }

        /// <summary>
        /// gets or sets APISTATUS
        /// </summary>
        public string APISTATUS { get; set; }

        public string MSG { get; set; }

        /// <summary>
        /// gets or sets VALIDATELOGIN
        /// </summary>
        public IList<UserDetail> VALIDATELOGIN { get; set; }

        /// <summary>
        /// gets or sets get company list
        /// </summary>
        public IList<Company> GETCOMPANY { get; set; }

        /// <summary>
        /// gets or sets get insert company list
        /// </summary>
        public IList<Company> INSERTCOMPANY { get; set; }

        /// <summary>
        /// gets or sets get insert company list
        /// </summary>
        public IList<Company> UPDATECOMPANY { get; set; }

        /// <summary>
        /// gets or sets api setting
        /// </summary>
        public IList<CompanyApiSetting> UPDATE_COMPANY_EXTRASETTINGS_API { get; set; }

        /// <summary>
        /// gets or sets company setting
        /// </summary>
        public IList<CompanySetting> GET_COMPANY_EXTRA_SETTINGS { get; set; }

        /// <summary>
        /// gets or sets email setting response
        /// </summary>
        public IList<CompanySetting> UPDATE_COMPANY_EXTRASETTINGS_EMAIL { get; set; }

        /// <summary>
        /// gets or sets sms setting
        /// </summary>
        public IList<CompanySetting> UPDATE_COMPANY_EXTRASETTINGS_SMS { get; set; }

        /// <summary>
        /// gets or sets company service list
        /// </summary>
        public IList<Service> GET_SERVICES { get; set; }
        
        /// <summary>
        /// gets or sets company Master price group list
        /// </summary>
        public IList<CompanyCommissionGroup> GET_COMMISSION_GROUPS { get; set; }

        /// <summary>
        /// gets or sets company Master price group list
        /// </summary>
        public IList<CompanyCommissionGroup> GET_COMMISSION_GROUP_STRUCTURE { get; set; }

        /// <summary>
        /// gets or sets primary settings
        /// </summary>
        public IList<CompanyCommissionGroup> GET_SERVICE_DETAILS { get; set; }

        /// <summary>
        /// gets or sets get recharge master commission structure
        /// </summary>
        public IList<CompanyCommissionGroup> GET_PRIMARY_RECHARGE_STRUCTURE { get; set; }

        /// <summary>
        /// gets or sets get recharge master commission structure
        /// </summary>
        public IList<CompanyCommissionGroup> GET_COMPANY_RECHARGE_COMMISSION { get; set; }

        /// <summary>
        /// gets or sets company user list
        /// </summary>
        public IList<UserDetail> GET_USERS_LIST { get; set; }

        /// <summary>
        /// gets or sets recharge defaut settings
        /// </summary>
        public IList<DefaultRechargeSetting> GET_DEFAULT_SETTINGS { get; set; }

        /// <summary>
        /// gets or sets member allotted groups
        /// </summary>
        public IList<Member_Allotted_group> GET_MEMBER_ALLOTED_GROUPS { get; set; }

        /// <summary>
        /// gets or sets flight details according to search parameter
        /// </summary>
        public string FLIGHT_SEARCH { get; set; }

		/// <summary>
        /// gets or sets list of sub categories
        /// </summary>
        public IList<SubCategory> GET_SERVICE_SUB_CATEGORY_LIST { get; set; }

        /// <summary>
        /// gets or sets flight booking details to database
        /// </summary>
        public List<INSERT_SERVICE_BOOKING_REQUEST> INSERT_SERVICE_BOOKING_REQUEST { get; set; }

        /// <summary>
        /// gets or sets Recharge details to database
        /// </summary>
        public List<RechargeDBTxnResponse> INSERT_SERVICE_RECHARGE_REQUEST_INSTANTPAY { get; set; }

        /// <summary>
        /// gets or sets company wallet balance
        /// </summary>
        public List<WalletResponse> GET_WALLET_BALANCE { get; set; }

        /// <summary>
        /// gets or sets white label theme for domain
        /// </summary>
        public List<CompanyTheme> GET_DOMAININFO { get; set; }

        public List<CompanyCommissionGroup> GET_COMMISSION_GROUPS_ALLOTEMENT_CHOICES { get; set; }

        /// <summary>
        /// gets or sets flight booking details
        /// </summary>
        public List<BookingDetail> GET_FLIGHT_TRANSACTIONS { get; set; }
        
    }
}
