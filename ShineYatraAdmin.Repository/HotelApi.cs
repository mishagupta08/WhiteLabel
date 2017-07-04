namespace ShineYatraAdmin.Repository
{

    #region namespace

    using Newtonsoft.Json;
    using Entity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System;
    using System.Xml;
    using System.Web;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml.Linq;

    #endregion namespace

    /// <summary>
    /// Hold api functionality
    /// </summary>
    public class HotelApi
    {
        //static void Main()
        //{
        //}

        /// <summary>
        /// Constant for success status
        /// </summary>
        private const string SUCCESS = "SUCCESS";

        /// <summary>
        /// Authentication key
        /// </summary>
        const string HotelAuthKey = "e969da44-91f8-4d51-b138-0ace0980d519";

        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespacesPost(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.Value.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        //replace <br> tag with new line character
        public static string ReplaceBRwithNewline(string txtVal)
        {
            string newText = "";
            // Create regular expressions    
            System.Text.RegularExpressions.Regex regex =
                new System.Text.RegularExpressions.Regex(@"(<br />|<br/>|</ br>|</br>|<br>)");
            // Replace new line with <br/> tag    
            newText = regex.Replace(txtVal, "\r\n");
            // Result    
            return newText;
        }

        #region Hotel API

        /// <summary>
        /// Method to search Hotel
        /// </summary>
        /// <param name="searchHotel"></param>
        /// <returns></returns>
        public static async Task<ArzHotelAvailResp> SearchHotel(HotelRequest searchHotel)
        {
            var hotelList = new ArzHotelAvailResp();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(HotelRequest));
                    httpClient.DefaultRequestHeaders.Add("authKey", HotelAuthKey);

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, searchHotel);

                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Hotel/SearchHotel", httpContent);

                    // If the response contains content we want to read it!

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        if (responseContent.Contains("Error"))
                        {
                            var response = JsonConvert.DeserializeObject<Response>(responseContent);
                        }
                        else
                        {
                            var serializer1 = new XmlSerializer(typeof(ArzHotelAvailResp));
                            string searchResponse = RemoveAllNamespacesPost(responseContent);
                            //searchResponse = HttpUtility.HtmlDecode(searchResponse);
                            //searchResponse = ReplaceBRwithNewline(searchResponse);

                            using (TextReader reader = new StringReader(searchResponse))
                            {
                                hotelList = (ArzHotelAvailResp)serializer1.Deserialize(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //**delete It* start*//
            //if (hotelList == null || hotelList.Searchresult == null)
            //{
            //    hotelList = new ArzHotelAvailResp();
            //    hotelList.Searchresult = new Searchresult();
            //    hotelList.Searchresult.Hotel = new List<Hotel>();
            //    hotelList.Searchresult.Hotel.Add(new Hotel
            //    {
            //        Hoteldetail = new Hoteldetail
            //        {
            //            Hotelid = "00000898",
            //            Hotelname = "The Grand",
            //            WebService = "arzooB",
            //            Hoteldesc = "This luxury hotel is located in the commercial hub of Vasant Kunj area. There are 390 contemporary-styled guest rooms, which overlook the pool and lush green gardens. Business events can be held in any of its 15 spacious convention halls, a business centre and a mini-conference room, which can seat a maximum of five people. Some fine dining options include Brix, an Italian restaurant, Enoki, a Japanese restaurant and Grand Cafe, the 24 hour coffee shop.",
            //            MinRate = "7950",
            //            Contactinfo = new Contactinfo { Address = "Nelson Mandela Marg (close to airport), Vasant Kunj Phase II, Vasant Kunj, NEW DELHI, DELHI, India, Pin-110070" },
            //            Images = new Images { Image = new Image { Imagepath = "cdn.travelpartnerweb.com/DesiyaImages/Image/1/nxd/maw/wye/mbv/HO.jpg" } }
            //        }
            //    });
            //}

            //**delete It* End*//

            return hotelList;
        }

        /// <summary>
        /// Method to search Hotel
        /// </summary>
        /// <param name="searchHotel"></param>
        /// <returns></returns>
        public static async Task<Entity.HotelDetail.ArzHotelDescResp> GetHotelDetail(HotelDescriptionRequest searchHotelDetail)
        {
            var hotelList = new Entity.HotelDetail.ArzHotelDescResp();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(HotelDescriptionRequest));
                    httpClient.DefaultRequestHeaders.Add("authKey", HotelAuthKey);

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, searchHotelDetail);

                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Hotel/SearchHotelDescription", httpContent);

                    // If the response contains content we want to read it!

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        if (responseContent.Contains("error") || responseContent.Contains("FAIL"))
                        {
                            hotelList = null;
                        }
                        else
                        {
                            var serializer1 = new XmlSerializer(typeof(Entity.HotelDetail.ArzHotelDescResp));
                            string searchResponse = RemoveAllNamespacesPost(responseContent);
                            searchResponse = HttpUtility.HtmlDecode(searchResponse);
                            searchResponse = ReplaceBRwithNewline(searchResponse);

                            using (TextReader reader = new StringReader(searchResponse))
                            {
                                hotelList = (Entity.HotelDetail.ArzHotelDescResp)serializer1.Deserialize(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return hotelList;
        }

        /// <summary>
        /// Method to search Hotel
        /// </summary>
        /// <param name="searchHotel"></param>
        /// <returns></returns>
        public static async Task<Entity.HotelDetail.Hotel> SearchHotelWithDetail(HotelRequest searchHotelDetail)
        {
            var hotelList = new Entity.HotelDetail.Hotel();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(HotelRequest));
                    httpClient.DefaultRequestHeaders.Add("authKey", HotelAuthKey);

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, searchHotelDetail);

                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Hotel/SearchHotelWithDetail", httpContent);

                    // If the response contains content we want to read it!

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        if (responseContent.Contains("error") || responseContent.Contains("FAIL"))
                        {
                            hotelList = null;
                        }
                        else
                        {
                            var serializer1 = new XmlSerializer(typeof(Entity.HotelDetail.Hotel));
                            string searchResponse = RemoveAllNamespacesPost(responseContent);
                            //searchResponse = HttpUtility.HtmlDecode(searchResponse);
                            //searchResponse = ReplaceBRwithNewline(searchResponse);

                            using (TextReader reader = new StringReader(searchResponse))
                            {
                                hotelList = (Entity.HotelDetail.Hotel)serializer1.Deserialize(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            /*****Delete Start*****/

            if (hotelList == null || hotelList.Hoteldetail == null)
            {
                hotelList = new Entity.HotelDetail.Hotel();
                hotelList.Hoteldetail = new Entity.HotelDetail.Hoteldetail();
                hotelList.Hoteldetail.Hotelname = "The Grand";
                hotelList.Hoteldetail.HotelId = "00000898";
                hotelList.Hoteldetail.Hoteldesc = "This luxury hotel is located in the commercial hub of Vasant Kunj area. There are 390 contemporary-styled guest rooms, which overlook the pool and lush green gardens. Business events can be held in any of its 15 spacious convention halls, a business centre and a mini-conference room, which can seat a maximum of five people.";
                hotelList.Hoteldetail.Starrating = "5";
                hotelList.Hoteldetail.Noofrooms = "1";
                hotelList.Hoteldetail.MinRate = "7500";
                hotelList.Hoteldetail.Contactinfo = new Entity.HotelDetail.Contactinfo();
                hotelList.Hoteldetail.Contactinfo.Address = "Nelson Mandela Marg (close to airport), Vasant Kunj Phase II, Vasant Kunj, NEW DELHI, DELHI, India, Pin-110070";
                hotelList.Hoteldetail.Contactinfo.Citywiselocation = "Vasant Kunj";

                hotelList.Hoteldetail.Images = new Entity.HotelDetail.Images();
                hotelList.Hoteldetail.Images.Image = new List<Entity.HotelDetail.Image>();
                hotelList.Hoteldetail.Images.Image.Add(new Entity.HotelDetail.Image { Imagepath = "http://cdn.travelpartnerweb.com/DesiyaImages/Image/1/nxd/maw/wye/mbv/0000024122RD.jpg" });
                hotelList.Hoteldetail.Images.Image.Add(new Entity.HotelDetail.Image { Imagepath = "http://cdn.travelpartnerweb.com/DesiyaImages/Image/1/nxd/maw/wye/mbv/0000024905RD.jpg" });
                hotelList.Hoteldetail.Images.Image.Add(new Entity.HotelDetail.Image { Imagepath = "http://cdn.travelpartnerweb.com/DesiyaImages/Image/1/nxd/maw/wye/mbv/0000023333RD.jpg" });
                hotelList.Hoteldetail.Images.Image.Add(new Entity.HotelDetail.Image { Imagepath = "http://cdn.travelpartnerweb.com/DesiyaImages/Image/1/nxd/maw/wye/mbv/XT1.jpg" });
                hotelList.Hoteldetail.Images.Image.Add(new Entity.HotelDetail.Image { Imagepath = "http://cdn.travelpartnerweb.com/DesiyaImages/Image/1/nxd/maw/wye/mbv/XT2.jpg" });
                hotelList.Hoteldetail.Images.Image.Add(new Entity.HotelDetail.Image { Imagepath = "http://cdn.travelpartnerweb.com/DesiyaImages/Image/1/nxd/maw/wye/mbv/XT3.jpg" });
                hotelList.Hoteldetail.Images.Image.Add(new Entity.HotelDetail.Image { Imagepath = "http://cdn.travelpartnerweb.com/DesiyaImages/Image/1/nxd/maw/wye/mbv/XT4.jpg" });

                hotelList.Ratedetail = new Ratedetail();
                hotelList.Ratedetail.Rate = new List<Rate>();

                hotelList.Ratedetail.Rate.Add(new Rate
                {
                    Roomtype = "Grand Premium Room Only TG Special",
                    Roombasis = "Complimentary Wi-Fi Internet, Taxes, ,10% Discount on Food and Beverages, Free room upgrade subject to availability",
                    RoomTypeCode = "0000184150",
                    RatePlanCode = "0000689459",
                    Ratebands = new Ratebands
                    {
                        Validdays = "1111111",
                        WsKey = "0uNZYHRVtT86eaST9cxEwBtzLdlIYUjBSYrQfbcoJRCld4hJqJ5J9mQ5oOVMvpmMisxuMwbRRW6LLccpNa9nxPbGM/bkO58ExDrDapJ4gq7B45RuCOzCgZa1JrJic2xK3xdveE9MzkF4s5fZZJnrk8Q6w2qSeIKuTCmFk8/vQHOnOnMaLOVKWsdPBf4axB3HSAQ8fAGeVbM9mGG2sw8eqaQkUVa5/1sIacktjDU7bFnb1KlhjkBUq6gxwf4UlPxISbC0fJxNC5KrMfmoT8DddxxsuMjFZPrR2HNjrW4zbuWzh1N9R0wT/JyOqGfxpfM2OrRdrunn1SpGytYyCpMf+7ni7GIfP7p8bEn4el2ypdbEOsNqkniCrpj/9TK3zkwEn4doaXhLJbPB45RuCOzCgZ+HaGl4SyWzweOUbgjswoFWp5nZ86iUG8Q6w2qSeIKu0Xg6kx6/VIvlRsv0brlhIgMdJk/xNq3JiX6Yfx2T7InULFBeohhejfflIYqwnPKwdzG+E+oGbFE+BrItg0qsrxwWtpoRqZ8/weOUbgjswoEeJcWhQsqZrg==",
                        ExtGuestTotal = "0",
                        RoomTotal = "7500",
                        ServicetaxTotal = "2387",
                        Discount = "345.0",
                        Commission = "0",
                        OriginalRoomTotal = "7950"
                    }
                });

                hotelList.Ratedetail.Rate.Add(new Rate
                {
                    Roomtype = "Grand Premium Room with Breakfast TG Special",
                    Roombasis = "Breakfast, Complimentary Wi-Fi Internet, Taxes,",
                    RoomTypeCode = "0000184151",
                    RatePlanCode = "0000689463",
                    Ratebands = new Ratebands
                    {
                        Validdays = "1111111",
                        WsKey = "xOpflrzr4ys6eaST9cxEwF5673/Pk2shweOUbgjswoGld4hJqJ5J9mQ5oOVMvpmMisxuMwbRRW4A8hCXqzdfkKw/Zffn7OMNxDrDapJ4gq7B45RuCOzCgZa1JrJic2xK3xdveE9MzkF4s5fZZJnrk8Q6w2qSeIKuTCmFk8/vQHOnOnMaLOVKWsdPBf4axB3HSAQ8fAGeVbNkf/7tWw7snL70mO3wUpfr7ANOsUaHXl4UGSHhnwqssBs1/1meUilv0CHNs6neM9a1nanBWALEewny+Iue3pdZ77m1Gq9TRy/TPACodTPy+u+5tRqvU0cv0zwAqHUz8vrzKoWaVPsO89db6E2oYWHjgrTTQ8Uz3Kgwf+IA4CTtS3purLcJG+UHw4dZqdlQMYB+neNrhA8rSPZvCoSerqeoo/LJSmYowiFZ1xBD2luqCfh+69pQRSEbyacZpMpYFYdIkvUy+xCoaBfc5EobFJGF",
                        ExtGuestTotal = "0",
                        RoomTotal = "8750",
                        ServicetaxTotal = "2500",
                        Discount = "402.0",
                        Commission = "0",
                        OriginalRoomTotal = "9276"
                    }
                });

                hotelList.Ratedetail.Rate.Add(new Rate
                {
                    Roomtype = "Grand Premium Room Only",
                    Roombasis = "Complimentary Wi-Fi Internet, ,10% Discount on Food and Beverages, 15% discount on all other SPA therapies.",
                    RoomTypeCode = "0000024903",
                    RatePlanCode = "0000609988",
                    Ratebands = new Ratebands
                    {
                        Validdays = "1111111",
                        WsKey = "vEv28ofDPCY6eaST9cxEwDcz+kdacs1mFluz6IhIkl6ld4hJqJ5J9mQ5oOVMvpmMJg3iwYJnLRbMxoQOqXitOpTH0oV0EbXNxDrDapJ4gq7B45RuCOzCgZa1JrJic2xK3xdveE9MzkF4s5fZZJnrk8Q6w2qSeIKuTCmFk8/vQHOnOnMaLOVKWsdPBf4axB3HSAQ8fAGeVbM9mGG2sw8eqaQkUVa5/1sIacktjDU7bFnb1KlhjkBUq1zoMYzvQHxQbGwOiAVCvPojxu8IaDP0HNsMXUof3PxO6nyKY9j9b5HBlETKY7vedg6Lsdfc5dF9taSo1c0E+u+gnLQNydaucsRXGFNZV6c5xDrDapJ4gq6Y//Uyt85MBJ+HaGl4SyWzweOUbgjswoGfh2hpeEsls8HjlG4I7MKBVqeZ2fOolBvEOsNqkniCrtF4OpMev1SL5UbL9G65YSIDHSZP8TatyYl+mH8dk+yJ1CxQXqIYXo335SGKsJzysOe8ozEaHlntiPUp4z2XOsuYmbGlICASnMHjlG4I7MKBQifjBI72IvM=",
                        ExtGuestTotal = "0",
                        RoomTotal = "9500",
                        ServicetaxTotal = "2463",
                        Discount = "437.0",
                        Commission = "0",
                        OriginalRoomTotal = "10070"
                    }
                });
            }

            /*****Delete End*****/

            return hotelList;
        }

        /// <summary>
        /// Method to search Hotel
        /// </summary>
        /// <param name="searchHotel"></param>
        /// <returns></returns>
        public static async Task<ArzHotelProvResp> ProvisionBooking(ProvisionalBooking provisionalBooking)
        {
            var hotelList = new ArzHotelProvResp();
            provisionalBooking.Ratebands.OriginalRoomTotal = null;
            if (provisionalBooking.GuestInformation.MiddleName == null)
            {
                provisionalBooking.GuestInformation.MiddleName = string.Empty;
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(ProvisionalBooking));
                    httpClient.DefaultRequestHeaders.Add("authKey", HotelAuthKey);

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, provisionalBooking);

                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Hotel/ProvisionalBooking", httpContent);

                    // If the response contains content we want to read it!

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        if (responseContent.Contains("error") || responseContent.Contains("FAIL"))
                        {
                            hotelList = null;
                        }
                        else
                        {
                            var serializer1 = new XmlSerializer(typeof(ArzHotelProvResp));
                            string searchResponse = RemoveAllNamespacesPost(responseContent);
                            //searchResponse = HttpUtility.HtmlDecode(searchResponse);
                            //searchResponse = ReplaceBRwithNewline(searchResponse);

                            using (TextReader reader = new StringReader(searchResponse))
                            {
                                hotelList = (ArzHotelProvResp)serializer1.Deserialize(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return hotelList;
        }

        /// <summary>
        /// Method to search Hotel
        /// </summary>
        /// <param name="searchHotel"></param>
        /// <returns></returns>
        public static async Task<ArzHotelBookingResp> BookingConfirmation(HotelDescriptionRequest provisionalBooking)
        {
            var hotelList = new ArzHotelBookingResp();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(HotelDescriptionRequest));
                    httpClient.DefaultRequestHeaders.Add("authKey", HotelAuthKey);

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, provisionalBooking);

                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Hotel/BookingConfirmation", httpContent);

                    // If the response contains content we want to read it!

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        if (responseContent.Contains("error") || responseContent.Contains("FAIL"))
                        {
                            hotelList = null;
                        }
                        else
                        {
                            var serializer1 = new XmlSerializer(typeof(ArzHotelBookingResp));
                            string searchResponse = RemoveAllNamespacesPost(responseContent);
                            searchResponse = HttpUtility.HtmlDecode(searchResponse);
                            searchResponse = ReplaceBRwithNewline(searchResponse);

                            using (TextReader reader = new StringReader(searchResponse))
                            {
                                hotelList = (ArzHotelBookingResp)serializer1.Deserialize(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return hotelList;
        }

        /// <summary>
        /// Method to search Hotel
        /// </summary>
        /// <param name="searchHotel"></param>
        /// <returns></returns>
        public static async Task<ArzHotelCancellationRes> BookingCancellation(HotelDescriptionRequest provisionalBooking)
        {
            var hotelList = new ArzHotelCancellationRes();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(HotelDescriptionRequest));
                    httpClient.DefaultRequestHeaders.Add("authKey", HotelAuthKey);

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, provisionalBooking);

                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Hotel/BookingCancellation", httpContent);

                    // If the response contains content we want to read it!

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        if (responseContent.Contains("error") || responseContent.Contains("FAIL"))
                        {
                            hotelList = null;
                        }
                        else
                        {
                            var serializer1 = new XmlSerializer(typeof(ArzHotelCancellationRes));
                            string searchResponse = RemoveAllNamespacesPost(responseContent);
                            searchResponse = HttpUtility.HtmlDecode(searchResponse);
                            searchResponse = ReplaceBRwithNewline(searchResponse);

                            using (TextReader reader = new StringReader(searchResponse))
                            {
                                hotelList = (ArzHotelCancellationRes)serializer1.Deserialize(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return hotelList;
        }

        #endregion  Hotel API
    }
}