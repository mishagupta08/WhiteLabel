using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class HotelBookingContainer
    {
        public int txn_id { get; set; }
        public int unique_ref_no { get; set; }
        public int service_id { get; set; }
        public string category { get; set; }
        public string txn_type { get; set; }
        public int member_id { get; set; }
        public int amount { get; set; }
        public string status { get; set; }
        public string deposit_mode { get; set; }
        public object remarks { get; set; }
        public string api_txn_id { get; set; }
        public string txn_date { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string passenger_category { get; set; }
        public string title { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address_line1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string check_in_time { get; set; }
        public string check_out_time { get; set; }
        public string hotel_name { get; set; }
        public string hotel_city { get; set; }
        public string hotel_address { get; set; }
        public string room_type { get; set; }
        public string hotel_amenities { get; set; }
        public string check_in_date { get; set; }
        public string check_out_date { get; set; }

        public string adult { get; set; }
        public string child { get; set; }
        public string room_count { get; set; }
        public string web_service { get; set; }
        public string booking_ref_no { get; set; }
    }
}
