namespace ShineYatraAdmin.Controllers
{
    #region namespace
    using Entity;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ShineYatraAdmin.Business;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Properties;
    using Common;
    using System.Web.Script.Serialization;
    using System.Web;
    using System.Configuration;
    using Newtonsoft.Json;

    #endregion namespace

    /// <summary>
    /// Holds Hotel operations
    /// </summary>
    public class HotelController : Controller
    {
        public const string FIVE = "5";
        public const string FOUR = "4";
        public const string THREE = "3";
        public const string TWO = "2";
        public const string ONE = "1";
        public int MINPRICE = Convert.ToInt32(Resources.MinPrice);
        public int MAXPRICE = Convert.ToInt32(Resources.MaxPrice);

        /// <summary>
        /// Hold flight model
        /// </summary>
        HotelViewModel hotelModel = new HotelViewModel();

        /// <summary>
        /// hold HotelManager
        /// </summary>
        HotelManager hotelManager;

        private ServiceManager _serviceManager;
        private PgManager _pgManager;
        private UserManager _userManager;

        FlightManager _flightManager;

        // GET: Hotel
        public async Task<ActionResult> Index()
        {
            if (ShineYatraSession.HotelCityList == null || ShineYatraSession.HotelCityList.Count == 0)
            {
                ShineYatraSession.HotelCityList = await _flightManager.GetFlightCityList(false);
            }
            return View();
        }

        /// <summary>
        /// method to get flight menu
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetHotelMenu(string menu)
        {
            /***Payment****/

            //PayUController cntrl = new PayUController();
            //PayuRequest request = new PayuRequest();
            //request.FirstName = "Bindu";
            //request.TransactionAmount = "1.0";
            //request.Email = "bindu89kasera@gmail.com";
            //request.Phone = "9829221803";
            //request.ProductInfo = "Booking Hotel Testing";
            //request.surl = "http://" + Request.Url.Authority + "/PayU/Return";
            //request.furl = "http://" + Request.Url.Authority + "/PayU/Return";

            //cntrl.Payment(request);

            /***Payment****/

            try
            {
                if (string.IsNullOrEmpty(menu))
                {
                    return null;
                }

                if (ShineYatraSession.HotelCityList == null || ShineYatraSession.HotelCityList.Count == 0)
                {
                    _flightManager = new FlightManager();
                    ShineYatraSession.HotelCityList = await _flightManager.GetFlightCityList(false);
                }

                hotelModel.SelectedMenu = menu;
                hotelModel.HotelRequestDetail = new HotelRequest();
                hotelModel.HotelRequestDetail.RoomCount = 1;

                hotelModel.HotelRequestDetail.RoomStayCandidateDetail = new RoomStayCandidate();
                hotelModel.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails = new List<GuestDetails>();
                for (var i = 0; i < 4; i++)
                {
                    var detail = new GuestDetails();

                    detail.Child = new Child();
                    detail.Child.Age = new List<string>();
                    detail.Child.Age.Add(string.Empty);
                    detail.Child.Age.Add(string.Empty);

                    hotelModel.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails.Add(detail);

                }

                hotelModel.AssignRooms();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }
            return View("Index", hotelModel);
        }

