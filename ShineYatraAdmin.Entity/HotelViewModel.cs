using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Entity
{
    public class HotelViewModel
    {
        /// <summary>
        /// gets or sets wallet balance
        /// </summary>
        public WalletResponse WalletResponseDetail { get; set; }

        /// <summary>
        /// gets or sets filter detail
        /// </summary>
        public Filter FilterDetail { get; set; }
        /// <summary>
        /// gets or sets selected menu
        /// </summary>
        public string SelectedMenu { get; set; }

        public Rate SelectedRate { get; set; }

        public string RoomTotalPerNight { get; set; }

        public string hotelRequestCookieId { get; set; }

        public HotelFilter HotelFilterDetail { get; set; }

        public HotelResultDashboard HotelResultDashboardDetail { get; set; }

        /// <summary>
        /// gets or sets hotel request
        /// </summary>
        public HotelRequest HotelRequestDetail { get; set; }

        /// <summary>
        /// gets or sets requet
        /// </summary>
        public HotelDescriptionRequest HotelDescRequest { get; set; }

        /// <summary>
        /// gets or sets hotel request
        /// </summary>
        public ProvisionalBooking ProvisionalBookingDetail { get; set; }

        /// <summary>
        /// gets or sets booking list
        /// </summary>
        public  List<HotelBookingContainer> BookingList { get; set; }

        /// <summary>
        /// gets or sets hotel booking response
        /// </summary>
        public ArzHotelBookingResp HotelBookingResponse { get; set; }

        public string txnId { get; set; }

        public string Info { get; set; }

        /// <summary>
        /// gets or sets flight city list
        /// </summary>
        public IList<KeyValuePair> RoomsList { get; set; }

        /// <summary>
        /// gets or sets child age list
        /// </summary>
        public IList<KeyValuePair> ChildAgeList { get; set; }

        /// <summary>
        /// gets or sets adult count list
        /// </summary>
        public IList<KeyValuePair> AdultCountList { get; set; }

        /// <summary>
        /// gets or sets child count list
        /// </summary>
        public IList<KeyValuePair> ChildCountList { get; set; }

        /// <summary>
        /// gets or sets Title
        /// </summary>
        public IList<KeyValuePair> Title { get; set; }

        /// <summary>
        /// gets or sets hotel list
        /// </summary>
        public IList<Hotel> HotelList { get; set; }

        /// <summary>
        /// gets or sets hotel error.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// gets or sets selected hotel detail
        /// </summary>
        public Entity.HotelDetail.Hotel SelectedHotel { get; set; }

        /// <summary>
        /// method to assign room
        /// </summary>
        public void AssignRooms()
        {
            this.RoomsList = new List<KeyValuePair>();
            this.RoomsList.Add(new KeyValuePair
            {
                Id = "1",
                Value = "1 Room"
            });

            this.RoomsList.Add(new KeyValuePair
            {
                Id = "2",
                Value = "2 Room"
            });

            this.RoomsList.Add(new KeyValuePair
            {
                Id = "3",
                Value = "3 Room"
            });

            this.RoomsList.Add(new KeyValuePair
            {
                Id = "4",
                Value = "4 Room"
            });

            this.AdultCountList = new List<KeyValuePair>();
            this.AdultCountList.Add(new KeyValuePair
            {
                Id = "1",
                Value = "1 Adults"
            });

            this.AdultCountList.Add(new KeyValuePair
            {
                Id = "2",
                Value = "2 Adults"
            });

            this.AdultCountList.Add(new KeyValuePair
            {
                Id = "3",
                Value = "3 Adults"
            });

            this.AdultCountList.Add(new KeyValuePair
            {
                Id = "4",
                Value = "4 Adults"
            });

            this.ChildCountList = new List<KeyValuePair>();
            this.ChildCountList.Add(new KeyValuePair
            {
                Id = "0",
                Value = "Children"
            });

            this.ChildCountList.Add(new KeyValuePair
            {
                Id = "1",
                Value = "1 Child"
            });

            this.ChildCountList.Add(new KeyValuePair
            {
                Id = "2",
                Value = "2 Children"
            });

            this.ChildAgeList = new List<KeyValuePair>();

            for (var i = 1; i < 12; i++)
            {
                this.ChildAgeList.Add(new KeyValuePair
                {
                    Id = i.ToString(),
                    Value = i.ToString()
                });
            }
        }

        /// <summary>
        /// method to assign room
        /// </summary>
        public void AssignTitle()
        {
            this.Title = new List<KeyValuePair>();
            this.Title.Add(new KeyValuePair
            {
                Id = "Mr.",
                Value = "Mr."
            });

            this.Title.Add(new KeyValuePair
            {
                Id = "Mrs.",
                Value = "Mrs."
            });

            this.Title.Add(new KeyValuePair
            {
                Id = "Miss.",
                Value = "Miss."
            });
        }
    }
}
