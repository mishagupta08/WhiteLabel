namespace ShineYatraAdmin
{
    #region namespace

    using Entity;
    using System.Collections.Generic;

    #endregion namespace

    /// <summary>
    /// hold session for shine yatra
    /// </summary>
    public static class ShineYatraSession
    {               
        /// <summary>
        /// gets or sets flight city list
        /// </summary>
        public static IList<KeyValuePair> HotelCityList { get; set; }

        /// <summary>
        /// gets or sets hotel request
        /// </summary>
        public static HotelRequest HotelRequest { get; set; }

        /// <summary>
        /// gets or sets selected Hotel
        /// </summary>
        public static ShineYatraAdmin.Entity.HotelDetail.Hotel SelectedHotel { get; set; }

    }
}