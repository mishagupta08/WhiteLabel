using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{     
    /// <summary>
    /// get and set service group and its commission structure
    /// </summary>
    public class GroupModel{      
        public string service_id { get; set; }  
        public string service_name { get; set; }  
        public string sub_service_name { get; set; }
        public IList<CompanyCommissionGroup> service_group_list { get; set; }        
    }

    /// <summary>
    /// model to create new group
    /// </summary>
    public class NewGroupModel
    {
        public string action { get; set; }
        public string service_id { get; set; }
        public string comp_group_name { get; set; }
        public string category { get; set; }
        public string company_id { get; set; }
        public string member_id { get; set; }
        public string sub_category { get; set; }

    }
}
