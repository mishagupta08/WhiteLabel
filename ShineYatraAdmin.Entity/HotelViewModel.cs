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
        /// gets or sets selected menu
        /// </summary>
        public string SelectedMenu { get; set; }

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
        /// gets or sets flight city list
        /// </summary>
        public IList<KeyValuePair> RoomsList { get; set; }

        /// <summary>
        /// gets or sets Title
        /// </summary>
        public IList<KeyValuePair> Title { get; set; }

        /// <summary>
        /// gets or sets hotel list
        /// </summary>
        public List<Hotel> HotelList { get; set; }

        /// <summary>
        /// gets or sets selected hotel detail
        /// </summary>
        public ShineYatraAdmin.Entity.HotelDetail.Hotel SelectedHotel { get; set; }

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

            this.RoomsList.Add(new KeyValuePair
            {
                Id = "5",
                Value = "5 Room"
            });
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
