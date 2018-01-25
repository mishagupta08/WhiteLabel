using System.Text.RegularExpressions;

namespace ShineYatraAdmin.Controllers
{
    #region namespace

    using Entity;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Business;
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
        private FlightManager _flightManager;
        private ServiceManager _serviceManager;
        private PgManager _pgManager;
        private UserManager _userManager;


        /// <summary>
        /// method to get flight search page
        /// </summary>        
        /// <returns></returns>
        public ActionResult SearchFlight()
        {
            var flightSearchModel = new Request();
            try
            {
                flightSearchModel.AssignTripMode();
                flightSearchModel.AssignFlightClass();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }

            var viewFolder = "FlightMenu//" + System.Web.HttpContext.Current.Session["CompanyTheme"].ToString().ToLower() + "//SearchFlight";
            return View(viewFolder, flightSearchModel);

            //return View("FlightMenu//SearchFlight", flightSearchModel);
        }

        /// <summary>
        /// method to call search flight from source to destination
        /// </summary>
        /// <param name="flightDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SearchFlight(Request flightDetail)
        {
            _flightManager = new FlightManager();
            _serviceManager = new ServiceManager();
            var searchPageViewModel = new SearchPageViewModel();
            var userData = User.Identity.Name.Split('|');
            try
            {
                searchPageViewModel.flightSearch = flightDetail;
                searchPageViewModel.arrayOfSearchedFlights = await _flightManager.SearchFlight(flightDetail);

                var serviceCgDetailsRequest = new AllotedServiceCGsDetailsRequest
                {
                    member_id = userData[1],
                    action = "GET_ALLOTED_SERVICE_COMMISSION_GROUPS_DETAILS",
                    service_id = "1",
                    sub_service_id = "0",
                    category = "FLIGHT",
                    sub_category = "DOMESTIC",
                    service_code = "0"
                };

                var allflightDiscountDetails = await _serviceManager.GetServiceAllottedGroupDetails(serviceCgDetailsRequest);

                var newarray = new ArrayOfOrigindestinationoption
                {
                    Origindestinationoption = new List<Origindestinationoption>()
                };
                
                foreach (var flight in searchPageViewModel.arrayOfSearchedFlights.Origindestinationoption)
                {
                    var subServiceId = flight.FlightsDetailList.FlightsDetail.First().SubServiceId;
                    var flightdiscount = allflightDiscountDetails.FirstOrDefault(o => o.sub_service_id.Equals(subServiceId));
                    var totalFare = flight.FareDetail.ChargeableFares.ActualBaseFare;
                    if (flightdiscount != null)
                    {
                        double discount;
                        if (flightdiscount.front_discount_per > 0)
                        {
                            discount = ((totalFare / 100) * flightdiscount.front_discount_per);
                        }
                        else
                        {
                            discount = flightdiscount.front_discount_amount;
                        }
                        flight.FareDetail.ChargeableFares.ActualBaseFare -= discount;
                        flight.FareDetail.frontdiscount = discount;
                        if (flightdiscount.back_discount_per > 0)
                        {
                            flight.FareDetail.backdiscount = ((totalFare / 100) * flightdiscount.back_discount_per);
                        }
                        else
                        {
                            flight.FareDetail.backdiscount = flightdiscount.back_discount_amount;
                        }
                    }
                    newarray.Origindestinationoption.Add(flight);
                }
                searchPageViewModel.arrayOfSearchedFlights = newarray;
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }

            searchPageViewModel.flightSearch.AssignFlightClass();
            searchPageViewModel.flightSearch.AssignTripMode();
            var viewFolder = "FlightMenu//" + System.Web.HttpContext.Current.Session["CompanyTheme"].ToString().ToLower() + "//SearchFlightResult";
            return View(viewFolder, searchPageViewModel);
        }