        public async Task<ActionResult> SearchHotel(HotelViewModel hotelDetail)
        {
            HotelViewModel hotelDetailListViewModel = new HotelViewModel();

            try
            {
                //ShineYatraSession.HotelRequest = new HotelRequest();
                //ShineYatraSession.HotelRequest.HotelCityName = hotelDetail.HotelRequestDetail.HotelCityName;
                //ShineYatraSession.HotelRequest.Start = hotelDetail.HotelRequestDetail.Start;
                //ShineYatraSession.HotelRequest.End = hotelDetail.HotelRequestDetail.End;

                /**For making guest objects*Start*/

                var guestList = new List<GuestDetails>();
                for (var i = 0; i < hotelDetail.HotelRequestDetail.RoomCount; i++)
                {
                    guestList.Add(hotelDetail.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails[i]);

                    if (hotelDetail.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails[i].ChildCount > 0)
                    {
                        var ageList = new List<string>();
                        for (var j = 0; j < hotelDetail.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails[i].ChildCount; j++)
                        {
                            ageList.Add(hotelDetail.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails[i].Child.Age[j]);
                        }

                        hotelDetail.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails[i].Child = new Child();
                        hotelDetail.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails[i].Child.Age = ageList;
                    }
                }

                hotelDetail.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails = guestList;

                foreach (var guest in hotelDetail.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails)
                {
                    hotelDetail.HotelRequestDetail.TotalAdultCount += guest.AdultCount;
                    hotelDetail.HotelRequestDetail.TotalChildCount += guest.ChildCount;

                    guest.Adults = guest.AdultCount.ToString();
                }

                /**For making guest objects*End*/

                hotelDetailListViewModel.hotelRequestCookieId = this.SaveHotelQuery(hotelDetail, string.Empty);
                this.hotelManager = new HotelManager();
                hotelDetailListViewModel.HotelRequestDetail = hotelDetail.HotelRequestDetail;
                hotelDetailListViewModel.HotelResultDashboardDetail = new HotelResultDashboard();

                var response = await this.hotelManager.SearchHotel(hotelDetail.HotelRequestDetail);

                hotelDetailListViewModel.AssignRooms();
                if (response != null && response.Searchresult != null)
                {
                    hotelDetailListViewModel.HotelList = response.Searchresult.Hotel;
                    hotelDetailListViewModel.HotelResultDashboardDetail.FiveStartCount = hotelDetailListViewModel.HotelList.Count(h => h.Hoteldetail != null && !string.IsNullOrEmpty(h.Hoteldetail.Starrating) && (h.Hoteldetail.Starrating.Trim() == FIVE));
                    hotelDetailListViewModel.HotelResultDashboardDetail.FourStartCount = hotelDetailListViewModel.HotelList.Count(h => h.Hoteldetail != null && !string.IsNullOrEmpty(h.Hoteldetail.Starrating) && (h.Hoteldetail.Starrating.Trim() == FOUR));
                    hotelDetailListViewModel.HotelResultDashboardDetail.ThreeStartCount = hotelDetailListViewModel.HotelList.Count(h => h.Hoteldetail != null && !string.IsNullOrEmpty(h.Hoteldetail.Starrating) && (h.Hoteldetail.Starrating.Trim() == THREE));
                    hotelDetailListViewModel.HotelResultDashboardDetail.TwoStartCount = hotelDetailListViewModel.HotelList.Count(h => h.Hoteldetail != null && !string.IsNullOrEmpty(h.Hoteldetail.Starrating) && (h.Hoteldetail.Starrating.Trim() == TWO));
                    hotelDetailListViewModel.HotelResultDashboardDetail.OneStartCount = hotelDetailListViewModel.HotelList.Count(h => h.Hoteldetail != null && !string.IsNullOrEmpty(h.Hoteldetail.Starrating) && (h.Hoteldetail.Starrating.Trim() == ONE));


                    /**Need to call commission or discount service call*Start*/
                    //ServiceManager serviceManager = new ServiceManager();
                    //var userData = User.Identity.Name.Split('|');
                    ////IList<CompanyCommissionGroup> allflightDiscountDetails = await serviceManager.GetSErviceAllottedGroupDetails(userData[1], "", "", "", "");
                    //var serviceCgDetailsRequest = new AllotedServiceCGsDetailsRequest
                    //{
                    //    member_id = userData[1],
                    //    action = "GET_ALLOTED_SERVICE_COMMISSION_GROUPS_DETAILS",
                    //    service_id = Common.getIdbyServiceName("HOTEL"),
                    //    sub_service_id = "0",
                    //    category = "HOTEL",
                    //    sub_category = "HOTELS",
                    //    service_code = "0"
                    //};

                    //var allDiscountDetails = await serviceManager.GetServiceAllottedGroupDetails(serviceCgDetailsRequest);

                    //CompanyCommissionGroup hotelDiscount = allDiscountDetails.Where(o => o.sub_service_id.Equals(Common.getIdbyServiceName("HOTEL"))).FirstOrDefault();

                    //var amount = string.Empty;
                    //if (hotelDiscount != null && Convert.ToDouble(hotelDiscount.back_discount_per) != 0)
                    //{
                    //    amount = hotelDiscount.back_discount_amount.ToString();
                    //}
                    //else
                    //{
                    //    if (hotelDiscount != null)
                    //    {
                    //        amount = hotelDiscount.back_discount_amount.ToString();
                    //    }
                    //}

                    //foreach (var hotel in hotelDetailListViewModel.HotelList)
                    //{
                    //    foreach (var rate in hotel.Ratedetail.Rate)
                    //    {
                    //        rate.Ratebands.CommisionGroupAmount = amount;
                    //    }
                    //}

                    /**Need to call commission or discount service call*End*/
                }
                else
                {
                    hotelDetailListViewModel.HotelList = new List<Hotel>();
                    hotelDetailListViewModel.Error = response.Error;
                }

                /***Filter Hotel list end*****/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            if (hotelDetailListViewModel == null)
            {
                return Json(string.Empty);
            }
            else
            {
                return View("hotelListView", hotelDetailListViewModel);
            }
        }

        public ActionResult GetHotelCancelView()
        {
            this.hotelModel = new HotelViewModel();
            return View("CancelHotelView", this.hotelModel);

        }

        public async Task<ActionResult> SubmitHotelCancelrequest(HotelViewModel cancelModel)
        {
            this.hotelManager = new HotelManager();
            var response = await this.hotelManager.BookingCancellation(cancelModel.HotelDescRequest);
            if (response == null || response.Cancellationinfo == null)
            {
                return Json(string.Empty);
            }
            else
            {
                if (string.IsNullOrEmpty(response.Cancellationinfo.Error))
                {
                    return Json(response.Cancellationinfo.Success);
                }
                return Json(response.Cancellationinfo.Error);
            }
        }

        public async Task<ActionResult> GetHotelDetailView(string hotelId, string hotelWebService, string cookieId)
        {
            this.hotelModel = new HotelViewModel();
            try
            {
                if (string.IsNullOrEmpty(hotelId) || string.IsNullOrEmpty(hotelWebService) || string.IsNullOrEmpty(cookieId))
                {
                    return null;
                }

                var hotelRequestCookie = Request.Cookies[cookieId];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (hotelRequestCookie != null)
                {
                    var hotelRequestCookieModel = serializer.Deserialize<HotelViewModel>(hotelRequestCookie.Value);
                    hotelManager = new HotelManager();
                    var request = new HotelRequest();
                    request = hotelRequestCookieModel.HotelRequestDetail;
                    request.hotelid = hotelId;
                    request.webService = hotelWebService;

                    var response = await hotelManager.SearchHotelWithDetail(request);

                    if (response == null || response.Hoteldetail == null)
                    {
                        this.hotelModel.Error = "Detail Not Found";
                    }
                    else
                    {
                        if (response.Hoteldetail.Images != null || response.Hoteldetail.Images.Image != null)
                        {
                            var HeadImage = response.Hoteldetail.Images.Image.FirstOrDefault(x => x.Imagepath != null && x.Imagepath.ToUpper().Contains("HO."));
                            if (HeadImage == null)
                            {
                                response.Hoteldetail.HeadOfficeImage = response.Hoteldetail.Images.Image.FirstOrDefault().Imagepath;
                            }
                            else
                            {
                                response.Hoteldetail.HeadOfficeImage = HeadImage.Imagepath;
                            }
                        }

                        request.HotelName = response.Hoteldetail.Hotelname;

                        /**Need to call commission or discount service call*Start*/
                        ServiceManager serviceManager = new ServiceManager();
                        var userData = User.Identity.Name.Split('|');
                        //IList<CompanyCommissionGroup> allflightDiscountDetails = await serviceManager.GetSErviceAllottedGroupDetails(userData[1], "", "", "", "");
                        var serviceCgDetailsRequest = new AllotedServiceCGsDetailsRequest
                        {
                            member_id = userData[1],
                            action = "GET_ALLOTED_SERVICE_COMMISSION_GROUPS_DETAILS",
                            service_id = Common.getIdbyServiceName("HOTEL"),
                            sub_service_id = "0",
                            category = "HOTEL",
                            sub_category = "HOTELS",
                            service_code = "0"
                        };
                        var allDiscountDetails = await serviceManager.GetServiceAllottedGroupDetails(serviceCgDetailsRequest);

                        CompanyCommissionGroup hotelDiscount = allDiscountDetails.FirstOrDefault();

                        var amount = string.Empty;
                        if (hotelDiscount != null && Convert.ToDouble(hotelDiscount.back_discount_per) != 0)
                        {
                            amount = hotelDiscount.back_discount_amount.ToString();
                        }
                        else
                        {
                            if (hotelDiscount != null)
                            {
                                amount = hotelDiscount.back_discount_amount.ToString();
                            }
                        }

                        foreach (var rate in response.Ratedetail.Rate)
                        {
                            rate.Ratebands.CommisionGroupAmount = amount;
                        }

                        /**Need to call commission or discount service call*End*/

                        this.hotelModel.HotelRequestDetail = request;
                        this.hotelModel.SelectedHotel = response;
                        hotelModel.HotelRequestDetail.NightCount = CalculateDays(hotelModel.HotelRequestDetail.Start, hotelModel.HotelRequestDetail.End);
                        //HttpContext.Response.Cookies.Remove(cookieId);
                        hotelRequestCookieModel = new HotelViewModel();
                        hotelRequestCookieModel.HotelRequestDetail = hotelModel.HotelRequestDetail;

                        var cId = this.SaveHotelQuery(hotelRequestCookieModel, cookieId);
                        this.hotelModel.hotelRequestCookieId = cId;

                        this.SaveRateDetail(response.Ratedetail, cookieId + "Ratedetail");
                    }
                }
            }
            catch (Exception e)
            {
                this.hotelModel.Error = e.Message;
            }

            //this.hotelModel.hotelRequestCookieId = cookieId;
            return View("hotelDetailView", this.hotelModel);
        }

        public ActionResult BookHotelView(string roomCode, string hotelRequestCookieId)
        {
            try
            {
                if (string.IsNullOrEmpty(roomCode) || string.IsNullOrEmpty(hotelRequestCookieId))
                {
                    return null;
                }

                var hotelRequestCookie = Request.Cookies[hotelRequestCookieId];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var hotelRequestCookieModel = new HotelViewModel();
                if (hotelRequestCookie != null)
                {
                    hotelRequestCookieModel = serializer.Deserialize<HotelViewModel>(hotelRequestCookie.Value);
                }

                if (hotelRequestCookieModel != null && hotelRequestCookieModel.HotelRequestDetail != null)
                {
                    hotelRequestCookieModel.HotelRequestDetail.roomCode = roomCode;
                }

                hotelModel.HotelRequestDetail = hotelRequestCookieModel.HotelRequestDetail;
                var cookieId = SaveHotelQuery(hotelRequestCookieModel, hotelRequestCookieId);
                hotelModel.hotelRequestCookieId = cookieId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }

            hotelModel.ProvisionalBookingDetail = new ProvisionalBooking();
            hotelModel.ProvisionalBookingDetail.GuestInformation = new GuestInformation();
            hotelModel.ProvisionalBookingDetail.GuestInformation.Address = new Address();

            /***comment call get wallet balance here**/
            hotelModel.WalletResponseDetail = new WalletResponse();

            hotelModel.AssignTitle();
            return View("bookHotel", hotelModel);
        }

        public async Task<ActionResult> SubmitBookingDetail(HotelViewModel bookingModel)
        {
            try
            {
                if (bookingModel == null || bookingModel.ProvisionalBookingDetail == null)
                {
                    return null;
                }

                /****Set Hotel Info***/
                bookingModel.ProvisionalBookingDetail.Hotelinfo = new Hotelinfo();

                var hotelRequestCookie = Request.Cookies[bookingModel.hotelRequestCookieId];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var hotelRequestDetail = new HotelRequest();
                if (hotelRequestCookie != null)
                {
                    hotelRequestDetail = serializer.Deserialize<HotelRequest>(hotelRequestCookie.Value);
                }

                if (hotelRequestDetail == null)
                {
                    return Json("Hotel request detail not found.");
                }

                Rate rateRoom = MakeProvisionalBookingRequest(bookingModel, hotelRequestDetail, null);

                /**nee bookhotel other work here ***/

                this.hotelManager = new HotelManager();
                var response = await this.hotelManager.ProvisionBooking(bookingModel.ProvisionalBookingDetail);

                if (response == null)
                {
                    return Json(string.Empty);
                }
                else
                {
                    HotelDescriptionRequest bookingRequest = MakeConfirmHotelBookingRequest(bookingModel, hotelRequestDetail, rateRoom, response);

                    var bookingResponse = await this.hotelManager.BookingConfirmation(bookingRequest);

                    if (bookingResponse == null || bookingResponse.Bookingresponse == null)
                    {
                        return Json(string.Empty);
                    }
                    else
                    {
                        if (bookingResponse.Bookingresponse.Bookingstatus == "E")
                        {
                            return Json(bookingResponse.Bookingresponse.Bookingremarks);
                        }
                        else
                        {
                            /***Payment****/

                            PayUController cntrl = new PayUController();
                            PayuRequest request = new PayuRequest();
                            request.FirstName = bookingModel.ProvisionalBookingDetail.GuestInformation.FirstName;
                            request.TransactionAmount = 1;
                            request.Email = bookingModel.ProvisionalBookingDetail.GuestInformation.Email;
                            request.Phone = bookingModel.ProvisionalBookingDetail.GuestInformation.PhoneNumber.Number;
                            request.ProductInfo = "Booking Hotel " + ShineYatraSession.HotelRequest.hotelid + ": " + ShineYatraSession.SelectedHotel.Hoteldetail.Hotelname + " For Name : " + request.FirstName;
                            request.surl = "http://" + Request.Url.Authority + "/PayU/Return";
                            request.furl = "http://" + Request.Url.Authority + "/PayU/Return";

                            cntrl.Payment(request);

                            /***Payment****/

                            //   return Json(bookingResponse.Bookingresponse.Bookingremarks + " Booking Confirm With TXD : " + bookingResponse.Bookingresponse.Bookingref);

                            return Json(string.Empty);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }

            return View("bookHotel", hotelModel);
        }

        private static HotelDescriptionRequest MakeConfirmHotelBookingRequest(HotelViewModel bookingModel, HotelRequest hotelRequestDetail, Rate rateRoom, ArzHotelProvResp response)
        {
            var bookingRequest = new HotelDescriptionRequest();
            bookingRequest.hotelid = hotelRequestDetail.hotelid;
            bookingRequest.webService = hotelRequestDetail.webService;
            bookingRequest.RoomTypeCode = rateRoom.RoomTypeCode;
            bookingRequest.RatePlanType = rateRoom.RatePlanCode;
            bookingRequest.CheckInDate = hotelRequestDetail.Start;
            bookingRequest.CheckOutDate = hotelRequestDetail.End;
            bookingRequest.city = hotelRequestDetail.HotelCityName;
            bookingRequest.Allocid = response.Allocresult.Allocid;
            bookingRequest.Fromallocation = response.Allocresult.Allocavail;
            bookingRequest.Roomtype = rateRoom.Roomtype;
            bookingRequest.WsKey = rateRoom.Ratebands.WsKey;
            bookingRequest.Roombasis = rateRoom.Roombasis.Replace("&", "And");
            bookingRequest.RoomStayCandidate = new RoomStayCandidate();

            bookingRequest.RoomStayCandidate.GuestDetails = new List<GuestDetails>();
            bookingRequest.RoomStayCandidate.GuestDetails = (hotelRequestDetail.RoomStayCandidateDetail.GuestDetails);

            bookingRequest.GuestInformation = bookingModel.ProvisionalBookingDetail.GuestInformation;
            return bookingRequest;
        }

        private static Rate MakeProvisionalBookingRequest(HotelViewModel bookingModel, HotelRequest hotelRequestDetail, Ratedetail rateDetail)
        {
            bookingModel.ProvisionalBookingDetail.Hotelinfo.Hotelid = hotelRequestDetail.hotelid;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.WebService = hotelRequestDetail.webService;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.Fromdate = hotelRequestDetail.Start;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.Todate = hotelRequestDetail.End;

            var rateRoom = new Rate();
            if (rateDetail != null)
            {
                rateRoom = rateDetail.Rate.FirstOrDefault(r => r.RoomTypeCode == hotelRequestDetail.roomCode);
            }

            bookingModel.HotelRequestDetail.NightCount = CalculateDays(hotelRequestDetail.Start, hotelRequestDetail.End);

            bookingModel.ProvisionalBookingDetail.TotalFare = hotelRequestDetail.NightCount * hotelRequestDetail.RoomCount * Convert.ToInt32(rateRoom.Ratebands.RoomTotal);
            bookingModel.ProvisionalBookingDetail.Hotelinfo.RoomTypeCode = rateRoom.RoomTypeCode;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.Roomtype = rateRoom.Roomtype;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.RatePlanType = rateRoom.RatePlanCode;

            bookingModel.ProvisionalBookingDetail.RoomStayCandidate = new RoomStayCandidate();

            /****Set Room stay candidate Info***/
            bookingModel.ProvisionalBookingDetail.RoomStayCandidate.GuestDetails = new List<GuestDetails>();
            bookingModel.ProvisionalBookingDetail.RoomStayCandidate.GuestDetails = (hotelRequestDetail.RoomStayCandidateDetail.GuestDetails);

            /****Set Rateband nfo***/
            bookingModel.ProvisionalBookingDetail.Ratebands = rateRoom.Ratebands;
            bookingModel.ProvisionalBookingDetail.ResidentOfIndia = "true";
            return rateRoom;
        }

        /// <summary>
        /// method to 
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public ActionResult AssessorNameSearch(string Prefix)
        {
            _flightManager = new FlightManager();
            try
            {
                IList<KeyValuePair> cityName = ShineYatraSession.HotelCityList;
                cityName = cityName.Where(o => o.Id.ToLower().Trim().Contains(Prefix.ToLower().Trim())).ToList();

                return Json(cityName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(null);
        }

        public string SaveHotelQuery(HotelViewModel requestDetail, string cookieId)
        {
            var GuidString = Guid.NewGuid().ToString();
            string requestDetailJson = new JavaScriptSerializer().Serialize(requestDetail);
            if (string.IsNullOrEmpty(cookieId))
            {
                var cookie = new HttpCookie(GuidString, requestDetailJson)
                {
                    Expires = DateTime.Now.AddYears(1)
                };

                HttpContext.Response.Cookies.Add(cookie);
                var cookie1 = new HttpCookie((GuidString + "RateDetail"), string.Empty)
                {
                    Expires = DateTime.Now.AddYears(1)
                };

                HttpContext.Response.Cookies.Add(cookie1);

            }
            else
            {
                GuidString = cookieId;
                HttpContext.Response.Cookies[cookieId].Value = requestDetailJson;
                //if (string.IsNullOrEmpty(HttpContext.Response.Cookies[(cookieId + "RateDetail")].Value))
                //{
                //    HttpContext.Response.Cookies[(cookieId + "RateDetail")].Value = string.Empty;
                //}
            }

            return GuidString;

        }

        public void SaveRateDetail(Ratedetail rateDetail, string cookieId)
        {
            string requestDetailJson = new JavaScriptSerializer().Serialize(rateDetail);
            Session[cookieId] = requestDetailJson;
            //if (string.IsNullOrEmpty(HttpContext.Response.Cookies[(cookieId)].Value))
            //{
            //    var cookie = new HttpCookie(cookieId, requestDetailJson)
            //    {
            //        Expires = DateTime.Now.AddYears(1)
            //    };

            //    HttpContext.Response.Cookies.Add(cookie);
            //}
            //else
            //{
            //    HttpContext.Response.Cookies[cookieId].Value = requestDetailJson;
            //}
        }

        /// <summary>
        /// Method save  to book flight ticket
        /// </summary>in
        /// <param name="bookingModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BookResponse(HotelViewModel bookingModel)
        {
            bookingModel.Error = string.Empty;
            var responseMessage = string.Empty;
            hotelManager = new HotelManager();
            var txnId = string.Empty;
            var info = string.Empty;
            var error = string.Empty;
            var walletBalance = 0.0;
            var bookResponse = new ArzHotelBookingResp();
            bookResponse.Bookingresponse = new HotelBookingresponse();
            try
            {
                if (bookingModel == null || bookingModel.ProvisionalBookingDetail == null)
                {
                    error = "In sufficient detail for Booking.";
                    goto ExecuteError;
                }

                if (string.IsNullOrEmpty(error))
                {
                    var userData = User.Identity.Name.Split('|');
                    var request = bookingModel.ProvisionalBookingDetail;
                    request.Creditcardno = "4111111111111111";
                    var isPaymentGatewayactive = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPaymentGatewayactive"]);

                    /****Set Hotel Info***/
                    bookingModel.ProvisionalBookingDetail.Hotelinfo = new Hotelinfo();

                    var hotelRequestCookie = Request.Cookies[bookingModel.hotelRequestCookieId];
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var hotelRequestCookieModel = new HotelViewModel();
                    var rateDetail = new Ratedetail();
                    if (hotelRequestCookie != null)
                    {
                        hotelRequestCookieModel = serializer.Deserialize<HotelViewModel>(hotelRequestCookie.Value);
                    }

                    //hotelRequestCookie = Request.Cookies[bookingModel.hotelRequestCookieId + "Ratedetail"];
                    var rateDetailSession = Session[bookingModel.hotelRequestCookieId + "Ratedetail"];
                    if (rateDetailSession != null)
                    {
                        rateDetail = serializer.Deserialize<Ratedetail>(rateDetailSession.ToString());
                    }

                    if (hotelRequestCookieModel == null || rateDetail == null)
                    {
                        error = "Hotel request detail not found.";
                        goto ExecuteError;
                    }

                    bookingModel.SelectedHotel = hotelRequestCookieModel.SelectedHotel;
                    Rate rateRoom = MakeProvisionalBookingRequest(bookingModel, hotelRequestCookieModel.HotelRequestDetail, rateDetail);

                    /**need bookhotel other work here ***/

                    /***comment* start*/
                    /**Need a call from bharat sir to save below request**/
                    //uncooment it info = SaveHotelBookingDetail(bookingModel.HotelDescRequest);
                    info = bookingModel.hotelRequestCookieId;

                    /***comment* end*/
                    try
                    {
                        var balrequest = new WalletRequest
                        {
                            action = "GET_WALLET_BALANCE",
                            domain_name = ConfigurationManager.AppSettings["DomainName"],
                            member_id = userData[1],
                            company_id = userData[2]
                        };

                        _userManager = new UserManager();

                        var balResponse = await _userManager.GET_WALLET_BALANCE(balrequest);
                        if (balResponse != null)
                        {
                            walletBalance = balResponse.wallet_balance;
                        }
                    }
                    catch (Exception exx)
                    {
                        error = exx.Message;
                        goto ExecuteError;
                    }
                    if (isPaymentGatewayactive && (bookingModel.HotelRequestDetail.PaymentMode == "bank" || walletBalance < request.TotalFare))
                    {
                        var g = Guid.NewGuid();
                        var guidString = Convert.ToBase64String(g.ToByteArray());
                        guidString = guidString.Replace("=", "");
                        guidString = guidString.Replace("+", "");
                        var paymentDetail = new CompanyFund
                        {
                            action = "INSERT_PG_REQUEST_FOR_SERVICE",
                            member_id = userData[1],
                            domain_name = ConfigurationManager.AppSettings["DomainName"],
                            request_token = guidString,
                            txn_type = "PG_REQUEST",
                            deposit_mode = "PG",
                            remarks = "Hotel booking payment by payment gateway",
                            amount = bookingModel.HotelRequestDetail.PaymentMode == "bank" ? request.TotalFare : request.TotalFare - walletBalance
                        };

                        _pgManager = new PgManager();
                        var balanceTxnId = await _pgManager.SavePaymntGatewayTransactions(paymentDetail);
                        if (!balanceTxnId.ToLower().Contains("failed"))
                        {
                            var cntrl = new PayUController();
                            var payrequest = new PayuRequest
                            {
                                FirstName = request.GuestInformation.FirstName,
                                TransactionAmount = bookingModel.HotelRequestDetail.PaymentMode == "bank" ? request.TotalFare : request.TotalFare - walletBalance,
                                Email = request.GuestInformation.Email,
                                Phone = request.GuestInformation.PhoneNumber.Number,
                                udf1 = info,
                                udf2 = Convert.ToString(balanceTxnId),
                                udf3 = bookingModel.hotelRequestCookieId,
                                memberId = userData[1],
                                ProductInfo = "Booking Hotel " + bookingModel.SelectedHotel.Hoteldetail.Hotelname + ": " + " For Name : " + request.GuestInformation.FirstName,
                                surl = "http://" + Request.Url.Authority + "/Hotel/Return",
                                furl = "http://" + Request.Url.Authority + "/Hotel/Return"
                            };
                            cntrl.Payment(payrequest);
                        }
                        else
                        {
                            error = (balanceTxnId);
                            goto ExecuteError;
                        }
                    }
                    else
                    {
                        if (request.TotalFare <= walletBalance)
                        {
                            var myCookie = Request.Cookies[info];
                            serializer = new JavaScriptSerializer();
                            // if (myCookie != null)
                            {
                                //  var ticketDetail = serializer.Deserialize<HotelDescriptionRequest>(myCookie.Value);
                                /***comment* start*/
                                /*need call from bharat sir to save hotel request*/
                                ////uncomment this var response = await _flightManager.InsertServiceBookingRequest(null);
                                /**end**/
                                ////uncomment this var insertServiceResonse = response.FirstOrDefault();
                                // uncomment this if (insertServiceResonse != null)
                                if (true)
                                {
                                    //uncomment this txnId = Convert.ToString(insertServiceResonse.txn_id);
                                    txnId = Guid.NewGuid().ToString().Substring(0, 5);
                                    this.hotelManager = new HotelManager();
                                    var response1 = await this.hotelManager.ProvisionBooking(bookingModel.ProvisionalBookingDetail);

                                    if (response1 == null)
                                    {
                                        error = ("Something went wrong while Provisional booking request.");
                                    }
                                    else
                                    {
                                        HotelDescriptionRequest bookingRequest = MakeConfirmHotelBookingRequest(bookingModel, hotelRequestCookieModel.HotelRequestDetail, rateRoom, response1);

                                        var bookingResponse = await this.hotelManager.BookingConfirmation(bookingRequest);

                                        if (bookingResponse == null || bookingResponse.Bookingresponse == null)
                                        {
                                            error = ("Something went wrong while booking confirmation request.");
                                        }
                                        else
                                        {
                                            if (bookingResponse.Bookingresponse.Bookingstatus == "E")
                                            {
                                                error = (bookingResponse.Bookingresponse.Bookingremarks);
                                                goto ExecuteError;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bookResponse.Bookingresponse.Bookingstatus = "Failed";
                                }
                            }
                        }
                        else
                        {
                            bookResponse.Bookingresponse.Bookingstatus = "Failed";
                            error = ("Insufficient amount in wallet.");
                        }

                        bookingModel.txnId = txnId;
                        bookingModel.HotelBookingResponse = bookResponse;
                        //TempData["BookingResponse"] = bookResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                error = (ex.Message);
            }

            ExecuteError:
            if (string.IsNullOrEmpty(error))
            {
                await BookingStatus(bookingModel);
            }
            else
            {
                hotelModel.Error = error;
                return View("bookHotel", bookingModel);
                //return RedirectToAction("BookingStatus", "Hotel", new { txnId, info });
            }

            return View("bookHotel", bookingModel);
        }

        [HttpPost]
        public async Task<ActionResult> Return(FormCollection form)
        {
            var txnId = string.Empty;
            var info = "";
            var bookResponse = new Bookingresponse();
            var request = new Request();
            var flightManager = new HotelManager();
            var serializer = new JavaScriptSerializer();
            try
            {
                var cookieId = form["udf1"];
                info = cookieId;
                var balanceTxId = form["udf2"];
                //getbooking 
                var getticketDetailList = new HotelDescriptionRequest();

                var myCookie = Request.Cookies[cookieId];

                // Read the cookie information and display it.
                if (myCookie != null)
                {
                    getticketDetailList = serializer.Deserialize<HotelDescriptionRequest>(myCookie.Value);
                }

                var hotelCookieModel = new HotelViewModel();
                var cookieValue = Request.Cookies[form["udf3"]];

                // Read the cookie information and display it.
                if (cookieValue != null)
                {
                    hotelCookieModel = serializer.Deserialize<HotelViewModel>(cookieValue.Value);
                }
                var rateDetail = new Ratedetail();
                cookieValue = Request.Cookies[form["udf3"] + "Ratedetail"];
                if (cookieValue != null)
                {
                    rateDetail = serializer.Deserialize<Ratedetail>(cookieValue.Value);
                }

                var ticketDetail = getticketDetailList;
                if (form["status"] == "success")
                {
                    //ticketDetail.my_info = "PG_REQUEST," + balanceTxId + "," + Convert.ToString(form["mode"]) + "," + Convert.ToString(form["txnid"]) + "," + Convert.ToString(form["bank_ref_num"]);
                    /*need call from bharat sir to save hotel request*/
                    var response = await _flightManager.InsertServiceBookingRequest(null);
                    var saveBookingResonse = response.FirstOrDefault();
                    Rate rateRoom = MakeProvisionalBookingRequest(hotelCookieModel, hotelCookieModel.HotelRequestDetail, rateDetail);
                    if (saveBookingResonse != null)
                    {
                        txnId = Convert.ToString(saveBookingResonse.txn_id);
                        ;
                        this.hotelManager = new HotelManager();
                        var response1 = await this.hotelManager.ProvisionBooking(hotelCookieModel.ProvisionalBookingDetail);

                        if (response1 == null)
                        {
                            return Json(string.Empty);
                        }
                        else
                        {
                            HotelDescriptionRequest bookingRequest = MakeConfirmHotelBookingRequest(hotelCookieModel, hotelCookieModel.HotelRequestDetail, rateRoom, response1);

                            var bookingResponse = await this.hotelManager.BookingConfirmation(bookingRequest);

                            if (bookingResponse == null || bookingResponse.Bookingresponse == null)
                            {
                                return Json(string.Empty);
                            }
                            else
                            {
                                if (bookingResponse.Bookingresponse.Bookingstatus == "E")
                                {
                                    return Json(bookingResponse.Bookingresponse.Bookingremarks);
                                }
                            }
                        }
                    }
                    else
                    {
                        bookResponse.Status = "Failed";
                    }
                }
                else
                {
                    bookResponse.Status = form["status"];
                }

                TempData["BookingResponse"] = bookResponse;
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:red'>" + ex.Message + "</span>");
            }
            return RedirectToAction("BookingStatus", "Hotel", new { txnId, info });
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="txnId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<ActionResult> BookingStatus(HotelViewModel bookingModel)
        {
            HotelViewModel eticket = new HotelViewModel();
            if (bookingModel == null || bookingModel.HotelBookingResponse == null)
            {
                eticket.Error = "Complate detail not found. Please try again later.";
            }
            else
            {
                _serviceManager = new ServiceManager();
                try
                {
                    //    var bookResponse = (ArzHotelBookingResp)TempData["BookingResponse"];

                    var userData = User.Identity.Name.Split('|');
                    var serializer = new JavaScriptSerializer();
                    if (bookingModel.HotelBookingResponse != null && bookingModel.HotelBookingResponse.Bookingresponse != null && bookingModel.HotelBookingResponse.Bookingresponse.Bookingstatus.ToLower() == "c")
                    {
                        eticket.Error = "Hotel Booked Successfully";
                        UPDATE_TRANSACTION_STATUS updatestatus = await _serviceManager.UpdateServiceBookingRequest(bookingModel.HotelBookingResponse.Bookingresponse.Bookingref, userData[1], bookingModel.HotelBookingResponse.Bookingresponse.BookingTrn, "COMPLETED");
                    }
                    else if (!string.IsNullOrEmpty(bookingModel.HotelBookingResponse.Bookingresponse.Bookingstatus))
                    {
                        eticket.Error = "Some Problem occured while booking, Please try again.";
                        UPDATE_TRANSACTION_STATUS updatestatus = await _serviceManager.UpdateServiceBookingRequest(bookingModel.txnId, userData[1], "", "Failed");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(bookingModel.txnId))
                        {
                            UPDATE_TRANSACTION_STATUS updatestatus = await _serviceManager.UpdateServiceBookingRequest(bookingModel.txnId, userData[1], "", "Failed");
                        }
                        ViewBag.status = "Some Problem occured while booking, Please try again.";
                    }

                    var myCookie = Request.Cookies[bookingModel.hotelRequestCookieId];
                    if (myCookie != null)
                    {
                        eticket = serializer.Deserialize<HotelViewModel>(myCookie.Value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }

            //return View(this.hotelModel);
            return View("hotelBookingStatus", eticket);
        }

        public string SaveHotelBookingDetail(HotelDescriptionRequest requestDetail)
        {
            var GuidString = Guid.NewGuid().ToString();
            string requestDetailJson = new JavaScriptSerializer().Serialize(requestDetail);
            var cookie = new HttpCookie(GuidString, requestDetailJson)
            {
                Expires = DateTime.Now.AddYears(1)
            };

            HttpContext.Response.Cookies.Add(cookie);

            return GuidString;

        }

        public static int CalculateDays(string date1, string date2)
        {
            try
            {
                if (string.IsNullOrEmpty(date1) || string.IsNullOrEmpty(date2))
                {
                    return 0;
                }

                DateTime d1 = DateTime.ParseExact(date1, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                DateTime d2 = DateTime.ParseExact(date2, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                return (d2 - d1).Days;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}