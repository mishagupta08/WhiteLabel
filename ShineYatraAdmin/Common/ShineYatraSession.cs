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
        /// gets or sets login user detail
        /// </summary>
        public static UserDetail LoginUser { get; set; }

        /// <summary>
        /// gets or sets selected menu
        /// </summary>
        public static string SelectedMenu { get; set; }

        /// <summary>
        /// gets or sets sort order
        /// </summary>
        public static string SortOrder { get; set; }

        /// <summary>
        /// gets or sets sort order
        /// </summary>
        public static string SortCoulmn { get; set; }

        /// <summary>
        /// gets or sets page index
        /// </summary>
        public static int PageIndex { get; set; }

        /// <summary>
        /// gets or sets company list
        /// </summary>
        public static IList<Company> TempCompanyList { get; set; }

        /// <summary>
        /// gets or sets company list
        /// </summary>
        public static IList<UserDetail> TempUSerList { get; set; }

        /// <summary>
        /// gets or sets flight city list
        /// </summary>
        public static IList<KeyValuePair> FlightCityList { get; set; }

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