using ShineYatraAdmin.Entity;
using System;
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
        public string phoneNumber { get; set; }
        [XmlElement(ElementName = "creditcardno")]
        public string Creditcardno { get; set; }
        [XmlElement(ElementName = "emailAddress")]
        public string EmailAddress { get; set; }
        public string PartnerRefId { get; set; }
        public string PaymentMode { get; set; }
        public bool PartialPaymentWithWallet { get; set; }
        public int SubServiceId { get; set; }
        public double backdiscount { get; set; }
        
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

        [XmlElement(ElementName = "age")]
        public string age { get; set; }

        //[XmlElement(ElementName = "extra_field_1")]
        //public string extra_field_1 { get; set; }

        //[XmlElement(ElementName = "extra_field_2")]
        //public string extra_field_2 { get; set; }

        //[XmlElement(ElementName = "extra_field_3")]
        //public string extra_field_3 { get; set; }

        //[XmlElement(ElementName = "extra_field_4")]
        //public string extra_field_4 { get; set; }

        //[XmlElement(ElementName = "extra_field_5")]
        //public string extra_field_5 { get; set; }

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
        public double AdultFare { get; set; }
        [XmlElement(ElementName = "bookingclass")]
        public string Bookingclass { get; set; }
        [XmlElement(ElementName = "classType")]
        public string ClassType { get; set; }
        [XmlElement(ElementName = "farebasiscode")]
        public string Farebasiscode { get; set; }
        [XmlElement(ElementName = "Rule")]
        public string Rule { get; set; }
        [XmlElement(ElementName = "adultCommission")]
        public double AdultCommission { get; set; }
        [XmlElement(ElementName = "childCommission")]
        public double ChildCommission { get; set; }
        [XmlElement(ElementName = "commissionOnTCharge")]
        public double CommissionOnTCharge { get; set; }
    }

    [XmlRoot(ElementName = "FlightsDetail")]
    public class FlightsDetail
    {
        [XmlElement(ElementName = "Id")]
        public string Id { get; set; }
        [XmlElement(ElementName = "ArrivalAirportCode")]
        public string ArrivalAirportCode { get; set; }
        [XmlElement(ElementName = "ArrivalDateTime")]
        public DateTime ArrivalDateTime { get; set; }
        [XmlElement(ElementName = "DepartureAirportCode")]
        public string DepartureAirportCode { get; set; }
        [XmlElement(ElementName = "DepartureDateTime")]
        public DateTime DepartureDateTime { get; set; }
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
        [XmlElement(ElementName = "SubServiceId")]
        public int SubServiceId { get; set; }        
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
        public double walletBalance { get; set; }
        public double discount { get; set;}
        public ArrayOfOrigindestinationoption arrayOfSearchedFlights { get; set; }
        public OriginDestinationOption flightfaredetails { get; set; }
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

    [XmlRoot(ElementName = "ChargeableFares")]
    public class ChargeableFares
    {
        [XmlElement(ElementName = "ActualBaseFare")]
        public double ActualBaseFare { get; set; }

        [XmlElement(ElementName = "Tax")]
        public double Tax { get; set; }

        [XmlElement(ElementName = "STax")]
        public double STax { get; set; }

        [XmlElement(ElementName = "SCharge")]
        public double SCharge { get; set; }

        [XmlElement(ElementName = "TDiscount")]
        public double TDiscount { get; set; }

        [XmlElement(ElementName = "TPartnerCommission")]
        public double TPartnerCommission { get; set; }
    }

    [XmlRoot(ElementName = "NonchargeableFares")]
    public class NonchargeableFares
    {
        [XmlElement(ElementName = "TCharge")]
        public float TCharge { get; set; }

        [XmlElement(ElementName = "TMarkup")]
        public float TMarkup { get; set; }

        [XmlElement(ElementName = "TSdiscount")]
        public float TSdiscount { get; set; }
    }

    [XmlRoot(ElementName = "FareDetail")]
    public class FareDetail
    {
        [XmlElement(ElementName = "ChargeableFares")]
        public ChargeableFares ChargeableFares { get; set; }

        [XmlElement(ElementName = "NonchargeableFares")]
        public NonchargeableFares NonchargeableFares { get; set; }
        
        public double backdiscount { get; set; }
        public double frontdiscount { get; set; }
    }

    [XmlRoot(ElementName = "BookingClass")]
    public class BookingClass
    {
        [XmlElement(ElementName = "Availability")]
        public string Availability { get; set; }

        [XmlElement(ElementName = "ResBookDesigCode")]
        public string ResBookDesigCode { get; set; }
    }

    [XmlRoot(ElementName = "FlightSegment")]
    public class FlightSegment
    {
        [XmlElement(ElementName = "AirEquipType")]
        public string AirEquipType { get; set; }

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

        [XmlElement(ElementName = "OperatingAirlineFlightNumber")]
        public string OperatingAirlineFlightNumber { get; set; }

        [XmlElement(ElementName = "RPH")]
        public string RPH { get; set; }

        [XmlElement(ElementName = "StopQuantity")]
        public string StopQuantity { get; set; }

        [XmlElement(ElementName = "airLineName")]
        public string AirLineName { get; set; }

        [XmlElement(ElementName = "airportTax")]
        public string AirportTax { get; set; }

        [XmlElement(ElementName = "imageFileName")]
        public string ImageFileName { get; set; }

        [XmlElement(ElementName = "viaFlight")]
        public string ViaFlight { get; set; }

        [XmlElement(ElementName = "BookingClass")]
        public BookingClass BookingClass { get; set; }

        [XmlElement(ElementName = "BookingClassFare")]
        public BookingClassFare BookingClassFare { get; set; }

        [XmlElement(ElementName = "Discount")]
        public string Discount { get; set; }

        [XmlElement(ElementName = "airportTaxChild")]
        public string AirportTaxChild { get; set; }

        [XmlElement(ElementName = "airportTaxInfant")]
        public string AirportTaxInfant { get; set; }

        [XmlElement(ElementName = "adultTaxBreakup")]
        public string AdultTaxBreakup { get; set; }

        [XmlElement(ElementName = "childTaxBreakup")]
        public string ChildTaxBreakup { get; set; }

        [XmlElement(ElementName = "infantTaxBreakup")]
        public string InfantTaxBreakup { get; set; }

        [XmlElement(ElementName = "octax")]
        public string Octax { get; set; }
    }

    [XmlRoot(ElementName = "FlightSegments")]
    public class FlightSegments
    {
        [XmlElement(ElementName = "FlightSegment")]
        public List<FlightSegment> FlightSegment { get; set; }
    }

    [XmlRoot(ElementName = "origindestinationoption")]
    public class OriginDestinationOption
    {

            public List<FlightsDetail> FlightsDetailList { get; set; }

            public FareDetail FareDetail { get; set; }
        
    }

    [XmlRoot(ElementName = "OriginDestinationOptions")]
    public class OriginDestinationOptions
    {
        [XmlElement(ElementName = "OriginDestinationOption")]
        public List<OriginDestinationOption> OriginDestinationOption { get; set; }
    }

    [XmlRoot(ElementName = "Response__Depart")]
    public class Response__Depart
    {
        [XmlElement(ElementName = "OriginDestinationOptions")]
        public OriginDestinationOptions OriginDestinationOptions { get; set; }
    }

    [XmlRoot(ElementName = "Response__Return")]
    public class Response__Return
    {
        [XmlElement(ElementName = "OriginDestinationOptions")]
        public OriginDestinationOptions OriginDestinationOptions { get; set; }
    }

    [XmlRoot(ElementName = "arzoo__response")]
    public class Arzoo__response
    {
        [XmlElement(ElementName = "Request")]
        public Request Request { get; set; }

        [XmlElement(ElementName = "Response__Depart")]
        public Response__Depart Response__Depart { get; set; }

        [XmlElement(ElementName = "Response__Return")]
        public Response__Return Response__Return { get; set; }

        [XmlElement(ElementName = "error__tag")]
        public string Error__tag { get; set; }
    }    

    /*****Cancelticket*****/

    [XmlRoot(ElementName = "telePhone")]
    public class TelePhone
    {
        [XmlElement(ElementName = "phoneNumber")]
        public string PhoneNumber { get; set; }
    }

    [XmlRoot(ElementName = "email")]
    public class Email
    {
        [XmlElement(ElementName = "emailAddress")]
        public string EmailAddress { get; set; }
    }

    [XmlRoot(ElementName = "onwardcanceldata")]
    public class Onwardcanceldata
    {
        [XmlElement(ElementName = "Canid")]
        public string Canid { get; set; }

        [XmlElement(ElementName = "remarks")]
        public string Remarks { get; set; }

        [XmlElement(ElementName = "status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "eticketdto")]
        public Eticketdto Eticketdto { get; set; }
    }

    [XmlRoot(ElementName = "cancelationdtls")]
    public class Cancelationdtls
    {
        [XmlElement(ElementName = "onwardcanceldata")]
        public Onwardcanceldata Onwardcanceldata { get; set; }
    }

    /*******Cancellation status******/

    [XmlRoot(ElementName = "Cancellation")]
    public class Cancellation
    {
        [XmlElement(ElementName = "CancellationId")]
        public string CancellationId { get; set; }

        [XmlElement(ElementName = "CancellationStatus")]
        public string CancellationStatus { get; set; }

        [XmlElement(ElementName = "CancellationCharges")]
        public string CancellationCharges { get; set; }
    }

    [XmlRoot(ElementName = "Cancellations")]
    public class Cancellations
    {
        [XmlElement(ElementName = "Cancellation")]
        public Cancellation Cancellation { get; set; }
    }

    [XmlRoot(ElementName = "EticketCanStatusRes")]
    public class EticketCanStatusRes
    {
        [XmlElement(ElementName = "transid")]
        public string Transid { get; set; }

        [XmlElement(ElementName = "partnerRefId")]
        public string PartnerRefId { get; set; }

        [XmlElement(ElementName = "Cancellations")]
        public Cancellations Cancellations { get; set; }

        [XmlElement(ElementName = "CancellationId")]
        public string CancellationId { get; set; }

        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
    }

    /***Pricing Rsponse***/

    [XmlRoot(ElementName = "pricingresponse")]
    public class Pricingresponse
    {
        [XmlElement(ElementName = "onwardFlights")]
        public OnwardFlights OnwardFlights { get; set; }

        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
    }

    [XmlRoot(ElementName = "onwardFlights")]
    public class OnwardFlights
    {
        [XmlElement(ElementName = "OriginDestinationOption")]
        public OriginDestinationOption OriginDestinationOption { get; set; }
    }

    [XmlRoot(ElementName = "FlightsDetailList")]
    public class FlightsDetailList
    {
        [XmlElement(ElementName = "FlightsDetail")]
        public List<FlightsDetail> FlightsDetail { get; set; }
    }

    [XmlRoot(ElementName = "origindestinationoption")]
    public class Origindestinationoption
    {
        [XmlElement(ElementName = "FlightsDetailList")]
        public FlightsDetailList FlightsDetailList { get; set; }
        [XmlElement(ElementName = "FareDetail")]
        public FareDetail FareDetail { get; set; }
    }

    [XmlRoot(ElementName = "ArrayOfOrigindestinationoption")]
    public class ArrayOfOrigindestinationoption
    {
        [XmlElement(ElementName = "origindestinationoption")]
        public List<Origindestinationoption> Origindestinationoption { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

    public class Passengers
    {
        public string passenger_category { get; set; }
        public string dob {get; set; }
        public string title { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string age { get; set; }
        //public string extra_field_1 { get; set; }
        //public string extra_field_2 { get; set; }
        //public string extra_field_3 { get; set; }
        //public string extra_field_4 { get; set; }
        //public string extra_field_5 { get; set; }
    }

    public class BookingDetail
    {
        public string action { get; set; }
        public string txn_id { get; set; }
        public string request_token { get; set; }
        public int service_id { get; set; }
        public int  sub_service_id { get; set; }
        public string trip_category { get; set; }
        public string category { get; set; }
        public string txn_type { get; set; }
        public string member_id { get; set; }
        public string travel_from { get; set; }
        public string travel_to { get; set; }
        public string travel_date { get; set; }
        public string travel_return_date { get; set; }
        public string company_id { get; set; }
        public string trip_mode { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public string mobile { get; set; }
        public int adult { get; set; }
        public int child { get; set; }
        public int infant { get; set; }
        public double amount { get; set; }
        public double pg_amount { get; set; }
        public double other_amount { get; set; }
        public double total_paid_amount { get; set; }
        public string status { get; set; }
        public string deposit_mode { get; set; }
        public string remarks { get; set; }
        public string airline_code { get; set; }
        public string trip_class { get; set; }
        public string flight_id { get; set; }
        public string flight_no { get; set; }
        public string ref_code { get; set; }
        public string api_txn_id { get; set; }
        public string unique_ref_no { get; set; }
        public string my_info { get; set; }
        public string user_name { get; set; }
        public string txn_date { get; set; }
        public double discount { get; set; }
        public List<Passengers> passenger_details { get; set; }
    }

    public class INSERT_SERVICE_BOOKING_REQUEST
    {
        public int txn_id { get; set; }
        public string MSG { get; set; }
        public string unique_ref_no { get; set; }
    }

    public class UPDATE_TRANSACTION_STATUS
    {        
        public string MSG { get; set; }        
    }

    public class INSERT_PG_REQUEST_FOR_SERVICE
    {
        public int payment_txn_id { get; set; }        
    }

    public class FlightBookingListRequest
    {
        public string member_id { get; set; }
        public string service_id { get; set; }
        public string action { get; set; }
        public string To_date { get; set; }
        public string From_date { get; set; }
        public string Flight_type { get; set; }
        public string Booking_Status { get; set; }        
    }


}

