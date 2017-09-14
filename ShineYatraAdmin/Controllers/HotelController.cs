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

    #endregion namespace

    /// <summary>
    /// Holds Hotel operations
    /// </summary>
    public class HotelController : Controller
    {
        /// <summary>
        /// Hold flight model
        /// </summary>
        HotelViewModel hotelModel = new HotelViewModel();

        /// <summary>
        /// hold HotelManager
        /// </summary>
        HotelManager hotelManager;

        FlightManager flightManager;

        // GET: Hotel
        public async Task<ActionResult> Index()
        {
            if (ShineYatraSession.HotelCityList == null || ShineYatraSession.HotelCityList.Count > 0)
            {
                ShineYatraSession.HotelCityList = await flightManager.GetFlightCityList(false);
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
                    flightManager = new FlightManager();
                    ShineYatraSession.HotelCityList = await flightManager.GetFlightCityList(false);
                }
                
                hotelModel.SelectedMenu = menu;
                hotelModel.HotelRequestDetail = new HotelRequest();
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
                ShineYatraSession.HotelRequest = new HotelRequest();
                ShineYatraSession.HotelRequest.HotelCityName = hotelDetail.HotelRequestDetail.HotelCityName;
                ShineYatraSession.HotelRequest.Start = hotelDetail.HotelRequestDetail.Start;
                ShineYatraSession.HotelRequest.End = hotelDetail.HotelRequestDetail.End;

                hotelDetail.HotelRequestDetail.GuestDetails.Child = new Child();
                ShineYatraSession.HotelRequest.GuestDetails = hotelDetail.HotelRequestDetail.GuestDetails;

                this.hotelManager = new HotelManager();

                var response = await this.hotelManager.SearchHotel(hotelDetail.HotelRequestDetail);
                if (response != null && response.Searchresult != null)
                {
                    hotelDetailListViewModel.HotelList = response.Searchresult.Hotel;
                }
                else
                {
                    hotelDetailListViewModel = null;
                }
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

        public async Task<ActionResult> GetHotelDetailView(string hotelId, string hotelWebService)
        {
            try
            {
                if (string.IsNullOrEmpty(hotelId) || string.IsNullOrEmpty(hotelWebService))
                {
                    return null;
                }

                hotelManager = new HotelManager();
                var request = new HotelRequest();
                request.hotelid = hotelId;
                request.webService = hotelWebService;
                request.HotelCityName = ShineYatraSession.HotelRequest.HotelCityName;
                request.Start = ShineYatraSession.HotelRequest.Start;
                request.End = ShineYatraSession.HotelRequest.End;
                request.GuestDetails = ShineYatraSession.HotelRequest.GuestDetails;

                ShineYatraSession.HotelRequest.hotelid = hotelId;
                ShineYatraSession.HotelRequest.webService = hotelWebService;

                var response = await hotelManager.SearchHotelWithDetail(request);

                if (response == null || response.Hoteldetail == null)
                {
                    return Json(string.Empty);
                }
                else
                {
                    this.hotelModel = new HotelViewModel();
                    this.hotelModel.SelectedHotel = response;
                    ShineYatraSession.SelectedHotel = response;
                    return View("hotelDetailView", this.hotelModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }

            return View("Index", hotelModel);
        }

        public ActionResult BookHotelView(string roomCode)
        {
            try
            {
                if (string.IsNullOrEmpty(roomCode))
                {
                    return null;
                }

                ShineYatraSession.HotelRequest.roomCode = roomCode;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }

            hotelModel.ProvisionalBookingDetail = new ProvisionalBooking();
            hotelModel.ProvisionalBookingDetail.GuestInformation = new GuestInformation();
            hotelModel.ProvisionalBookingDetail.GuestInformation.Address = new Address();

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

                bookingModel.ProvisionalBookingDetail.Hotelinfo.Hotelid = ShineYatraSession.HotelRequest.hotelid;
                bookingModel.ProvisionalBookingDetail.Hotelinfo.WebService = ShineYatraSession.HotelRequest.webService;
                bookingModel.ProvisionalBookingDetail.Hotelinfo.Fromdate = ShineYatraSession.HotelRequest.Start;
                bookingModel.ProvisionalBookingDetail.Hotelinfo.Todate = ShineYatraSession.HotelRequest.End;

                var rateRoom = ShineYatraSession.SelectedHotel.Ratedetail.Rate.FirstOrDefault(r => r.RoomTypeCode == ShineYatraSession.HotelRequest.roomCode);

                bookingModel.ProvisionalBookingDetail.Hotelinfo.RoomTypeCode = rateRoom.RoomTypeCode;
                bookingModel.ProvisionalBookingDetail.Hotelinfo.Roomtype = rateRoom.Roomtype;
                bookingModel.ProvisionalBookingDetail.Hotelinfo.RatePlanType = rateRoom.RatePlanCode;


                bookingModel.ProvisionalBookingDetail.RoomStayCandidate = new RoomStayCandidate();

                /****Set Room stay candidate Info***/
                bookingModel.ProvisionalBookingDetail.RoomStayCandidate.GuestDetails = ShineYatraSession.HotelRequest.GuestDetails;

                /****Set Rateband nfo***/
                bookingModel.ProvisionalBookingDetail.Ratebands = rateRoom.Ratebands;
                bookingModel.ProvisionalBookingDetail.ResidentOfIndia = "true";

                this.hotelManager = new HotelManager();
                var response = await this.hotelManager.ProvisionBooking(bookingModel.ProvisionalBookingDetail);

                if (response == null)
                {
                    return Json(string.Empty);
                }
                else
                {
                    var bookingRequest = new HotelDescriptionRequest();
                    bookingRequest.hotelid = ShineYatraSession.SelectedHotel.Hoteldetail.HotelId;
                    bookingRequest.webService = ShineYatraSession.HotelRequest.webService;
                    bookingRequest.RoomTypeCode = rateRoom.RoomTypeCode;
                    bookingRequest.RatePlanType = rateRoom.RatePlanCode;
                    bookingRequest.CheckInDate = ShineYatraSession.HotelRequest.Start;
                    bookingRequest.CheckOutDate = ShineYatraSession.HotelRequest.End;
                    bookingRequest.city = ShineYatraSession.HotelRequest.HotelCityName;
                    bookingRequest.Allocid = response.Allocresult.Allocid;
                    bookingRequest.Fromallocation = response.Allocresult.Allocavail;
                    bookingRequest.Roomtype = rateRoom.Roomtype;
                    bookingRequest.WsKey = rateRoom.Ratebands.WsKey;
                    bookingRequest.Roombasis = rateRoom.Roombasis.Replace("&", "And");
                    bookingRequest.RoomStayCandidate = new RoomStayCandidate();
                    bookingRequest.RoomStayCandidate.GuestDetails = ShineYatraSession.HotelRequest.GuestDetails;
                    bookingRequest.GuestInformation = bookingModel.ProvisionalBookingDetail.GuestInformation;
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

        /// <summary>
        /// method to 
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public ActionResult AssessorNameSearch(string Prefix)
        {
            flightManager = new FlightManager();
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
    }
}