using System.Collections.Generic;

namespace ShineYatraAdmin.Entity
{
    public class Filter
    {
        /// <summary>
        /// gets or sets result type
        /// </summary>
        public string ResultType { get; set; }

        /// <summary>
        /// gets or sets select type
        /// </summary>
        public string SelectType { get; set; }

        /// <summary>
        /// gets or sets select type
        /// </summary>
        public string SelectedTypeValue { get; set; }

        /// <summary>
        /// gets or sets From Date
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// gets or sets To Date
        /// </summary>
        public string ToDate { get; set; }

        public IList<KeyValuePair> SelectTypeList { get; set; }

        public void AssignSelectTypeList()
        {
            this.SelectTypeList = new List<KeyValuePair>();
            this.SelectTypeList.Add(new KeyValuePair { Id = "All", Value = "All" });
            this.SelectTypeList.Add(new KeyValuePair { Id = "Id Wise", Value = "Id Wise" });
            this.SelectTypeList.Add(new KeyValuePair { Id = "Member Wise", Value = "Member Wise" });
            this.SelectTypeList.Add(new KeyValuePair { Id = "Mobile No Wise", Value = "Mobile No Wise" });
            this.SelectTypeList.Add(new KeyValuePair { Id = "Email Id Wise", Value = "Email Id Wise" });
        }

    }
}