        /// <summary>
        /// get method to call flight booking page
        /// </summary>
        /// <param name="passengerDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BookFlight(Request passengerDetails)
        {
            _flightManager = new FlightManager();
            _userManager = new UserManager();
            _serviceManager = new ServiceManager();
            var searchPageViewModel = new SearchPageViewModel();
            try
            {
                var userData = User.Identity.Name.Split('|');
                searchPageViewModel = new SearchPageViewModel
                {
                    flightSearch = passengerDetails,
                    FlightBookingDetail = passengerDetails
                };
                searchPageViewModel.FlightBookingDetail.PersonName = new personName
                {
                    CustomerInfo = new List<CustomerInfo>()
                };

                var detail = passengerDetails.AdultPax + passengerDetails.ChildPax + passengerDetails.InfantPax;

                for (var count = 0; count < detail; count++)
                {
                    searchPageViewModel.FlightBookingDetail.PersonName.CustomerInfo.Add(new CustomerInfo());
                }

                searchPageViewModel.AssignNameReference();
                searchPageViewModel.AssignChildNameReference();
                searchPageViewModel.flightfaredetails = await _flightManager.FlightPricing(searchPageViewModel.flightSearch);
                var serviceCgDetailsRequest = new AllotedServiceCGsDetailsRequest
                {
                    member_id = userData[1],
                    action = "GET_ALLOTED_SERVICE_COMMISSION_GROUPS_DETAILS",
                    service_id = "1",
                    sub_service_id = Convert.ToString(passengerDetails.SubServiceId),
                    category = "FLIGHT",
                    sub_category = "DOMESTIC",
                    service_code = "0"
                };
                var allflightDiscountDetails = await _serviceManager.GetServiceAllottedGroupDetails(serviceCgDetailsRequest);
                var discountDetail = allflightDiscountDetails.FirstOrDefault();
                double discount;
                double totalFare = searchPageViewModel.flightfaredetails.FareDetail.ChargeableFares.ActualBaseFare;
                if (discountDetail.front_discount_per > 0)
                {
                    discount = ((totalFare / 100) * discountDetail.front_discount_per);
                }
                else
                {
                    discount = discountDetail.front_discount_amount;
                }

                searchPageViewModel.flightfaredetails.FareDetail.ChargeableFares.ActualBaseFare -= discount;

                searchPageViewModel.flightfaredetails.FareDetail.backdiscount = searchPageViewModel.flightSearch.backdiscount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }

            var viewFolder = "FlightMenu//" + System.Web.HttpContext.Current.Session["CompanyTheme"].ToString().ToLower() + "//BookingDetail";
            return View(viewFolder, searchPageViewModel);

            //return View("FlightMenu//BookingDetail", searchPageViewModel);
        }


