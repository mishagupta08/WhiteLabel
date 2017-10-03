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
    using System.Web.Script.Serialization;
    using System.Configuration;
    using System.Web;

    #endregion namespace

    /// <summary>
    /// Holds flight operations
    /// </summary>
    [Authorize]
    public class FlightController : Controller
    {        
        FlightManager _flightManager;
        UserManager _userManager;

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
            var flightModel = new FlightViewModel();
            try
            {
                _flightManager = new FlightManager();
                flightModel.SelectedMenu = menu;
                flightModel.FlightSearchDetail = new Request();
                flightModel.FlightSearchDetail.AssignTripMode();
                flightModel.FlightSearchDetail.AssignFlightClass();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }
            return View("Index", flightModel.FlightSearchDetail);
        }
        /// <summary>
        /// method to call search flight from source to destination
        /// </summary>
        /// <param name="flightDetail"></param>
        /// <returns></returns>
        public async Task<ActionResult> SearchFlight(Request flightDetail)
        {
            _flightManager = new FlightManager();
            ServiceManager serviceManager = new ServiceManager();
            SearchPageViewModel searchPageViewModel = new SearchPageViewModel();
            try
            {
                searchPageViewModel.flightSearch = flightDetail;
                searchPageViewModel.flightSearch.AssignTripMode();
                searchPageViewModel.flightSearch.AssignFlightClass();
                searchPageViewModel.arrayOfSearchedFlights = await _flightManager.SearchFlight(flightDetail);

                IList<CompanyCommissionGroup> allflightDiscountDetails = await serviceManager.GetSErviceAllottedGroupDetails("1", "", "", "", "");

                ArrayOfOrigindestinationoption newarray = new ArrayOfOrigindestinationoption();
                newarray.Origindestinationoption = new List<Origindestinationoption>();

                foreach (var flight in searchPageViewModel.arrayOfSearchedFlights.Origindestinationoption)
                {
                    CompanyCommissionGroup flightdiscount = allflightDiscountDetails.Where(o => o.sub_service_code.Equals(flight.FlightsDetailList.FlightsDetail.First().OperatingAirlineCode)).FirstOrDefault();
                    double totalFare = flight.FareDetail.ChargeableFares.ActualBaseFare;
                    if (flightdiscount != null && Convert.ToDouble(flightdiscount.front_discount_per) != 0)
                    {
                        flight.FareDetail.ChargeableFares.ActualBaseFare = totalFare - ((totalFare / 100) * Convert.ToDouble(flightdiscount.front_discount_per));
                    }
                    else
                    {
                        if (flightdiscount != null)
                            flight.FareDetail.ChargeableFares.ActualBaseFare =
                                totalFare - Convert.ToDouble(flightdiscount.front_discount_amount);
                    }
                    flight.FareDetail.front_discount_per = flightdiscount.front_discount_per;
                    flight.FareDetail.front_discount_amount = flightdiscount.front_discount_amount;
                    flight.FareDetail.back_discount_per = flightdiscount.back_discount_per;
                    flight.FareDetail.back_discount_amount = flightdiscount.back_discount_amount;
                    newarray.Origindestinationoption.Add(flight);
                }
                searchPageViewModel.arrayOfSearchedFlights = newarray;
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
            _flightManager = new FlightManager();
            _userManager = new UserManager();

            SearchPageViewModel searchPageViewModel = new SearchPageViewModel();
            searchPageViewModel.flightSearch = passengerDetails;
            searchPageViewModel.FlightBookingDetail = passengerDetails;
            searchPageViewModel.FlightBookingDetail.PersonName = new personName();
            searchPageViewModel.FlightBookingDetail.PersonName.CustomerInfo = new List<CustomerInfo>();

            try
            {
                WalletRequest balrequest = new WalletRequest();
                string[] userData = User.Identity.Name.Split('|');
                balrequest.action = "GET_WALLET_BALANCE";
                balrequest.domain_name = ConfigurationManager.AppSettings["DomainName"];
                balrequest.member_id = userData[1];
                balrequest.company_id = userData[2];
                WalletResponse bal_response = await _userManager.GET_WALLET_BALANCE(balrequest);
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
            searchPageViewModel.flightfaredetails = await _flightManager.FlightPricing(searchPageViewModel.flightSearch);

            double totalFare = searchPageViewModel.flightfaredetails.FareDetail.ChargeableFares.ActualBaseFare;

            if (Convert.ToDouble(passengerDetails.front_discount_per) != 0)
            {
                searchPageViewModel.flightfaredetails.FareDetail.ChargeableFares.ActualBaseFare = totalFare - ((totalFare / 100) * Convert.ToDouble(passengerDetails.front_discount_per));
            }
            else
            {
                searchPageViewModel.flightfaredetails.FareDetail.ChargeableFares.ActualBaseFare = totalFare - Convert.ToDouble(passengerDetails.front_discount_amount);
            }

            totalFare = searchPageViewModel.flightfaredetails.FareDetail.ChargeableFares.ActualBaseFare;

            if (Convert.ToDouble(passengerDetails.back_discount_per) != 0)
            {
                searchPageViewModel.flightfaredetails.FareDetail.discountedFare = ((totalFare / 100) * Convert.ToDouble(passengerDetails.back_discount_per));
            }
            else
            {
                searchPageViewModel.flightfaredetails.FareDetail.discountedFare = Convert.ToDouble(passengerDetails.back_discount_amount);
            }


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
            _flightManager = new FlightManager();
            string txnId = string.Empty;
            string info = string.Empty;
            Bookingresponse bookResponse = new Bookingresponse();
            try
            {
                var request = bookingDetail.FlightBookingDetail;
                request.Creditcardno = "4111111111111111";
                string[] userData = User.Identity.Name.Split('|');


                //INSERT_SERVICE_BOOKING_REQUEST response = await saveBookingDetail(bookingDetail);
                var cookieId = saveBookingDetail(bookingDetail);
                info = cookieId;
                if (request.PaymentMode == "bank" || bookingDetail.walletBalance < request.AdultFare)
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
                    remarks = "Flight booking payment by payment gateway",
                    amount = request.PaymentMode == "bank" ? request.AdultFare : request.AdultFare - bookingDetail.walletBalance
                };
                                        
                    string balanceTxnId = await _flightManager.SavePaymntGatewayTransactions(paymentDetail);
                    if (!balanceTxnId.ToLower().Contains("failed"))
                    {
                        
                        PayUController cntrl = new PayUController();
                        PayuRequest payrequest = new PayuRequest
                        {
                            FirstName = request.PersonName.CustomerInfo.FirstOrDefault()?.givenName,
                            TransactionAmount = request.PaymentMode == "bank"
                                ? request.AdultFare
                                : request.AdultFare - bookingDetail.walletBalance,
                            Email = request.EmailAddress,
                            Phone = request.phoneNumber,
                            udf1 = Convert.ToString(cookieId),
                            udf2 = Convert.ToString(balanceTxnId),
                            memberId = userData[1],
                            ProductInfo = "Booking Flight " + request.FlightNumber + ": " + " For Name : " +
                                          request.PersonName.CustomerInfo.FirstOrDefault()?.givenName,
                            surl = "http://" + Request.Url.Authority + "/Flight/Return",
                            furl = "http://" + Request.Url.Authority + "/Flight/Return"
                        };
                        cntrl.Payment(payrequest);
                    }                    
                }
                else
                {
                     var myCookie = Request.Cookies["Cookie-" + cookieId];
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        if (myCookie != null)
                        {
                            var ticketDetail = serializer.Deserialize<BookingDetail>(myCookie.Value);
                            var response = await _flightManager.InsertServiceBookingRequest(ticketDetail);
                            var insertServiceResonse = response.FirstOrDefault();
                            if (insertServiceResonse != null)
                            {
                                txnId = Convert.ToString(insertServiceResonse.txn_id);
                                bookResponse = await _flightManager.BookTicket(request);
                               
                            }
                            else
                            {
                                bookResponse.Status = "Failed";
                            }
                            TempData["BookingResponse"] = bookResponse;
                         }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return RedirectToAction("BookingStatus", "Flight", new {txnId,info });
        }

        [HttpPost]
        public async Task<ActionResult> Return(FormCollection form)
        {            
            string txnId = string.Empty;
            string info = "";
            Bookingresponse bookResponse = new Bookingresponse();
            Request request = new Request();
            FlightManager flightManager = new FlightManager();
            JavaScriptSerializer serializer = new JavaScriptSerializer();           
            try
            {
                string cookieId = form["udf1"];
                info = cookieId;
                string balanceTxId = form["udf2"];
                //getbooking 
                BookingDetail getticketDetailList = new BookingDetail();
                string[] userData = User.Identity.Name.Split('|');
                //getticketDetailList = await flightManager.getBookingDetails(txnId, userData[1]);

                var myCookie = Request.Cookies["Cookie-" + cookieId];

                // Read the cookie information and display it.
                if (myCookie != null)
                {
                    getticketDetailList = serializer.Deserialize<BookingDetail>(myCookie.Value);
                }

                var ticketDetail = getticketDetailList;
                if (form["status"] == "success")
                {
                    ticketDetail.my_info = "PG_REQUEST,"+ balanceTxId + ","+ Convert.ToString(form["mode"]) + ","+ Convert.ToString(form["txnid"]) + ","+ Convert.ToString(form["bank_ref_num"]) ;
                    List<INSERT_SERVICE_BOOKING_REQUEST> response = await flightManager.InsertServiceBookingRequest(ticketDetail);
                    var saveBookingResonse = response.FirstOrDefault();
                    if (saveBookingResonse != null)
                    {
                        txnId = Convert.ToString(saveBookingResonse.txn_id);

                        request.Origin = ticketDetail.travel_from;
                        request.Destination = ticketDetail.travel_to;
                        request.DepartDate = ticketDetail.travel_date;
                        request.AdultPax = ticketDetail.adult;
                        request.ChildPax = ticketDetail.child;
                        request.InfantPax = ticketDetail.infant;
                        request.Preferredclass = ticketDetail.trip_class;
                        request.Mode = ticketDetail.trip_mode;
                        request.Id = ticketDetail.flight_id;
                        request.FlightNumber = ticketDetail.flight_no;
                        request.phoneNumber = ticketDetail.mobile_no;
                        request.EmailAddress = ticketDetail.email;
                        request.PartnerRefId = saveBookingResonse.unique_ref_no;
                        request.Creditcardno = "4111111111111111";
                        request.PersonName = new personName {CustomerInfo = new List<CustomerInfo>()};
                        foreach (var passenger in ticketDetail.passenger_details)
                        {
                            request.PersonName.CustomerInfo.Add(new CustomerInfo
                            {
                                psgrtype = passenger.passenger_category,
                                dob = passenger.dob,
                                givenName = passenger.first_name,
                                surName = passenger.last_name,
                                nameReference = passenger.title,
                                age = passenger.age                                

                            });
                        }
                        bookResponse = await flightManager.BookTicket(request);                        
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
            return RedirectToAction("BookingStatus", "Flight", new { txnId,info });
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="txnId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<ActionResult> BookingStatus(string txnId,string info)
        {
            BookingDetail eticket = null;
            _flightManager = new FlightManager();
            try
            {
                var bookResponse = (Bookingresponse) TempData["BookingResponse"];         
                   
                var userData = User.Identity.Name.Split('|');
                var serializer = new JavaScriptSerializer();
                if (bookResponse != null && bookResponse.Status.ToLower() == "success")
                {
                    ViewBag.status = "Ticket Booked Successfully";
                    UPDATE_TRANSACTION_STATUS updatestatus = await _flightManager.UpdateServiceBookingRequest(txnId, userData[1], bookResponse.Transid, "COMPLETED");                   
                                       
                }
                else if (!string.IsNullOrEmpty(bookResponse?.Error))
                {
                    ViewBag.status = "Some Problem occured while booking, Please try again.";
                    UPDATE_TRANSACTION_STATUS updatestatus = await _flightManager.UpdateServiceBookingRequest(txnId, userData[1], "", "Failed");
                }
                else
                {
                    ViewBag.status = "Some Problem occured while booking, Please try again.";                    
                }

                var myCookie = Request.Cookies["Cookie-" + info];
                if (myCookie != null)
                {
                    eticket = serializer.Deserialize<BookingDetail>(myCookie.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(eticket);
        }

        /// <summary>
        /// Method to save booked flight details to datbase
        /// </summary>
        /// <param name="bookingDetail"></param>
        /// <returns></returns>
        public string saveBookingDetail(SearchPageViewModel bookingDetail)
        {
            _flightManager = new FlightManager();
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
                ticketDetail.mobile_no = bookingDetail.FlightBookingDetail.phoneNumber;
                ticketDetail.email = bookingDetail.FlightBookingDetail.EmailAddress;
                ticketDetail.infant = bookingDetail.FlightBookingDetail.InfantPax;
                ticketDetail.child = bookingDetail.FlightBookingDetail.ChildPax;
                ticketDetail.adult = bookingDetail.FlightBookingDetail.AdultPax;
                ticketDetail.travel_from = bookingDetail.FlightBookingDetail.Origin;
                ticketDetail.travel_to = bookingDetail.FlightBookingDetail.Destination;
                ticketDetail.travel_date = bookingDetail.FlightBookingDetail.DepartDate;
                ticketDetail.travel_return_date = bookingDetail.FlightBookingDetail.ReturnDate;
                ticketDetail.trip_mode = bookingDetail.FlightBookingDetail.Mode;
                ticketDetail.deposit_mode = bookingDetail.FlightBookingDetail.PaymentMode;
                ticketDetail.trip_category = "DOMESTIC";
                ticketDetail.txn_type = "TEST";
                ticketDetail.status = "Pending";
                ticketDetail.remarks = "flight booking";
                ticketDetail.category = "DOMESTIC";
                ticketDetail.flight_id = bookingDetail.FlightBookingDetail.Id;
                ticketDetail.flight_no = bookingDetail.FlightBookingDetail.FlightNumber;
                ticketDetail.ref_code = bookingDetail.FlightBookingDetail.OperatingAirlineCode;
                ticketDetail.amount = bookingDetail.FlightBookingDetail.AdultFare;
                ticketDetail.member_id = userData[1];
                ticketDetail.company_id = userData[2];
                ticketDetail.trip_class = bookingDetail.FlightBookingDetail.Preferredclass;
                ticketDetail.my_info = "";
                ticketDetail.passenger_details = new List<Passengers>();
                if (bookingDetail.FlightBookingDetail.PaymentMode.ToLower().Trim() == "bank")
                {
                    ticketDetail.pg_amount = bookingDetail.FlightBookingDetail.AdultFare;
                }
                else
                {
                    if (bookingDetail.walletBalance < bookingDetail.FlightBookingDetail.AdultFare)
                    {
                        ticketDetail.pg_amount = bookingDetail.FlightBookingDetail.AdultFare - bookingDetail.walletBalance;
                    }
                    else
                    {
                        ticketDetail.pg_amount = 0;
                    }
                }

                ticketDetail.other_amount = 0;
                ticketDetail.total_paid_amount = 0;
                foreach (var passenger in bookingDetail.FlightBookingDetail.PersonName.CustomerInfo)

                {
                    ticketDetail.passenger_details.Add(new Passengers
                    {
                        passenger_category = passenger.psgrtype,
                        dob = passenger.dob,
                        first_name = passenger.givenName,
                        last_name = passenger.surName,
                        title = passenger.nameReference,
                        age = passenger.age,
                        //extra_field_1 = "A",
                        //extra_field_2 = "B",
                        //extra_field_3 = "C",
                        //extra_field_4 = "D",
                        //extra_field_5 = "E",

                    });
                }

                string ticketDetailJson = new JavaScriptSerializer().Serialize(ticketDetail);
                var cookie = new HttpCookie("Cookie-" + GuidString, ticketDetailJson)
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                HttpContext.Response.Cookies.Add(cookie);

                //List<INSERT_SERVICE_BOOKING_REQUEST> response = await flightManager.InsertServiceBookingRequest(ticketDetail);
                //return response.FirstOrDefault();
                return GuidString;
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
        public async Task<ActionResult> CancelFlight(string transId,string partnerRefernceId,string txnId )
        {
            _flightManager = new FlightManager();
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                EticketRequest cancelTicket = new EticketRequest();
                cancelTicket.Transid = transId;
                cancelTicket.PartnerRefId = partnerRefernceId;
                var cancelResponse = await _flightManager.CancelFlightTicket(cancelTicket);
                string result = "fail";
                if (cancelResponse != null)
                {
                    result = "success";
                    UPDATE_TRANSACTION_STATUS updatestatus = await _flightManager.UpdateServiceBookingRequest(txnId, userData[1], transId, "CANCELLED");

                }
                return Json(result, JsonRequestBehavior.AllowGet);
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
            _flightManager = new FlightManager();
            try
            {
                List<KeyValuePair> cityName = await _flightManager.GetFlightCityList(true);
                cityName = (from r in cityName where r.Value.ToLower().Trim().StartsWith(Prefix.ToLower().Trim()) || r.Id.ToLower().Trim().StartsWith(Prefix.ToLower().Trim()) select r).ToList();
                return Json(cityName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(null);
        }

        public async Task<ActionResult> MyFlightBookings()
        {
            _flightManager = new FlightManager();
            List<BookingDetail> bookingDetails = null;
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                bookingDetails = await _flightManager.GetMemberFlightList(userData[1],"1");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(bookingDetails);
        }
    }
}