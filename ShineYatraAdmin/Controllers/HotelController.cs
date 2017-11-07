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
    using Entity.HotelDetail;
    using System.Globalization;

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
                    hotelDetailListViewModel.HotelList = new List<Entity.Hotel>();
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
                    request.HotelName = null;
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
                        this.SaveSelectedHotelDetail(response, cookieId);
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

        public async Task<ActionResult> BookHotelView(string roomCode, string hotelRequestCookieId)
        {
            try
            {
                if (string.IsNullOrEmpty(roomCode) || string.IsNullOrEmpty(hotelRequestCookieId))
                {
                    return null;
                }

                hotelModel.ProvisionalBookingDetail = new ProvisionalBooking();
                hotelModel.ProvisionalBookingDetail.GuestInformation = new GuestInformation();
                hotelModel.ProvisionalBookingDetail.GuestInformation.Address = new Address();

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

                /***Rate detail***/

                var hotelRequestJson = Session[hotelRequestCookieId + "RateDetail"].ToString();
                serializer = new JavaScriptSerializer();

                if (hotelRequestCookie != null)
                {
                    var rateDetail = serializer.Deserialize<Ratedetail>(hotelRequestJson);
                    if (rateDetail != null)
                    {
                        hotelModel.SelectedRate = rateDetail.Rate.FirstOrDefault(r => r.RoomTypeCode == roomCode);
                        var fare = (hotelRequestCookieModel.HotelRequestDetail.NightCount * hotelRequestCookieModel.HotelRequestDetail.RoomCount * (Convert.ToInt32(hotelModel.SelectedRate.Ratebands.ExtGuestTotal) + Convert.ToInt32(hotelModel.SelectedRate.Ratebands.RoomTotal) + Convert.ToInt32(hotelModel.SelectedRate.Ratebands.ServicetaxTotal))) - Convert.ToDouble(hotelModel.SelectedRate.Ratebands.Discount);
                        hotelModel.ProvisionalBookingDetail.TotalFare = Convert.ToInt32(fare);
                        hotelModel.RoomTotalPerNight = (Convert.ToInt32(hotelModel.SelectedRate.Ratebands.ExtGuestTotal) + Convert.ToInt32(hotelModel.SelectedRate.Ratebands.RoomTotal) + Convert.ToInt32(hotelModel.SelectedRate.Ratebands.ServicetaxTotal)).ToString();
                    }
                }

                hotelModel.SelectedHotel = serializer.Deserialize<Entity.HotelDetail.Hotel>(Session[hotelRequestCookieId + "Hotel"].ToString());
                hotelModel.HotelRequestDetail = hotelRequestCookieModel.HotelRequestDetail;
                var cookieId = SaveHotelQuery(hotelRequestCookieModel, hotelRequestCookieId);
                hotelModel.hotelRequestCookieId = cookieId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }

            /***comment call get wallet balance here**/
            var userData = User.Identity.Name.Split('|');
            var balrequest = new WalletRequest
            {
                action = "GET_WALLET_BALANCE",
                domain_name = ConfigurationManager.AppSettings["DomainName"],
                member_id = userData[1],
                company_id = userData[2]
            };

            _userManager = new UserManager();
            hotelModel.WalletResponseDetail = new WalletResponse();

            var balResponse = await _userManager.GET_WALLET_BALANCE(balrequest);
            var walletBalance = 0f;
            if (balResponse != null)
            {
                walletBalance = balResponse.wallet_balance;
                Session["WalletBalance"] = walletBalance;
            }

            hotelModel.WalletResponseDetail.wallet_balance = (float)walletBalance;

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

                bookingModel.HotelRequestDetail = hotelRequestDetail;
                Rate rateRoom = MakeProvisionalBookingRequest(bookingModel, null);

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

        private static Rate MakeProvisionalBookingRequest(HotelViewModel bookingModel, Ratedetail rateDetail)
        {
            bookingModel.ProvisionalBookingDetail.Hotelinfo.Hotelid = bookingModel.HotelRequestDetail.hotelid;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.WebService = bookingModel.HotelRequestDetail.webService;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.Fromdate = bookingModel.HotelRequestDetail.Start;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.Todate = bookingModel.HotelRequestDetail.End;

            var rateRoom = new Rate();
            if (rateDetail != null)
            {
                rateRoom = rateDetail.Rate.FirstOrDefault(r => r.RoomTypeCode == bookingModel.HotelRequestDetail.roomCode);
                bookingModel.SelectedRate = rateRoom;
            }

            bookingModel.HotelRequestDetail.NightCount = CalculateDays(bookingModel.HotelRequestDetail.Start, bookingModel.HotelRequestDetail.End);

            var fare = (bookingModel.HotelRequestDetail.NightCount * bookingModel.HotelRequestDetail.RoomCount * (Convert.ToInt32(rateRoom.Ratebands.RoomTotal) + Convert.ToInt32(rateRoom.Ratebands.ExtGuestTotal) + Convert.ToInt32(rateRoom.Ratebands.ServicetaxTotal))) - Convert.ToDouble(rateRoom.Ratebands.Discount);
            bookingModel.ProvisionalBookingDetail.TotalFare = Convert.ToInt32(fare);
            bookingModel.ProvisionalBookingDetail.Hotelinfo.RoomTypeCode = rateRoom.RoomTypeCode;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.Roomtype = rateRoom.Roomtype;
            bookingModel.ProvisionalBookingDetail.Hotelinfo.RatePlanType = rateRoom.RatePlanCode;

            bookingModel.ProvisionalBookingDetail.RoomStayCandidate = new RoomStayCandidate();

            /****Set Room stay candidate Info***/
            bookingModel.ProvisionalBookingDetail.RoomStayCandidate.GuestDetails = new List<GuestDetails>();
            bookingModel.ProvisionalBookingDetail.RoomStayCandidate.GuestDetails = (bookingModel.HotelRequestDetail.RoomStayCandidateDetail.GuestDetails);

            bookingModel.ProvisionalBookingDetail.GuestInformation.Address.Country = "India";
            bookingModel.ProvisionalBookingDetail.GuestInformation.PhoneNumber.AreaCode = "1";
            bookingModel.ProvisionalBookingDetail.GuestInformation.PhoneNumber.CountryCode = "1";
            bookingModel.ProvisionalBookingDetail.GuestInformation.PhoneNumber.Extension = "1";

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
            var GuidString = Guid.NewGuid().ToString().Substring(0, 8);
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
        }

        public void SaveSelectedHotelDetail(Entity.HotelDetail.Hotel detail, string cookieId)
        {
            string requestDetailJson = new JavaScriptSerializer().Serialize(detail);
            Session[cookieId + "Hotel"] = requestDetailJson;
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
            var isWallet = true;
            var error = string.Empty;
            var walletBalance = 0.0;
            var bookResponse = new ArzHotelBookingResp();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
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
                    var isPaymentGatewayactive = Convert.ToString(Session["web_pg_api_enabled"]).ToUpper() == "Y" && userData[6] != "3";

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
                        bookingModel.WalletResponseDetail = new WalletResponse();

                        var balResponse = await _userManager.GET_WALLET_BALANCE(balrequest);
                        if (balResponse != null)
                        {
                            walletBalance = balResponse.wallet_balance;
                            Session["WalletBalance"] = walletBalance;
                        }

                        bookingModel.WalletResponseDetail.wallet_balance = (float)walletBalance;
                    }
                    catch (Exception exx)
                    {
                        error = exx.Message;
                        goto ExecuteError;
                    }

                    /****Set Hotel Info***/
                    bookingModel.ProvisionalBookingDetail.Hotelinfo = new Hotelinfo();

                    var hotelRequestCookie = Request.Cookies[bookingModel.hotelRequestCookieId];
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

                    hotelRequestCookieModel.HotelRequestDetail.PaymentMode = bookingModel.HotelRequestDetail.PaymentMode;
                    hotelRequestCookieModel.HotelRequestDetail.PartialPaymentWithWallet = bookingModel.HotelRequestDetail.PartialPaymentWithWallet;
                    bookingModel.SelectedHotel = hotelRequestCookieModel.SelectedHotel;
                    bookingModel.HotelRequestDetail = hotelRequestCookieModel.HotelRequestDetail;

                    Rate rateRoom = MakeProvisionalBookingRequest(bookingModel, rateDetail);

                    /**need bookhotel other work here ***/

                    /***comment* start*/
                    /**Need a call from bharat sir to save below request**/
                    //info = SaveHotelServiceBookingRequest(bookingModel);
                    //info = bookingModel.hotelRequestCookieId;

                    /***comment* end*/

                    if (isPaymentGatewayactive && (bookingModel.HotelRequestDetail.PaymentMode == "bank"))
                    {
                        double pgamount = 0;
                        bool pgflag = false;

                        if (bookingModel.HotelRequestDetail.PartialPaymentWithWallet)
                        {
                            if (walletBalance < request.TotalFare)
                            {
                                pgamount = request.TotalFare - walletBalance;
                                pgflag = true;
                                isWallet = false;

                            }
                            else
                            {
                                pgamount = 0;
                                pgflag = false;
                            }
                        }
                        else
                        {
                            isWallet = false;
                            pgamount = request.TotalFare;
                            pgflag = true;
                        }

                        if (pgflag)
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
                                amount = bookingModel.HotelRequestDetail.PartialPaymentWithWallet ? request.TotalFare - walletBalance : request.TotalFare
                            };

                            _pgManager = new PgManager();
                            var balanceTxnId = await _pgManager.SavePaymntGatewayTransactions(paymentDetail);
                            if (!balanceTxnId.ToLower().Contains("failed"))
                            {
                                string bookingModelJson = new JavaScriptSerializer().Serialize(bookingModel);
                                var key = Guid.NewGuid().ToString().Substring(0, 5);
                                Session[key] = bookingModelJson;

                                var cntrl = new PayUController();
                                var payrequest = new PayuRequest
                                {
                                    FirstName = request.GuestInformation.FirstName,
                                    TransactionAmount = pgamount,
                                    Email = request.GuestInformation.Email,
                                    Phone = request.GuestInformation.PhoneNumber.Number,
                                    udf1 = bookingModel.hotelRequestCookieId + "Ratedetail",
                                    udf2 = Convert.ToString(balanceTxnId),
                                    udf3 = key,
                                    memberId = userData[1],
                                    ProductInfo = "Booking Hotel " + bookingModel.HotelRequestDetail.HotelName + ": " + " For Name : " + request.GuestInformation.FirstName,
                                    surl = "http://" + Request.Url.Authority + "/Hotel/Return",
                                    furl = "http://" + Request.Url.Authority + "/Hotel/Return"
                                };

                                cntrl.Payment(payrequest);
                            }
                            else
                            {
                                TempData["ErrorCode"] = 5001;
                                error = (balanceTxnId);
                                goto ExecuteError;
                            }
                        }
                    }

                    if (isWallet)
                    {
                        if (request.TotalFare <= walletBalance)
                        {
                            //var myCookie = Request.Cookies[info];
                            //serializer = new JavaScriptSerializer();
                            // if (myCookie != null)
                            {
                                var ticketDetail = CreateServiceBookingRequest(bookingModel);
                                /***comment* start*/
                                /*need call from bharat sir to save hotel request*/
                                var response = await hotelManager.InsertServiceBookingRequest(ticketDetail);
                                /**end**/
                                var insertServiceResonse = response.FirstOrDefault();
                                if (insertServiceResonse != null)
                                {
                                    txnId = Convert.ToString(insertServiceResonse.txn_id);
                                    //txnId = Guid.NewGuid().ToString().Substring(0, 5);
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
                                            TempData["ErrorCode"] = 5002;
                                            error = ("Something went wrong while booking confirmation request.");
                                        }
                                        else
                                        {
                                            bookingModel.HotelBookingResponse = new ArzHotelBookingResp();
                                            bookingModel.HotelBookingResponse.Bookingresponse = new HotelBookingresponse();
                                            bookingModel.HotelBookingResponse.Bookingresponse.Bookingref = bookingResponse.Bookingresponse.Bookingref;
                                            bookingModel.HotelBookingResponse.Bookingresponse.Bookingremarks = bookingResponse.Bookingresponse.Bookingremarks;
                                            bookingModel.HotelBookingResponse.Bookingresponse.Bookingstatus = bookingResponse.Bookingresponse.Bookingstatus;
                                            bookingModel.HotelBookingResponse.Bookingresponse.BookingTrn = bookingResponse.Bookingresponse.BookingTrn;

                                            if (bookingResponse.Bookingresponse.Bookingstatus == "E")
                                            {
                                                TempData["ErrorCode"] = 5002;
                                                error = (bookingResponse.Bookingresponse.Bookingremarks);
                                                goto ExecuteError;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bookResponse.Bookingresponse.Bookingstatus = "Failed";
                                    TempData["ErrorCode"] = 5002;
                                }
                            }
                        }
                        else
                        {
                            bookResponse.Bookingresponse.Bookingstatus = "Failed";
                            error = ("Insufficient amount in wallet.");
                        }

                        bookingModel.txnId = txnId;

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
                return await HotelBookingStatus(bookingModel);
            }
            else
            {
                bookingModel.AssignTitle();
                bookingModel.Error = error;
                bookingModel.WalletResponseDetail = new WalletResponse();
                bookingModel.WalletResponseDetail.wallet_balance = (float)walletBalance;
                bookingModel.SelectedHotel = serializer.Deserialize<Entity.HotelDetail.Hotel>(Session[bookingModel.hotelRequestCookieId + "Hotel"].ToString());
                return View("bookHotel", bookingModel);
                //return RedirectToAction("BookingStatus", "Hotel", new { txnId, info });
            }

            //return View("bookHotel", bookingModel);
        }

        [HttpPost]
        public async Task<ActionResult> Return(FormCollection form)
        {
            var txnId = string.Empty;
            var info = "";
            var error = string.Empty;
            var bookResponse = new Bookingresponse();
            var request = new Request();
            var hotelManager = new HotelManager();
            var serializer = new JavaScriptSerializer();
            var hotelModelDetail = new HotelViewModel();
            var id = form["udf3"];
            try
            {
                var balanceTxId = form["udf2"];
                if (string.IsNullOrEmpty(id) && Session[id] != null)
                {
                    hotelModelDetail = serializer.Deserialize<HotelViewModel>(Session[id].ToString());
                }

                var rateDetail = new Ratedetail();
                var rateDetailJson = Session[form["udf1"]];
                if (rateDetailJson != null)
                {
                    rateDetail = serializer.Deserialize<Ratedetail>(rateDetailJson.ToString());
                }

                //var ticketDetail = getticketDetailList;
                if (form["status"] == "success")
                {
                    //ticketDetail.my_info = "PG_REQUEST," + balanceTxId + "," + Convert.ToString(form["mode"]) + "," + Convert.ToString(form["txnid"]) + "," + Convert.ToString(form["bank_ref_num"]);
                    /*need call from bharat sir to save hotel request*/
                    Rate rateRoom = MakeProvisionalBookingRequest(hotelModelDetail, rateDetail);

                    var ticketDetail = CreateServiceBookingRequest(hotelModelDetail);
                    ticketDetail.my_info = "PG_REQUEST," + balanceTxId + "," + Convert.ToString(form["mode"]) + "," + Convert.ToString(form["txnid"]) + "," + Convert.ToString(form["bank_ref_num"]);
                    var response = await hotelManager.InsertServiceBookingRequest(ticketDetail);
                    var saveBookingResonse = response.FirstOrDefault();

                    if (saveBookingResonse != null)
                    {
                        txnId = Convert.ToString(saveBookingResonse.txn_id);
                        hotelModelDetail.txnId = txnId;
                        this.hotelManager = new HotelManager();
                        var response1 = await this.hotelManager.ProvisionBooking(hotelModelDetail.ProvisionalBookingDetail);

                        if (response1 == null)
                        {
                            error = ("5002: Something went wrong while Provisional booking request.");
                        }
                        else
                        {
                            HotelDescriptionRequest bookingRequest = MakeConfirmHotelBookingRequest(hotelModelDetail, hotelModelDetail.HotelRequestDetail, rateRoom, response1);

                            var bookingResponse = await this.hotelManager.BookingConfirmation(bookingRequest);

                            if (bookingResponse == null || bookingResponse.Bookingresponse == null)
                            {
                                error = ("Something went wrong while booking confirmation request.");
                                goto ExecuteError;
                            }
                            else
                            {
                                hotelModelDetail.HotelBookingResponse = new ArzHotelBookingResp();
                                hotelModelDetail.HotelBookingResponse.Bookingresponse = new HotelBookingresponse();
                                hotelModelDetail.HotelBookingResponse.Bookingresponse.Bookingref = bookingResponse.Bookingresponse.Bookingref;
                                hotelModelDetail.HotelBookingResponse.Bookingresponse.Bookingremarks = bookingResponse.Bookingresponse.Bookingremarks;
                                hotelModelDetail.HotelBookingResponse.Bookingresponse.Bookingstatus = bookingResponse.Bookingresponse.Bookingstatus;
                                hotelModelDetail.HotelBookingResponse.Bookingresponse.BookingTrn = bookingResponse.Bookingresponse.BookingTrn;

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
                        bookResponse.Status = "Failed";
                        error = "Payment request Failed";
                    }
                }
                else
                {
                    bookResponse.Status = form["status"];
                    error = "Payment request Failed";
                }

                //TempData["BookingResponse"] = bookResponse;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            ExecuteError:
            if (string.IsNullOrEmpty(error))
            {
                return await HotelBookingStatus(hotelModelDetail);
            }
            else
            {
                hotelModelDetail.AssignTitle();
                hotelModelDetail.Error = error;
                hotelModelDetail.WalletResponseDetail = new WalletResponse();
                hotelModelDetail.SelectedHotel = serializer.Deserialize<Entity.HotelDetail.Hotel>(Session[id + "Hotel"].ToString());
                return View("bookHotel", hotelModelDetail);
                //return RedirectToAction("BookingStatus", "Hotel", new { txnId, info });
            }

            //return RedirectToAction("BookingStatus", "Hotel", new { txnId, info });
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="txnId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<ActionResult> HotelBookingStatus(HotelViewModel bookingModel)
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
                        UPDATE_TRANSACTION_STATUS updatestatus = await _serviceManager.UpdateServiceBookingRequest(bookingModel.txnId, userData[1], bookingModel.HotelBookingResponse.Bookingresponse.BookingTrn, "COMPLETED");
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

                        eticket.Error = "Some Problem occured while booking, Please try again.";
                    }

                    var myCookie = Request.Cookies[bookingModel.hotelRequestCookieId];
                    if (myCookie != null)
                    {
                        eticket = serializer.Deserialize<HotelViewModel>(myCookie.Value);
                    }
                }
                catch (Exception ex)
                {
                    eticket.Error = (ex.Message);
                }
            }

            eticket.HotelRequestDetail.PaymentMode = bookingModel.HotelRequestDetail.PaymentMode;

            bookingModel.HotelRequestDetail = eticket.HotelRequestDetail;
            //return View(this.hotelModel);
            return View("HotelBookingStatus", bookingModel);
        }

        public string SaveHotelBookingDetail(HotelDescriptionRequest requestDetail)
        {
            var GuidString = Guid.NewGuid().ToString().Substring(0, 8);
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

        public HotelBoookingDetail CreateServiceBookingRequest(HotelViewModel model)
        {
            DateTime checkingDate = DateTime.ParseExact(model.HotelRequestDetail.Start, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
            DateTime checkoutDate = DateTime.ParseExact(model.HotelRequestDetail.End, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture);
            var userData = User.Identity.Name.Split('|');
            var ticketDetail = new HotelBoookingDetail();
            string key = Guid.NewGuid().ToString().Substring(0, 5);
            ticketDetail.action = "INSERT_SERVICE_HOTEL_REQUEST";
            ticketDetail.domain_name = ConfigurationManager.AppSettings["DomainName"];
            ticketDetail.request_token = key;
            ticketDetail.txn_type = "HOTELS";
            ticketDetail.category = "HOTEL";
            ticketDetail.member_id = userData[1];
            ticketDetail.amount = model.ProvisionalBookingDetail.TotalFare;
            ticketDetail.service_id = 2;
            ticketDetail.sub_service_id = 18;
            ticketDetail.destination = model.HotelRequestDetail.HotelCityName;
            ticketDetail.check_in = checkingDate.ToString("dd-MMM-yyyy");
            ticketDetail.check_out = checkoutDate.ToString("dd-MMM-yyyy");
            ticketDetail.adults = model.HotelRequestDetail.TotalAdultCount;
            ticketDetail.childern = model.HotelRequestDetail.TotalChildCount;
            ticketDetail.title = model.ProvisionalBookingDetail.GuestInformation.Title;
            ticketDetail.first_name = model.ProvisionalBookingDetail.GuestInformation.FirstName;
            ticketDetail.middle_name = model.ProvisionalBookingDetail.GuestInformation.MiddleName;
            ticketDetail.last_name = model.ProvisionalBookingDetail.GuestInformation.LastName;
            ticketDetail.mobile_number = model.ProvisionalBookingDetail.GuestInformation.PhoneNumber.Number;
            ticketDetail.email = model.ProvisionalBookingDetail.GuestInformation.Email;
            ticketDetail.address_line1 = model.ProvisionalBookingDetail.GuestInformation.Address.AddressLine;
            ticketDetail.city = model.ProvisionalBookingDetail.GuestInformation.Address.City;
            ticketDetail.zipcode = model.ProvisionalBookingDetail.GuestInformation.Address.ZipCode;
            ticketDetail.state = model.ProvisionalBookingDetail.GuestInformation.Address.State;
            ticketDetail.country = model.ProvisionalBookingDetail.GuestInformation.Address.Country;
            ticketDetail.my_info = string.Empty;
            ticketDetail.remarks = "Hotel Booking";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            model.SelectedHotel = serializer.Deserialize<Entity.HotelDetail.Hotel>(Session[model.hotelRequestCookieId + "Hotel"].ToString());

            //var hotelRequestJson = Session[hotelModel.hotelRequestCookieId + "RateDetail"].ToString();
            //serializer = new JavaScriptSerializer();

            //if (hotelRequestJson != null)
            //{
            //    var rateDetail = serializer.Deserialize<Ratedetail>(hotelRequestJson);
            //    if (rateDetail != null)
            //    {
            //        hotelModel.SelectedRate = rateDetail.Rate.FirstOrDefault(r => r.RoomTypeCode == hotelModel.HotelRequestDetail.roomCode);
            //    }
            //}

            ticketDetail.room_type = model.SelectedRate.Roomtype;
            ticketDetail.hotel_name = model.SelectedHotel.Hoteldetail.Hotelname;
            ticketDetail.hotel_city = model.SelectedHotel.Hoteldetail.City;
            ticketDetail.hotel_address = model.SelectedHotel.Hoteldetail.Contactinfo.Address;
            ticketDetail.hotel_amenities = model.SelectedRate.Roombasis;
            ticketDetail.check_in_time = model.SelectedHotel.Hoteldetail.Bookinginfo.Checkintime;
            ticketDetail.check_out_time = model.SelectedHotel.Hoteldetail.Bookinginfo.Checkintime;

            if (model.HotelRequestDetail.PaymentMode.ToLower().Trim() == "bank")
            {
                ticketDetail.pg_amount = model.ProvisionalBookingDetail.TotalFare;
            }
            else
            {
                var isPaymentGatewayactive = Convert.ToString(Session["web_pg_api_enabled"]).ToUpper() == "Y" && userData[6] != "3";
                if (isPaymentGatewayactive && model.WalletResponseDetail.wallet_balance < model.ProvisionalBookingDetail.TotalFare)
                {
                    ticketDetail.pg_amount = model.ProvisionalBookingDetail.TotalFare - model.WalletResponseDetail.wallet_balance;
                }
                else
                {
                    ticketDetail.pg_amount = 0;
                }
            }
            ticketDetail.other_amount = 0;
            ticketDetail.total_paid_amount = 0;

            //string ticketDetailJson = new JavaScriptSerializer().Serialize(ticketDetail);
            //var cookie = new HttpCookie(key, ticketDetailJson)
            //{
            //    Expires = DateTime.Now.AddYears(1)
            //};

            //HttpContext.Response.Cookies.Add(cookie);

            return ticketDetail;
        }

        public async Task<ActionResult> GetHotelTransactionList(HotelViewModel model)
        {
            if (model == null || model.FilterDetail == null)
            {
                model = new HotelViewModel();
                model.FilterDetail = new Entity.Filter();
            }

            model.FilterDetail.AssignSelectTypeList();
            hotelManager = new HotelManager();
            var userData = User.Identity.Name.Split('|');
            var bookingList = await hotelManager.GetHotelsTransactionSummaryList(userData[1]);
            if (bookingList != null && model.FilterDetail != null)
            {
                if (!string.IsNullOrEmpty(model.FilterDetail.ResultType))
                {
                    if (model.FilterDetail.ResultType == "Success")
                    {
                        bookingList = bookingList.Where(book => book.status != null && (book.status.ToUpper().Contains("COMPLETE") || book.status.ToUpper().Contains("SUCCESS"))).ToList();
                    }
                    else if (model.FilterDetail.ResultType == "Fail")
                    {
                        bookingList = bookingList.Where(book => book.status != null && (book.status.ToUpper().Contains("FAIL") || book.status.ToUpper().Contains("PENDING"))).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(model.FilterDetail.SelectType))
                {
                    var selectedTypeValueInt = 0;

                    if (!string.IsNullOrEmpty(model.FilterDetail.SelectedTypeValue))
                    {
                        selectedTypeValueInt = Convert.ToInt32(model.FilterDetail.SelectedTypeValue);
                    }
                    if (model.FilterDetail.SelectType.Contains("Id"))
                    {
                        bookingList = bookingList.Where(book => book.txn_id == selectedTypeValueInt).ToList();
                    }

                    if (model.FilterDetail.SelectType.Contains("Member"))
                    {
                        bookingList = bookingList.Where(book => book.member_id == selectedTypeValueInt).ToList();
                    }

                    if (model.FilterDetail.SelectType.Contains("Mobile"))
                    {
                        bookingList = bookingList.Where(book => book.mobile != null && book.mobile == model.FilterDetail.SelectedTypeValue).ToList();
                    }

                    if (model.FilterDetail.SelectType.Contains("Email"))
                    {
                        bookingList = bookingList.Where(book => book.email != null && book.email == model.FilterDetail.SelectedTypeValue).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(model.FilterDetail.FromDate))
                {
                    model.FilterDetail.FromDate = model.FilterDetail.FromDate.Trim();

                    String format = "dd/MM/yyyy";
                    DateTime d1 = DateTime.ParseExact(model.FilterDetail.FromDate, format, CultureInfo.CurrentCulture);

                    //bookingList = bookingList.Where(book => book.status != null && (book.status.ToUpper().Contains("FAIL") || book.status.ToUpper().Contains("PENDING"))).ToList();
                    bookingList = bookingList.Where(p => p.check_in_date != null && DateTime.ParseExact(p.check_in_date.Trim(), format, CultureInfo.CurrentCulture) >= d1).ToList();
                }

                if (!string.IsNullOrEmpty(model.FilterDetail.ToDate))
                {
                    model.FilterDetail.ToDate = model.FilterDetail.ToDate.Trim();

                    String format = "dd/MM/yyyy";
                    DateTime d1 = DateTime.ParseExact(model.FilterDetail.ToDate, format, CultureInfo.CurrentCulture);

                    //bookingList = bookingList.Where(book => book.status != null && (book.status.ToUpper().Contains("FAIL") || book.status.ToUpper().Contains("PENDING"))).ToList();
                    bookingList = bookingList.Where(p => p.check_in_date != null && DateTime.ParseExact(p.check_out_date.Trim(), format, CultureInfo.CurrentCulture) <= d1).ToList();
                }
            }

            model.BookingList = bookingList;
            return View(model);
        }

    }
}