using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class BillServiceViewModel
    {
        public string service { get; set; }
        public IList<CompanyCommissionGroup> serviceProviders { get; set; }
        public List<ZoneList> ZoneList { get; set; }
    }

    public class BillServiceFieldViewModel
    {      
        public IList<BillServicesFields> fieldList { get; set; }
        public List<ZoneList> ZoneList { get; set; }
    }

    public class BillServicesFields
    {
        public string member_id { get; set; }
        public int? field_id { get; set; }
        public string field_name { get; set; }
        public string service_id { get; set; }
        public int? sub_service_id { get; set; }
        public string visible { get; set; }
        public string required { get; set; }
    }
    
    public class ZoneList
    {
        public string zone_id { get; set; }      
        public string zone_name { get; set; }        
        public int sub_service_id { get; set; }
        public string active_status { get; set; }
    }
    
    public class SubZoneList
    {
        public int zone_id { get; set; }
        public string sub_zone_name { get; set; }
        public int sub_zone_id { get; set; }
        public string active_status { get; set; }
    }
}
