namespace ShineYatraAdmin.Entity
{
    #region namespace

    using System.Xml.Serialization;

    #endregion namespace


    [XmlRoot(ElementName = "child")]
    public class Child
    {
        [XmlElement(ElementName = "age")]
        public string Age { get; set; }
    }

    [XmlRoot(ElementName = "guestDetails")]
    public class GuestDetails
    {
        [XmlElement(ElementName = "adults")]
        public string Adults { get; set; }

        [XmlElement(ElementName = "child")]
        public Child Child { get; set; }
    }

    [XmlRoot(ElementName = "HotelRequest")]
    public class HotelRequest
    {
        [XmlElement(ElementName = "guestDetails")]
        public GuestDetails GuestDetails { get; set; }

        [XmlElement(ElementName = "start")]
        public string Start { get; set; }

        [XmlElement(ElementName = "end")]
        public string End { get; set; }

        [XmlElement(ElementName = "hotelCityName")]
        public string HotelCityName { get; set; }

        [XmlElement(ElementName = "hotelName")]
        public string HotelName { get; set; }

        [XmlElement(ElementName = "area")]
        public string Area { get; set; }

        [XmlElement(ElementName = "attraction")]
        public string Attraction { get; set; }

        [XmlElement(ElementName = "rating")]
        public string Rating { get; set; }

        [XmlElement(ElementName = "hotelPackage")]
        public string HotelPackage { get; set; }

        [XmlElement(ElementName = "sortingPreference")]
        public string SortingPreference { get; set; }

        [XmlElement(ElementName = "hotelid")]
        public string hotelid { get; set; }

        [XmlElement(ElementName = "webService")]
        public string webService { get; set; }

        [XmlElement(ElementName = "roomCode")]
        public string roomCode { get; set; }

    }

    /// <summary>
    /// Holds Hotel Description Request
    /// </summary>
    public class HotelDescriptionRequest
    {
        [XmlElement(ElementName = "hotelid")]
        public string hotelid { get; set; }

        [XmlElement(ElementName = "webService")]
        public string webService { get; set; }

        [XmlElement(ElementName = "city")]
        public string city { get; set; }

        [XmlElement(ElementName = "roomTypeCode")]
        public string RoomTypeCode { get; set; }

        [XmlElement(ElementName = "checkInDate")]
        public string CheckInDate { get; set; }

        [XmlElement(ElementName = "checkOutDate")]
        public string CheckOutDate { get; set; }

        [XmlElement(ElementName = "ratePlanType")]
        public string RatePlanType { get; set; }

        [XmlElement(ElementName = "fromallocation")]
        public string Fromallocation { get; set; }

        [XmlElement(ElementName = "allocid")]
        public string Allocid { get; set; }

        [XmlElement(ElementName = "roombasis")]
        public string Roombasis { get; set; }

        [XmlElement(ElementName = "roomStayCandidate")]
        public RoomStayCandidate RoomStayCandidate { get; set; }

        [XmlElement(ElementName = "guestInformation")]
        public GuestInformation GuestInformation { get; set; }

        [XmlElement(ElementName = "roomtype")]
        public string Roomtype { get; set; }

        [XmlElement(ElementName = "wsKey")]
        public string WsKey { get; set; }

        [XmlElement(ElementName = "lastName")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "bookingref")]
        public string Bookingref { get; set; }

        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
    }
    
    [XmlRoot(ElementName = "hotelinfo")]
    public class Hotelinfo
    {
        [XmlElement(ElementName = "hotelid")]
        public string Hotelid { get; set; }
        [XmlElement(ElementName = "roomtype")]
        public string Roomtype { get; set; }
        [XmlElement(ElementName = "webService")]
        public string WebService { get; set; }
        [XmlElement(ElementName = "fromdate")]
        public string Fromdate { get; set; }
        [XmlElement(ElementName = "todate")]
        public string Todate { get; set; }
        [XmlElement(ElementName = "roomTypeCode")]
        public string RoomTypeCode { get; set; }
        [XmlElement(ElementName = "ratePlanType")]
        public string RatePlanType { get; set; }
    }

    [XmlRoot(ElementName = "roomStayCandidate")]
    public class RoomStayCandidate
    {
        [XmlElement(ElementName = "guestDetails")]
        public GuestDetails GuestDetails { get; set; }
    }

    [XmlRoot(ElementName = "phoneNumber")]
    public class PhoneNumber
    {
        [XmlElement(ElementName = "countryCode")]
        public string CountryCode { get; set; }
        [XmlElement(ElementName = "areaCode")]
        public string AreaCode { get; set; }
        [XmlElement(ElementName = "number")]
        public string Number { get; set; }
        [XmlElement(ElementName = "extension")]
        public string Extension { get; set; }
    }

    [XmlRoot(ElementName = "address")]
    public class Address
    {
        [XmlElement(ElementName = "addressLine")]
        public string AddressLine { get; set; }
        [XmlElement(ElementName = "city")]
        public string City { get; set; }
        [XmlElement(ElementName = "zipCode")]
        public string ZipCode { get; set; }
        [XmlElement(ElementName = "state")]
        public string State { get; set; }
        [XmlElement(ElementName = "country")]
        public string Country { get; set; }
    }

    [XmlRoot(ElementName = "guestInformation")]
    public class GuestInformation
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "firstName")]
        public string FirstName { get; set; }
        [XmlElement(ElementName = "middleName")]
        public string MiddleName { get; set; }
        [XmlElement(ElementName = "lastName")]
        public string LastName { get; set; }
        [XmlElement(ElementName = "phoneNumber")]
        public PhoneNumber PhoneNumber { get; set; }
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "address")]
        public Address Address { get; set; }
        [XmlElement(ElementName = "residentOfIndia")]
        public string ResidentOfIndia { get; set; }
    }

    [XmlRoot(ElementName = "ProvisionalBooking")]
    public class ProvisionalBooking
    {
        [XmlElement(ElementName = "hotelinfo")]
        public Hotelinfo Hotelinfo { get; set; }
        [XmlElement(ElementName = "roomStayCandidate")]
        public RoomStayCandidate RoomStayCandidate { get; set; }
        [XmlElement(ElementName = "ratebands")]
        public Ratebands Ratebands { get; set; }
        [XmlElement(ElementName = "guestInformation")]
        public GuestInformation GuestInformation { get; set; }
        [XmlElement(ElementName = "residentOfIndia")]
        public string ResidentOfIndia { get; set; }
    }

}
