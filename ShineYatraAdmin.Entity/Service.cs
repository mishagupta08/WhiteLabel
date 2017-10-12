using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    ///<summary>
    ///class to get the details of a price group or commission structure
    ///</summary>
    public class Service
    {
        public int service_id { get; set; }
        public string service_name { get; set; }
        public string service_group { get; set; }

        /// <summary>
        /// get and set details for service commission group
        /// </summary>
        public IList<CompanyCommissionGroup> service_group_list{ get; set; }        
    }

   public class AllotedServiceCGsDetailsRequest
    {
        public string action { get; set; }
        public string category { get; set; }        
        public string sub_category { get; set; }
        public string service_code { get; set; }
        public string sub_service_id { get; set; }
        public string member_id { get; set; }
        public string service_id { get; set; }
    }

}

