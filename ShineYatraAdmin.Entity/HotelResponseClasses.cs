namespace ShineYatraAdmin.Entity
{
    using System.Collections.Generic;
    #region namespace

    using System.Xml.Serialization;

    #endregion namespace

    [XmlRoot(ElementName = "contactinfo")]
    public class Contactinfo
    {
        [XmlElement(ElementName = "address")]
        public string Address { get; set; }
        [XmlElement(ElementName = "citywiselocation")]
        public string Citywiselocation { get; set; }
        [XmlElement(ElementName = "locationinfo")]
        public string Locationinfo { get; set; }
        [XmlElement(ElementName = "phone")]
        public string Phone { get; set; }
        [XmlElement(ElementName = "fax")]
        public string Fax { get; set; }
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "website")]
        public string Website { get; set; }
    }

    [XmlRoot(ElementName = "bookinginfo")]
    public class Bookinginfo
    {
        [XmlElement(ElementName = "checkintime")]
        public string Checkintime { get; set; }
        [XmlElement(ElementName = "checkouttime")]
        public string Checkouttime { get; set; }
    }

    [XmlRoot(ElementName = "services")]
    public class Services
    {
        [XmlElement(ElementName = "creditcards")]
        public string Creditcards { get; set; }
        [XmlElement(ElementName = "hotelservices")]
        public string Hotelservices { get; set; }
        [XmlElement(ElementName = "roomservices")]
        public string Roomservices { get; set; }
    }

    [XmlRoot(ElementName = "image")]
    public class Image
    {
        [XmlElement(ElementName = "imagedesc")]
        public string Imagedesc { get; set; }
        [XmlElement(ElementName = "imagepath")]
        public string Imagepath { get; set; }
    }

    [XmlRoot(ElementName = "images")]
    public class Images
    {
        [XmlElement(ElementName = "image")]
        public Image Image { get; set; }
    }

    [XmlRoot(ElementName = "hoteldetail")]
    public class Hoteldetail
    {
        [XmlElement(ElementName = "hotelid")]
        public string Hotelid { get; set; }
        [XmlElement(ElementName = "hotelname")]
        public string Hotelname { get; set; }
        [XmlElement(ElementName = "hoteldesc")]
        public string Hoteldesc { get; set; }
        [XmlElement(ElementName = "starrating")]
        public string Starrating { get; set; }
        [XmlElement(ElementName = "noofrooms")]
        public string Noofrooms { get; set; }
        [XmlElement(ElementName = "minRate")]
        public string MinRate { get; set; }
        [XmlElement(ElementName = "rph")]
        public string Rph { get; set; }
        [XmlElement(ElementName = "webService")]
        public string WebService { get; set; }
        [XmlElement(ElementName = "contactinfo")]
        public Contactinfo Contactinfo { get; set; }
        [XmlElement(ElementName = "bookinginfo")]
        public Bookinginfo Bookinginfo { get; set; }
        [XmlElement(ElementName = "services")]
        public Services Services { get; set; }
        [XmlElement(ElementName = "facilities")]
        public string Facilities { get; set; }
        [XmlElement(ElementName = "images")]
        public Images Images { get; set; }
        [XmlElement(ElementName = "geoCode")]
        public string GeoCode { get; set; }
    }

    [XmlRoot(ElementName = "ratebands")]
    public class Ratebands
    {
        [XmlElement(ElementName = "validdays")]
        public string Validdays { get; set; }
        [XmlElement(ElementName = "wsKey")]
        public string WsKey { get; set; }
        [XmlElement(ElementName = "extGuestTotal")]
        public string ExtGuestTotal { get; set; }
        [XmlElement(ElementName = "roomTotal")]
        public string RoomTotal { get; set; }
        [XmlElement(ElementName = "servicetaxTotal")]
        public string ServicetaxTotal { get; set; }
        [XmlElement(ElementName = "discount")]
        public string Discount { get; set; }
        [XmlElement(ElementName = "commission")]
        public string Commission { get; set; }
        [XmlElement(ElementName = "originalRoomTotal")]
        public string OriginalRoomTotal { get; set; }
        [XmlIgnore]
        public string CommisionGroupAmount { get; set; }
    }

    [XmlRoot(ElementName = "rate")]
    public class Rate
    {
        [XmlElement(ElementName = "ratetype")]
        public string Ratetype { get; set; }
        [XmlElement(ElementName = "hotelPackage")]
        public string HotelPackage { get; set; }
        [XmlElement(ElementName = "roomtype")]
        public string Roomtype { get; set; }
        [XmlElement(ElementName = "roombasis")]
        public string Roombasis { get; set; }
        [XmlElement(ElementName = "roomTypeCode")]
        public string RoomTypeCode { get; set; }
        [XmlElement(ElementName = "ratePlanCode")]
        public string RatePlanCode { get; set; }
        [XmlElement(ElementName = "ratebands")]
        public Ratebands Ratebands { get; set; }
        [XmlElement(ElementName = "discountMessage")]
        public string DiscountMessage { get; set; }
    }

    [XmlRoot(ElementName = "ratedetail")]
    public class Ratedetail
    {
        [XmlElement(ElementName = "rate")]
        public List<Rate> Rate { get; set; }
    }

    [XmlRoot(ElementName = "hotel")]
    public class Hotel
    {
        [XmlElement(ElementName = "hoteldetail")]
        public Hoteldetail Hoteldetail { get; set; }
        [XmlElement(ElementName = "ratedetail")]
        public Ratedetail Ratedetail { get; set; }
        [XmlElement(ElementName = "promotion")]
        public string Promotion { get; set; }
    }

    [XmlRoot(ElementName = "searchresult")]
    public class Searchresult
    {
        [XmlElement(ElementName = "hotel")]
        public List<Hotel> Hotel { get; set; }
    }

    [XmlRoot(ElementName = "arzHotelAvailResp")]
    public class ArzHotelAvailResp
    {
        [XmlElement(ElementName = "searchresult")]
        public Searchresult Searchresult { get; set; }

        [XmlElement(ElementName = "Error")]
        public string Error { get; set; }
    }

    /*Hotel Detail response class*/

    /*Provisionl booking*/

    [XmlRoot(ElementName = "allocresult")]
    public class Allocresult
    {
        [XmlElement(ElementName = "allocavail")]
        public string Allocavail { get; set; }
        [XmlElement(ElementName = "allocid")]
        public string Allocid { get; set; }
    }

    [XmlRoot(ElementName = "arzHotelProvResp")]
    public class ArzHotelProvResp
    {
        [XmlElement(ElementName = "allocresult")]
        public Allocresult Allocresult { get; set; }
    }

    [XmlRoot(ElementName = "bookingresponse")]
    public class HotelBookingresponse
    {
        [XmlElement(ElementName = "bookingstatus")]
        public string Bookingstatus { get; set; }
        [XmlElement(ElementName = "bookingremarks")]
        public string Bookingremarks { get; set; }
        [XmlElement(ElementName = "bookingref")]
        public string Bookingref { get; set; }
        [XmlElement(ElementName = "bookingTrn")]
        public string BookingTrn { get; set; }
    }

    [XmlRoot(ElementName = "arzHotelBookingResp")]
    public class ArzHotelBookingResp
    {
        [XmlElement(ElementName = "bookingresponse")]
        public HotelBookingresponse Bookingresponse { get; set; }
    }

    [XmlRoot(ElementName = "cancellationinfo")]
    public class Cancellationinfo
    {
        [XmlElement(ElementName = "currency")]
        public string Currency { get; set; }
        [XmlElement(ElementName = "error")]
        public string Error { get; set; }
        [XmlElement(ElementName = "success")]
        public string Success
        {
            get; set;
        }
    }

    [XmlRoot(ElementName = "arzHotelCancellationRes")]
    public class ArzHotelCancellationRes
    {
        [XmlElement(ElementName = "cancellationinfo")]
        public Cancellationinfo Cancellationinfo { get; set; }
    }

}