        /// <summary>
        /// Method save  to book flight ticket
        /// </summary>in
        /// <param name="bookingDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> BookResponse(SearchPageViewModel bookingDetail)
        {
            _flightManager = new FlightManager();
            _userManager = new UserManager();
            _pgManager = new PgManager();
            var txnId = string.Empty;
            var info = string.Empty;
            var walletBalance = 0.0;
            var bookResponse = new Bookingresponse();
            try
            {
                var userData = User.Identity.Name.Split('|');
                var request = bookingDetail.FlightBookingDetail;
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

                    var balResponse = await _userManager.GET_WALLET_BALANCE(balrequest);
                    if (balResponse != null)
                    {
                        walletBalance = balResponse.wallet_balance;
                        Session["WalletBalance"] = walletBalance;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
                }

                bookingDetail.walletBalance = walletBalance;
                info = saveBookingDetail(bookingDetail);
                bool pgflag = false;
                if (isPaymentGatewayactive && request.PaymentMode == "bank")
                {
                    double pgamount = 0;

                    if (request.PartialPaymentWithWallet)
                    {
                        if (walletBalance < request.AdultFare)
                        {
                            pgamount = request.AdultFare - walletBalance;
                            pgflag = true;
                        }
                        else
                        {
                            pgamount = 0;
                            pgflag = false;
                        }
                    }
                    else
                    {
                        pgamount = request.AdultFare;
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
                            remarks = "Flight booking payment by payment gateway",
                            amount = request.PartialPaymentWithWallet ? request.AdultFare - walletBalance : request.AdultFare
                        };

                        var balanceTxnId = await _pgManager.SavePaymntGatewayTransactions(paymentDetail);
                        if (!balanceTxnId.ToLower().Contains("failed"))
                        {
                            var cntrl = new PayUController();
                            var payrequest = new PayuRequest
                            {
                                FirstName = request.PersonName.CustomerInfo.FirstOrDefault()?.givenName,
                                TransactionAmount = pgamount,
                                Email = request.EmailAddress,
                                Phone = request.phoneNumber,
                                udf1 = info,
                                udf2 = Convert.ToString(balanceTxnId),
                                memberId = userData[1],
                                ProductInfo = "Booking Flight " + request.FlightNumber + ": " + " For Name : " + request.PersonName.CustomerInfo.FirstOrDefault()?.givenName,
                                surl = "http://" + Request.Url.Authority + "/Flight/Return",
                                furl = "http://" + Request.Url.Authority + "/Flight/Return"
                            };
                            cntrl.Payment(payrequest);
                        }
                        else
                        {
                            TempData["ErrorCode"] = 5001;
                            bookResponse.Status = balanceTxnId;
                            TempData["BookingResponse"] = bookResponse;
                        }
                    }
                }

                if (!pgflag)
                {
                    if (request.AdultFare <= walletBalance)
                    {
                        var myCookie = Request.Cookies["Cookie-" + info];
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        if (myCookie != null)
                        {
                            var ticketDetail = serializer.Deserialize<BookingDetail>(myCookie.Value);
                            var response = await _flightManager.InsertServiceBookingRequest(ticketDetail);
                            var insertServiceResonse = response.FirstOrDefault();
                            if (insertServiceResonse != null)
                            {
                                Session["WalletBalance"] = insertServiceResonse.wallet_balance;
                                txnId = Convert.ToString(insertServiceResonse.txn_id);
                                bookResponse = await _flightManager.BookTicket(request);
                                if (!bookResponse.Status.ToUpper().Equals("SUCCESS"))
                                {
                                    TempData["ErrorCode"] = 5005;
                                }
                            }
                            else
                            {
                                bookResponse.Status = "Failed";
                                TempData["ErrorCode"] = 5002;
                            }
                        }
                    }
                    else
                    {
                        bookResponse.Status = "Failed";
                    }
                    TempData["BookingResponse"] = bookResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return RedirectToAction("BookingStatus", "Flight", new { txnId, info });
        }

        [HttpPost]
        public async Task<ActionResult> Return(FormCollection form)
        {
            var txnId = string.Empty;
            var info = "";
            var bookResponse = new Bookingresponse();
            var request = new Request();
            var flightManager = new FlightManager();
            var serializer = new JavaScriptSerializer();
            try
            {
                var cookieId = form["udf1"];
                info = cookieId;
                var balanceTxId = form["udf2"];
                //getbooking 
                var getticketDetailList = new BookingDetail();

                var myCookie = Request.Cookies["Cookie-" + cookieId];

                // Read the cookie information and display it.
                if (myCookie != null)
                {
                    getticketDetailList = serializer.Deserialize<BookingDetail>(myCookie.Value);
                }

                var ticketDetail = getticketDetailList;
                if (form["status"] == "success")
                {
                    ticketDetail.my_info = "PG_REQUEST," + balanceTxId + "," + Convert.ToString(form["mode"]) + "," + Convert.ToString(form["txnid"]) + "," + Convert.ToString(form["bank_ref_num"]);
                    var response = await flightManager.InsertServiceBookingRequest(ticketDetail);
                    var saveBookingResonse = response.FirstOrDefault();
                    if (saveBookingResonse != null)
                    {
                        txnId = Convert.ToString(saveBookingResonse.txn_id);
                        Session["WalletBalance"] = saveBookingResonse.wallet_balance;
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
                        request.PersonName = new personName { CustomerInfo = new List<CustomerInfo>() };
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
                        if (!bookResponse.Status.ToUpper().Equals("SUCCESS"))
                        {
                            TempData["ErrorCode"] = 5005;
                        }
                    }
                    else
                    {
                        bookResponse.Status = "Failed";
                        TempData["ErrorCode"] = 5002;
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
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return RedirectToAction("BookingStatus", "Flight", new { txnId, info });
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="txnId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<ActionResult> BookingStatus(string txnId, string info)
        {
            BookingDetail eticket = null;
            _serviceManager = new ServiceManager();

            try
            {
                var bookResponse = (Bookingresponse)TempData["BookingResponse"];

                var userData = User.Identity.Name.Split('|');
                var serializer = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(txnId))
                {
                    if (bookResponse.Status.ToLower() == "success")
                    {
                        ViewBag.status = "Ticket Booked Successfully";

                        var myCookie = Request.Cookies["Cookie-" + info];
                        if (myCookie != null)
                        {
                            eticket = serializer.Deserialize<BookingDetail>(myCookie.Value);
                        }
                        UPDATE_TRANSACTION_STATUS updatestatus =
                            await _serviceManager.UpdateServiceBookingRequest(txnId, userData[1], bookResponse.Transid,
                                "COMPLETED");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txnId))
                        {
                            UPDATE_TRANSACTION_STATUS updatestatus =
                                await _serviceManager.UpdateServiceBookingRequest(txnId, userData[1], "", "Failed");
                        }
                        ViewBag.statusCode = TempData["ErrorCode"];
                        ViewBag.status = bookResponse.Status;
                    }
                }
                else
                {
                    ViewBag.statusCode = TempData["ErrorCode"];
                    if (bookResponse != null && !String.IsNullOrEmpty(bookResponse.Status))
                    {
                        ViewBag.status = bookResponse.Status;
                    }
                    else
                    {
                        ViewBag.status = "Some problem Occured while doing your transaction";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
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
                var userData = User.Identity.Name.Split('|');
                ticketDetail.action = "INSERT_SERVICE_BOOKING_REQUEST";
                var g = Guid.NewGuid();
                var guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");
                ticketDetail.request_token = guidString;
                ticketDetail.service_id = 1;
                ticketDetail.discount = bookingDetail.FlightBookingDetail.backdiscount;
                ticketDetail.sub_service_id = bookingDetail.FlightBookingDetail.SubServiceId;
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
                var isPaymentGatewayactive = Convert.ToString(Session["web_pg_api_enabled"]).ToUpper() == "Y" && userData[6] != "3";
                double pgamount = 0;

                if (isPaymentGatewayactive && bookingDetail.FlightBookingDetail.PaymentMode == "bank")
                {
                    if (bookingDetail.FlightBookingDetail.PartialPaymentWithWallet)
                    {
                        if (bookingDetail.walletBalance < bookingDetail.FlightBookingDetail.AdultFare)
                        {
                            pgamount = bookingDetail.FlightBookingDetail.AdultFare - bookingDetail.walletBalance;
                        }
                        else
                        {
                            pgamount = 0;
                        }
                    }
                    else
                    {
                        pgamount = bookingDetail.FlightBookingDetail.AdultFare;
                    }
                }
                else
                {
                    pgamount = 0;
                }

                ticketDetail.pg_amount = pgamount;
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
                        age = passenger.age
                    });
                }

                string ticketDetailJson = new JavaScriptSerializer().Serialize(ticketDetail);
                var cookie = new HttpCookie("Cookie-" + guidString, ticketDetailJson)
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                HttpContext.Response.Cookies.Add(cookie);
                return guidString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return null;
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
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(null);
        }

        public ActionResult MyFlightBookings()
        {
            return View("Report/MyFlightBookings");
        }

        public async Task<ActionResult> GetFlightList(FormCollection frm)
        {
            _flightManager = new FlightManager();
            List<BookingDetail> bookingDetails = null;
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                var request = new FlightBookingListRequest
                {
                    member_id = userData[1],
                    service_id = "1",
                    action = "GET_FLIGHT_TRANSACTIONS_SUMMARY",
                    To_date = frm.GetValue("toDate").AttemptedValue,
                    From_date = frm.GetValue("fromDate").AttemptedValue,
                    Flight_type = frm.GetValue("flighttype") != null ? frm.GetValue("flighttype").AttemptedValue : "",
                    Booking_Status = frm.GetValue("bookStatus") != null ? frm.GetValue("bookStatus").AttemptedValue : ""
                };

                bookingDetails = await _flightManager.GetMemberFlightList(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return PartialView("Report/FilghtList", bookingDetails);
        }

        /// <summary>
        /// Method to cancel flight booking detail
        /// </summary>
        /// <param name="transId"></param>
        /// <param name="partnerRefernceId"></param>
        /// <param name="txnId"></param>
        /// <returns></returns>
        public async Task<ActionResult> CancelFlight(string transId, string partnerRefernceId, string txnId)
        {
            _flightManager = new FlightManager();
            _serviceManager = new ServiceManager();
            try
            {
                var userData = User.Identity.Name.Split('|');
                var cancelTicket = new EticketRequest
                {
                    Transid = transId,
                    PartnerRefId = partnerRefernceId
                };
                var cancelResponse = await _flightManager.CancelFlightTicket(cancelTicket);
                var result = "fail";
                if (cancelResponse != null)
                {
                    result = "success";
                    UPDATE_TRANSACTION_STATUS updatestatus = await _serviceManager.UpdateServiceBookingRequest(txnId, userData[1], transId, "CANCELLED");

                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(null);
        }

    }
}