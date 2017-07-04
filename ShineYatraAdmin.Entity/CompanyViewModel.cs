using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class CompanyViewModel
    {
        /// <summary>
        /// gets or sets selected menu
        /// </summary>
        public string LoginUserName { get; set; }

        /// <summary>
        /// gets or sets selected menu
        /// </summary>
        public string SelectedMenu { get; set; }

        /// <summary>
        /// gets or sets Search List Parameter
        /// </summary>
        public SearchParameter SearchListParameter { get; set; }

        /// <summary>
        /// gets or sets search list
        /// </summary>
        public IList<KeyValuePair> SearchStatusList { get; set; }

        /// <summary>
        /// gets or sets Country List
        /// </summary>
        public IList<KeyValuePair> CountryList { get; set; }

        /// <summary>
        /// gets or sets company list
        /// </summary>
        public IList<Company> CompanyList { get; set; }

        /// <summary>
        /// gets or sets company detail
        /// </summary>
        public Company CompanyDetail { get; set; }

        /// <summary>
        /// Method to assign search list
        /// </summary>
        public void AssignSearchList()
        {
            this.SearchStatusList = new List<KeyValuePair>();

            this.SearchStatusList.Add(new KeyValuePair
            {
                Id = "All",
                Value = "All"
            });

            this.SearchStatusList.Add(new KeyValuePair
            {
                Id = "Y",
                Value = "Y"
            });

            this.SearchStatusList.Add(new KeyValuePair
            {
                Id = "N",
                Value = "N"
            });
        }

        public void AssignCountryList()
        {
            this.CountryList = new List<KeyValuePair>();

            this.CountryList.Add(new KeyValuePair
            {
                Id = "0",
                Value = "--Select--"
            });

            this.CountryList.Add(new KeyValuePair
            {
                Id = "1",
                Value = "India"
            });
        }

        /****Paging Variables*****/

        /// <summary>
        /// gets or sets Current Page Index
        /// </summary>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// gets or sets Current Page Index
        /// </summary>
        public int PageSlot { get; set; }

        /// <summary>
        /// gets or sets column
        /// </summary>
        public int SortingColumn { get; set; }

        /// <summary>
        /// gets or sets order
        /// </summary>
        public int SortingOrder { get; set; }

        /// <summary>
        /// gets or sets page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// gets or sets page count
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// gets or sets from page number
        /// </summary>
        public int FromPage { get; set; }

        /// <summary>
        /// gets or sets to page number
        /// </summary>
        public int ToPage { get; set; }

        /// <summary>
        /// gets or sets paging count
        /// </summary>
        public int PagingCount { get; set; }

        /// <summary>
        /// gets or sets record count
        /// </summary>
        public int RecordCount { get; set; }

        /****Paging Variables*****/

        /// <summary>
        /// gets or sets search list
        /// </summary>
        public IList<KeyValuePair> SettingList { get; set; }

        /// <summary>
        /// gets or sets api setting
        /// </summary>
        public CompanyApiSetting ApiSetting { get; set; }

        /// <summary>
        /// gets or sets email setting
        /// </summary>
        public CompanyEmailSetting EmailSetting { get; set; }

        /// <summary>
        /// gets or sets sms setting
        /// </summary>
        public CompanySmsSetting SmsSetting { get; set; }

        /// <summary>
        /// gets or sets otp setting
        /// </summary>
        public CompanyOtpSetting OtpSetting { get; set; }

        /// <summary>
        /// gets or sets recharge commission setting
        /// </summary>
        public CompanyCommissionSetting CommissionSetting { get; set; }

        /// <summary>
        /// gets or sets company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// method to assign setting 
        /// </summary>
        public void AssignSettingList()
        {
            this.SettingList = new List<KeyValuePair>();
            this.SettingList.Add(new KeyValuePair
            {
                Id = "Y",
                Value = "Yes"
            });

            this.SettingList.Add(new KeyValuePair
            {
                Id = "N",
                Value = "No"
            });
        }
    }
}
