using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    //<summary>
    //class to get the details of a price group or commission structure
    //</summary>
    public class CompanyCommissionGroup
    {

        public int company_id { get; set; }
        public int member_id { get; set; }

        public int row_id { get; set; }

        //Service Details
        public int service_id { get; set; }        
        public string service_name { get; set; }
        
        //Sub Service Details
        public int sub_service_id { get; set; }
        public string sub_service_name { get; set; }
        public string sub_service_code { get; set; }
        public string sub_category { get; set; }

        //group details        
        public string comp_group_id { get; set; }
        public string comp_group_name { get; set; }

        //IS this group default group
        public string default_group { get; set; }

        //discount structure                                                                   
        public float front_discount_per { get; set; }
        public float front_discount_amount { get; set; }
        public float back_discount_per { get; set; }
        public float back_discount_amount { get; set; }
        public float bv_discount_per { get; set; }
        public float bv_discount_amount { get; set; }
        public float distb_back_discount_per { get; set; }
        public float distb_back_discount_amount { get; set; }
        public float gap_discount_per { get; set; }
        public float gap_discount_amount { get; set; }
        public float SELF_COMM_PER { get; set; }
        public float DIST_COMM_PER { get; set; }

        //Is the structure for service is active
        public string active_status { get; set; }
        
    }

    /// <summary>
    /// class to update group row
    /// </summary>
    public class CompanyCommissionGroupRow
    {
        public string action { get; set; }           
        public string company_group_id { get; set; } 
        public string member_id { get; set; }
        public string category { get; set; }
        public string sub_category { get; set; }
        public int row_id { get; set; }
        public int service_id { get; set; }
        public int sub_service_id { get; set; }
        public float front_discount_per { get; set; }
        public float front_discount_amount { get; set; }
        public float back_discount_per { get; set; }
        public float back_discount_amount { get; set; }        
        public float gap_discount_per { get; set; }
        public float gap_discount_amount { get; set; }
        public float distribution_per { get; set; }
    }  
    
}
