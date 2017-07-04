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

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method to get flight menu
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public ActionResult GetFlightMenu(string menu)
        {
            try
            {
                flightManager = new FlightManager();
                if (string.IsNullOrEmpty(menu))
                {
                    if (ShineYatraSession.SelectedMenu == null)
                    {
                        return null;
                    }

                    menu = ShineYatraSession.SelectedMenu;
                }

                if (ShineYatraSession.LoginUser == null)
                {
                    return null;
                }

                ShineYatraSession.SelectedMenu = menu;

                flightModel.SelectedMenu = menu;
                flightModel.FlightSearchDetail = new Request();
                flightModel.FlightSearchDetail.AssignTripMode();
                //if (ShineYatraSession.FlightCityList == null || ShineYatraSession.FlightCityList.Count == 0)
                //{
                //    ShineYatraSession.FlightCityList = await flightManager.GetFlightCityList("");
                //}

                //flightModel.FlightCityList = ShineYatraSession.FlightCityList;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }
            return View("Index", flightModel);
        }

        public async Task<ActionResult> SearchFlight(FlightViewModel flightDetail)
        {
            flightManager = new FlightManager();
            SearchPageViewModel searchPageViewModel = new SearchPageViewModel();
            try
            {
                searchPageViewModel.flightSearch = flightDetail.FlightSearchDetail;
                flightDetail.FlightSearchDetail.Preferredclass = "E";
                flightDetail.FlightSearchDetail.Mode = "ONE";
                searchPageViewModel.arrayOfSearchedFlights = await flightManager.SearchFlight(flightDetail.FlightSearchDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return View("FlightMenu//SearchFlightResult", searchPageViewModel);
        }

        public async Task<ActionResult> BookFlight(Request passengerDetails)
        {
            flightManager = new FlightManager();
            SearchPageViewModel searchPageViewModel = new SearchPageViewModel();
            searchPageViewModel.flightSearch = passengerDetails;
            searchPageViewModel.FlightBookingDetail = passengerDetails;
            searchPageViewModel.FlightBookingDetail.PersonName = new personName();
            searchPageViewModel.FlightBookingDetail.PersonName.CustomerInfo = new List<CustomerInfo>();

            var detail = passengerDetails.AdultPax + passengerDetails.ChildPax + passengerDetails.InfantPax;

            for (var count = 0; count < detail; count++)
            {
                searchPageViewModel.FlightBookingDetail.PersonName.CustomerInfo.Add(new CustomerInfo());
            }

            searchPageViewModel.AssignNameReference();

            searchPageViewModel.arrayOfSearchedFlights = await flightManager.SearchFlightOnID(searchPageViewModel.flightSearch);

            return View("FlightMenu//BookingDetail", searchPageViewModel);
        }

        /// <summary>
        /// Method to book detail
        /// </summary>
        /// <param name="bookingDetail"></param>
        /// <returns></returns>
        public async Task<ActionResult> BookingResponse(SearchPageViewModel bookingDetail)
        {
            flightManager = new FlightManager();
            Request request = new Request();
            Bookingresponse bookResponse = new Bookingresponse();
            try
            {
                
                request = bookingDetail.FlightBookingDetail;
                request.Creditcardno = "4111111111111111";
                 bookResponse = await flightManager.BookTicket(request);               
                return View(bookResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(null);            
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
        /// method to 
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