using ShineYatraAdmin.Entity;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShineYatraAdmin.Entity
{
    [XmlRoot(ElementName = "Request")]
    public class Request
    {
        [XmlElement(ElementName = "FlightType")]
        public string FlightType { get; set; }
        [XmlElement(ElementName = "Origin")]
        public string Origin { get; set; }
        [XmlElement(ElementName = "Destination")]
        public string Destination { get; set; }
        [XmlElement(ElementName = "DepartDate")]
        public string DepartDate { get; set; }
        [XmlElement(ElementName = "ReturnDate")]
        public string ReturnDate { get; set; }
        [XmlElement(ElementName = "AdultPax")]
        public int AdultPax { get; set; }
        [XmlElement(ElementName = "ChildPax")]
        public int ChildPax { get; set; }
        [XmlElement(ElementName = "InfantPax")]
        public int InfantPax { get; set; }
        [XmlElement(ElementName = "Preferredclass")]
        public string Preferredclass { get; set; }
        [XmlElement(ElementName = "mode")]
        public string Mode { get; set; }
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "FlightNumber")]
        public string FlightNumber { get; set; }                    
        public string OperatingAirlineCode { get; set; }
        [XmlElement(ElementName = "personName")]
        public personName PersonName { get; set; }
        [XmlElement(ElementName = "phoneNumber")]
        public string PhoneNumber { get; set; }
        [XmlElement(ElementName = "creditcardno")]
        public string Creditcardno { get; set; }
        [XmlElement(ElementName = "emailAddress")]
        public string EmailAddress { get; set; }
        public string PaymentMode { get; set; }

        /// <summary>
        /// Gets or sets trip mode
        /// </summary>
        public List<KeyValuePair> FlightClass { get; set; }

        /// <summary>
        /// Gets or sets trip mode
        /// </summary>
        public List<KeyValuePair> TripMode { get; set; }
        public float AdultFare { get; set; }
        /// <summary>
        /// Method to assign trip mode
        /// </summary>
        public void AssignTripMode()
        {
            this.TripMode = new List<KeyValuePair>();

            this.TripMode.Add(new KeyValuePair
            {
                Id = "ONE",
                Value = "OneWay"
            });

            this.TripMode.Add(new KeyValuePair
            {
                Id = "ROUND",
                Value = "Return"
            });
        }

        /// <summary>
        /// Method to assign Flight Class
        /// </summary>
        public void AssignFlightClass()
        {
            this.FlightClass = new List<KeyValuePair>();

            this.FlightClass.Add(new KeyValuePair
            {
                Id = "E",
                Value = "Economy"
            });            
        }
    }

    [XmlRoot(ElementName = "CustomerInfo")]
    public class CustomerInfo
    {
        [XmlElement(ElementName = "givenName")]
        public string givenName { get; set; }

        [XmlElement(ElementName = "surName")]
        public string surName { get; set; }

        [XmlElement(ElementName = "nameReference")]
        public string nameReference { get; set; }

        [XmlElement(ElementName = "psgrtype")]
        public string psgrtype { get; set; }

        [XmlElement(ElementName = "dob")]
        public string dob { get; set; }

    }

    [XmlRoot(ElementName = "personName")]
    public class personName
    {
        [XmlElement(ElementName = "CustomerInfo")]
        public List<CustomerInfo> CustomerInfo { get; set; }
    }

    [XmlRoot(ElementName = "BookingClassFare")]
    public class BookingClassFare
    {
        [XmlElement(ElementName = "adultFare")]
        public string AdultFare { get; set; }
        [XmlElement(ElementName = "bookingclass")]
        public string Bookingclass { get; set; }
        [XmlElement(ElementName = "classType")]
        public string ClassType { get; set; }
        [XmlElement(ElementName = "farebasiscode")]
        public string Farebasiscode { get; set; }
        [XmlElement(ElementName = "Rule")]
        public string Rule { get; set; }
        [XmlElement(ElementName = "adultCommission")]
        public string AdultCommission { get; set; }
        [XmlElement(ElementName = "childCommission")]
        public string ChildCommission { get; set; }
        [XmlElement(ElementName = "commissionOnTCharge")]
        public string CommissionOnTCharge { get; set; }
    }

    [XmlRoot(ElementName = "FlightsDetail")]
    public class FlightsDetail
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "ArrivalAirportCode")]
        public string ArrivalAirportCode { get; set; }
        [XmlElement(ElementName = "ArrivalDateTime")]
        public string ArrivalDateTime { get; set; }
        [XmlElement(ElementName = "DepartureAirportCode")]
        public string DepartureAirportCode { get; set; }
        [XmlElement(ElementName = "DepartureDateTime")]
        public string DepartureDateTime { get; set; }
        [XmlElement(ElementName = "FlightNumber")]
        public string FlightNumber { get; set; }
        [XmlElement(ElementName = "OperatingAirlineCode")]
        public string OperatingAirlineCode { get; set; }
        [XmlElement(ElementName = "StopQuantity")]
        public string StopQuantity { get; set; }
        [XmlElement(ElementName = "ImageFileName")]
        public string ImageFileName { get; set; }
        [XmlElement(ElementName = "Availability")]
        public string Availability { get; set; }
        [XmlElement(ElementName = "AirLineName")]
        public string AirLineName { get; set; }
        [XmlElement(ElementName = "IsReturnFlight")]
        public string IsReturnFlight { get; set; }
        [XmlElement(ElementName = "BookingClassFare")]
        public BookingClassFare BookingClassFare { get; set; }
    }

    [XmlRoot(ElementName = "ArrayOfFlightsDetail")]
    public class ArrayOfFlightsDetail
    {
        [XmlElement(ElementName = "FlightsDetail")]
        public List<FlightsDetail> FlightsDetail { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }

    public class SearchPageViewModel
    {
        public float walletBalance { get; set; }
        public ArrayOfFlightsDetail arrayOfSearchedFlights { get; set; }
        public Request flightSearch { get; set; }
        public Request FlightBookingDetail { get; set; }
        public FlightsDetail flightsDetail { get; set; }
        public List<KeyValuePair> NameReferenceList { get; set; }
        public List<KeyValuePair> ChildNameReferenceList { get; set; }
        /// <summary>
        /// Method to assign Name References
        /// </summary>
        public void AssignNameReference()
        {
            this.NameReferenceList = new List<KeyValuePair>();

            this.NameReferenceList.Add(new KeyValuePair
            {
                Id = "Mr.",
                Value = "Mr."
            });

            this.NameReferenceList.Add(new KeyValuePair
            {
                Id = "Ms.",
                Value = "Ms."
            });

            this.NameReferenceList.Add(new KeyValuePair
            {
                Id = "Mrs.",
                Value = "Mrs."
            });
        }

        /// <summary>
        /// Method to assign Name References
        /// </summary>
        public void AssignChildNameReference()
        {
            this.ChildNameReferenceList = new List<KeyValuePair>();
            
            this.ChildNameReferenceList.Add(new KeyValuePair
            {
                Id = "Mstr.",
                Value = "Mstr."
            });
            this.ChildNameReferenceList.Add(new KeyValuePair
            {
                Id = "Miss.",
                Value = "Miss."
            });
        }

    }
    [XmlRoot(ElementName = "Bookingresponse")]
    public class Bookingresponse
    {
        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "transid")]
        public string Transid { get; set; }
        [XmlElement(ElementName = "partnerRefId")]
        public string PartnerRefId { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }

        public string txn_id { get; set; }
    }

    [XmlRoot(ElementName = "EticketRequest")]
    public class EticketRequest
    {
        [XmlElement(ElementName = "transid")]
        public string Transid { get; set; }
        [XmlElement(ElementName = "partnerRefId")]
        public string PartnerRefId { get; set; }
    }

    [XmlRoot(ElementName = "CancelationDetails")]
    public class CancelationDetails
    {
        public string error { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }

    [XmlRoot(ElementName = "Eticket")]
    public class Eticket
    {
        [XmlElement(ElementName = "givenName")]
        public string GivenName { get; set; }
        [XmlElement(ElementName = "surName")]
        public string SurName { get; set; }
        [XmlElement(ElementName = "nameReference")]
        public string NameReference { get; set; }
        [XmlElement(ElementName = "eticketno")]
        public string Eticketno { get; set; }
        [XmlElement(ElementName = "flightuid")]
        public string Flightuid { get; set; }
        [XmlElement(ElementName = "passuid")]
        public string Passuid { get; set; }
    }

    [XmlRoot(ElementName = "eticketdto")]
    public class Eticketdto
    {
        [XmlElement(ElementName = "Eticket")]
        public Eticket Eticket { get; set; }
    }

    [XmlRoot(ElementName = "OriDestPNRRequest")]
    public class OriDestPNRRequest
    {
        [XmlElement(ElementName = "flightno")]
        public string Flightno { get; set; }
        [XmlElement(ElementName = "eticketdto")]
        public Eticketdto Eticketdto { get; set; }
        [XmlElement(ElementName = "confirmationid")]
        public string Confirmationid { get; set; }
        [XmlElement(ElementName = "pnrnumber")]
        public string Pnrnumber { get; set; }
    }

    [XmlRoot(ElementName = "origindestinationoptions")]
    public class Origindestinationoptions
    {
        [XmlElement(ElementName = "OriDestPNRRequest")]
        public OriDestPNRRequest OriDestPNRRequest { get; set; }
    }

    [XmlRoot(ElementName = "requestedPNR")]
    public class RequestedPNR
    {
        [XmlElement(ElementName = "origindestinationoptions")]
        public Origindestinationoptions Origindestinationoptions { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "partnerRefId")]
        public string PartnerRefId { get; set; }
    }

    [XmlRoot(ElementName = "EticketDetails")]
    public class EticketDetails
    {
        [XmlElement(ElementName = "requestedPNR")]
        public RequestedPNR RequestedPNR { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }

    public class Passengers
    {
        public string passenger_category { get; set; }
        public string dob {get; set; }
        public string title { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string extra_field_1 { get; set; }
        public string extra_field_2 { get; set; }
        public string extra_field_3 { get; set; }
        public string extra_field_4 { get; set; }
        public string extra_field_5 { get; set; }
    }

    public class BookingDetail
    {
        public string action { get; set; }
        public string request_token { get; set; }
        public int service_id { get; set; }
        public string sub_service_id { get; set; }
        public string trip_category { get; set; }
        public string category { get; set; }
        public string txn_type { get; set; }
        public string member_id { get; set; }
        public string travel_from { get; set; }
        public string travel_to { get; set; }
        public string travel_start_Date { get; set; }
        public string travel_return_date { get; set; }
        public string company_id { get; set; }
        public string trip_mode { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public int adult { get; set; }
        public int child { get; set; }
        public int infant { get; set; }
        public float amount { get; set; }
        public string status { get; set; }
        public string deposit_mode { get; set; }
        public string remarks { get; set; }
        public string airline_code { get; set; }
        public string trip_class { get; set; }
        public string flight_id { get; set; }
        public string flight_no { get; set; }
        public string ref_code { get; set; }
        public string api_txn_id { get; set; }
        public List<Passengers> passenger_details { get; set; }
    }

    public class INSERT_SERVICE_BOOKING_REQUEST
    {
        public int txn_id { get; set; }
        public string MSG { get; set; }
}
}

