using System.Collections.Generic;

namespace ShineYatraAdmin.Entity
{
    /// <summary>
    /// Hold Flight Model Values
    /// </summary>
    public class FlightViewModel
    {
        /// <summary>
        /// gets or sets selected menu
        /// </summary>
        public string SelectedMenu { get; set; }

        /// <summary>
        /// gets or sets Flight search detail
        /// </summary>
        public Request FlightSearchDetail { get; set; }

        /// <summary>
        /// gets or sets flight city list
        /// </summary>
        public IList<KeyValuePair> FlightCityList { get; set; }
    }
}
