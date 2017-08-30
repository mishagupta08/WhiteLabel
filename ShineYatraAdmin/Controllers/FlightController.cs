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
    /// Holds flight operations
    /// </summary>
    [Authorize]
    public class FlightController : Controller
    {
        /// <summary>
        /// Hold flight model
        /// </summary>
        FlightViewModel flightModel = new FlightViewModel();

        FlightManager flightManager;
        UserManager userManager;

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method to get flight search page
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public ActionResult GetFlightMenu(string menu)
        {
            try
            {
                flightManager = new FlightManager();
                flightModel.SelectedMenu = menu;
                flightModel.FlightSearchDetail = new Request();
                flightModel.FlightSearchDetail.AssignTripMode();
                flightModel.FlightSearchDetail.AssignFlightClass();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }
            return View("Index", flightModel);
        }
        /// <summary>
        /// method to call search flight from source to destination
        /// </summary>
        /// <param name="flightDetail"></param>
        /// <returns></returns>
        public async Task<ActionResult> SearchFlight(FlightViewModel flightDetail)
        {
            flightManager = new FlightManager();
            SearchPageViewModel searchPageViewModel = new SearchPageViewModel();
            try
            {
                searchPageViewModel.flightSearch = flightDetail.FlightSearchDetail;
                searchPageViewModel.arrayOfSearchedFlights = await flightManager.SearchFlight(flightDetail.FlightSearchDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return View("FlightMenu//SearchFlightResult", searchPageViewModel);
        }

        /// <summary>
        /// get method to call flight booking page
        /// </summary>
        /// <param name="passengerDetails"></param>
        /// <returns></returns>
        public async Task<ActionResult> BookFlight(Request passengerDetails)
        {
            flightManager = new FlightManager();
            userManager = new UserManager();
            SearchPageViewModel searchPageViewModel = new SearchPageViewModel();
            searchPageViewModel.flightSearch = passengerDetails;
            searchPageViewModel.FlightBookingDetail = passengerDetails;
            searchPageViewModel.FlightBookingDetail.PersonName = new personName();
            searchPageViewModel.FlightBookingDetail.PersonName.CustomerInfo = new List<CustomerInfo>();
            
            try
            {
                WalletRequest balrequest = new WalletRequest();
                balrequest.action = "GET_WALLET_BALANCE";
                balrequest.domain_name = "nbfcp.bisplindia.in";
                balrequest.ledger_id = "100";
                balrequest.company_id = "1";
                WalletResponse bal_response = await userManager.GET_WALLET_BALANCE(balrequest);
                if (bal_response != null)
                {
                    searchPageViewModel.walletBalance = bal_response.wallet_balance;
                }
            }
            catch (Exception Exx)
            {
                Console.WriteLine(Exx.InnerException);
            }
            var detail = passengerDetails.AdultPax + passengerDetails.ChildPax + passengerDetails.InfantPax;

            for (var count = 0; count < detail; count++)
            {
                searchPageViewModel.FlightBookingDetail.PersonName.CustomerInfo.Add(new CustomerInfo());
            }

            searchPageViewModel.AssignNameReference();
            searchPageViewModel.AssignChildNameReference();

            searchPageViewModel.arrayOfSearchedFlights = await flightManager.SearchFlightOnID(searchPageViewModel.flightSearch);
            return View("FlightMenu//BookingDetail", searchPageViewModel);
        }

        

        /// <summary>
        /// Method save  to book flight ticket
        /// </summary>in
        /// <param name="bookingDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BookingResponse(SearchPageViewModel bookingDetail)
        {
            flightManager = new FlightManager();
            Request request = new Request();
            string status = "";
            Bookingresponse bookResponse = new Bookingresponse();
            try
            {
                request = bookingDetail.FlightBookingDetail;
                request.Creditcardno = "4111111111111111";
                INSERT_SERVICE_BOOKING_REQUEST response = await saveBookingDetail(bookingDetail);
                if (response != null)
                {
                    if (!string.IsNullOrEmpty(response.MSG))
                    {
                        status = response.MSG;
                    }
                    else
                    {
                        if (request.PaymentMode == "bank" || bookingDetail.walletBalance < request.AdultFare)
                        {
                            PayUController cntrl = new PayUController();
                            PayuRequest payrequest = new PayuRequest();
                            payrequest.FirstName = request.PersonName.CustomerInfo.FirstOrDefault().givenName;
                            payrequest.TransactionAmount = "1.0";
                            payrequest.Email = request.EmailAddress;
                            payrequest.Phone = request.PhoneNumber;
                            payrequest.ProductInfo = "Booking Flght " + request.FlightNumber + ": " + " For Name : " + request.PersonName.CustomerInfo.FirstOrDefault().givenName;
                            payrequest.surl = "http://" + Request.Url.Authority + "/PayU/Return";
                            payrequest.furl = "http://" + Request.Url.Authority + "/PayU/Return";
                            cntrl.Payment(payrequest);
                        }                         
                       
                        bookResponse.txn_id = Convert.ToString(response.txn_id);
                        bookResponse = await flightManager.BookTicket(request);
                        if (bookResponse.Status.ToLower() == "success")
                            status = "Ticket Booked successfully";
                        else
                            status = bookResponse.Error;
                            

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            ViewBag.status = status;
            return View();
        }      

        /// <summary>
        /// Method to save booked flight details to datbase
        /// </summary>
        /// <param name="bookingDetail"></param>
        /// <returns></returns>
        public async Task<INSERT_SERVICE_BOOKING_REQUEST> saveBookingDetail(SearchPageViewModel bookingDetail)
        {
            flightManager = new FlightManager();
            BookingDetail ticketDetail = new BookingDetail();
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                ticketDetail.action = "INSERT_SERVICE_BOOKING_REQUEST";
                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                ticketDetail.request_token = GuidString;
                ticketDetail.service_id = 1;
                ticketDetail.sub_service_id = "11";
                ticketDetail.mobile_no = bookingDetail.FlightBookingDetail.PhoneNumber;
                ticketDetail.email = bookingDetail.FlightBookingDetail.EmailAddress;
                ticketDetail.infant = bookingDetail.FlightBookingDetail.InfantPax;
                ticketDetail.child = bookingDetail.FlightBookingDetail.ChildPax;
                ticketDetail.adult = bookingDetail.FlightBookingDetail.AdultPax;
                ticketDetail.travel_from = bookingDetail.FlightBookingDetail.Origin;
                ticketDetail.travel_to = bookingDetail.FlightBookingDetail.Destination;
                ticketDetail.travel_start_Date = bookingDetail.FlightBookingDetail.DepartDate;
                ticketDetail.travel_return_date = bookingDetail.FlightBookingDetail.ReturnDate;
                ticketDetail.trip_mode = bookingDetail.FlightBookingDetail.Mode;
                ticketDetail.deposit_mode = "NEFT";
                ticketDetail.trip_category = "DOMESTIC";
                ticketDetail.txn_type = "TEST";
                ticketDetail.status = "Pending";
                ticketDetail.remarks = "flight booking";
                ticketDetail.category = "DOMESTIC";
                ticketDetail.airline_code = "AI";
                ticketDetail.amount = 10000;
                ticketDetail.member_id = userData[1];
                ticketDetail.company_id = userData[2];
                ticketDetail.trip_class = bookingDetail.FlightBookingDetail.Preferredclass;
                ticketDetail.bookflight = new List<Passengers>();
                foreach (var passenger in bookingDetail.FlightBookingDetail.PersonName.CustomerInfo)

                {
                    ticketDetail.bookflight.Add(new Passengers
                    {
                        passenger_category = passenger.psgrtype,
                        dob = passenger.dob,
                        first_name = passenger.givenName,
                        last_name = passenger.surName,
                        title = passenger.nameReference,
                        extra_field_1 = "A",
                        extra_field_2 = "B",
                        extra_field_3 = "C",
                        extra_field_4 = "D",
                        extra_field_5 = "E",

                    });
                }
                List<INSERT_SERVICE_BOOKING_REQUEST> response = await flightManager.InsertServiceBookingRequest(ticketDetail);
                return response.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return null;
        }

        /// <summary>
        /// Method to cancel flight booking detail
        /// </summary>
        /// <param name="bookingDetail"></param>
        /// <returns></returns>
        public async Task<ActionResult> CancelFlight(EticketRequest bookingDetail)
        {
            flightManager = new FlightManager();
            CancelationDetails bookResponse = new CancelationDetails();
            try
            {
                bookResponse = await flightManager.CancelFlightTicket(bookingDetail);
                return View(bookResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(null);
        }

        /// <summary>
        /// method to get city names stating with prefix
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public async Task<ActionResult> AssessorNameSearch(string Prefix)
        {
            flightManager = new FlightManager();
            try
            {
                List<KeyValuePair> cityName = await flightManager.GetFlightCityList(true);

                cityName = cityName.Where(o => o.Id.ToLower().Trim().StartsWith(Prefix.ToLower().Trim())).ToList();
               
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