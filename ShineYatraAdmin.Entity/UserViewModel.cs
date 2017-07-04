using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class UserViewModel
    {
        /// <summary>
        /// get and set service list
        /// </summary>
        public IList<Service> services { get; set; }

        /// <summary>
        /// get and set userDetail
        /// </summary>
        public UserDetail userDetail { get; set; }

        /// <summary>
        /// get and set userDetail
        /// </summary>
        public IList<UserDetail> userDetailList { get; set; }

        /// <summary>
        /// gets or sets service commission stucture
        /// </summary>
        public IList<CompanyCommissionGroup> commissionstucture { get; set; }

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
        /// gets or sets Search List Parameter
        /// </summary>
        public SearchParameter SearchListParameter { get; set; }

        /// <summary>
        /// gets or sets search list
        /// </summary>
        public IList<KeyValuePair> SearchStatusList { get; set; }

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

        /// <summary>
        /// get and set flight, bus and hotel group id for the member
        /// </summary>
        public Member_Allotted_group membergroups { get; set; }
    }
}
