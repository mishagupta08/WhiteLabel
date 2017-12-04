namespace ShineYatraAdmin.Entity.HotelDetail
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
        [XmlElement(ElementName = "pincode")]
        public string Pincode { get; set; }
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
        [XmlElement(ElementName = "imagepath")]
        public string Imagepath { get; set; }
    }

    [XmlRoot(ElementName = "images")]
    public class Images
    {
        [XmlElement(ElementName = "image")]
        public List<Image> Image { get; set; }
    }

    [XmlRoot(ElementName = "hoteldetail")]
    public class Hoteldetail
    {
        [XmlElement(ElementName = "hotelid")]
        public string HotelId { get; set; }
        [XmlElement(ElementName = "hotelname")]
        public string Hotelname { get; set; }
        [XmlElement(ElementName = "hoteldesc")]
        public string Hoteldesc { get; set; }
        [XmlElement(ElementName = "hotelchain")]
        public string Hotelchain { get; set; }
        [XmlElement(ElementName = "starrating")]
        public string Starrating { get; set; }
        [XmlElement(ElementName = "city")]
        public string City { get; set; }
        [XmlElement(ElementName = "country")]
        public string Country { get; set; }
        [XmlElement(ElementName = "noofrooms")]
        public string Noofrooms { get; set; }
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
        [XmlElement(ElementName = "minrate")]
        public string MinRate { get; set; }
        [XmlIgnore]
        public string HeadOfficeImage { get; set; }
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

        [XmlElement(ElementName = "Error")]
        public string Error { get; set; }
    }

    [XmlRoot(ElementName = "searchresult")]
    public class Searchresult
    {
        [XmlElement(ElementName = "hotel")]
        public Hotel Hotel { get; set; }
    }

    [XmlRoot(ElementName = "arzHotelDescResp")]
    public class ArzHotelDescResp
    {
        [XmlElement(ElementName = "searchresult")]
        public Searchresult Searchresult { get; set; }

        public string Error { get; set; }
    }

    public class HotelBoookingDetail
    {
        public string action { get; set; }
        public string domain_name { get; set; }
        public string request_token { get; set; }
        public string txn_type { get; set; }
        public string category { get; set; }
        public string member_id { get; set; }
        public double amount { get; set; }
        public int service_id { get; set; }
        public int sub_service_id { get; set; }
        public string destination { get; set; }
        public string check_in { get; set; }
        public string check_out { get; set; }
        public int adults { get; set; }
        public int childern { get; set; }
        public string title { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string mobile_number { get; set; }
        public string email { get; set; }
        public string address_line1 { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string my_info { get; set; }
        public string remarks { get; set; }
        public double pg_amount { get; set; }
        public double other_amount { get; set; }
        public double total_paid_amount { get; set; }
        public double discount { get; set; }
        public string room_type { get; set; }
        public string check_in_time { get; set; }
        public string check_out_time { get; set; }
        public string hotel_name { get; set; }
        public string hotel_city { get; set; }
        public string hotel_address { get; set; }
        public string hotel_amenities { get; set; }

        public string adult_count { get; set; }
        public string child_count { get; set; }
        public string room_count { get; set; }
        public string webservice { get; set; }
        public string booking_ref_no { get; set; }
    }
}
