using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShineYatraAdmin.Entity;

namespace ShineYatraAdmin.Entity
{
    public class PrimarySetting
    {
        public IList<CompanyCommissionGroup> structure { get; set; }
        public DefaultRechargeSetting defaultRechargeSetting { get; set; }
        public IList<SubCategory> bill_services { get; set; }
        public string memberid { get; set; }
    }

    public class DefaultRechargeSetting
    {
        public string action { get; set; }
        public int default_setting_id { get; set; }
        public float prepaid_margin { get; set; }
        public float postpaid_margin { get; set; }
        public float dth_margin { get; set; }
    }

    public class PrimaryRechargeMargin
    {
        public string action { get; set; }
        public int member_id { get; set; }
        public float margin_per { get; set; }
        public int recharge_service_id { get; set; }
        public int service_id { get; set; }        
    }

    public class PrimarySettingMargin
    {
            
        public string action { get; set; }
        public int sub_service_id { get; set; }        
        public int service_id { get; set; }
        public float front_discount_per { get; set; }
        public float front_discount_amount { get; set; }
        public float back_discount_per { get; set; }
        public float back_discount_amount { get; set; }
        public float gap_discount_per { get; set; }
        public float gap_discount_amount { get; set; }
        public string member_id { get; set; }
    }

    public class SubCategory
    {
        public string sub_category { get; set; }
        public string category { get; set; }
    }

}
