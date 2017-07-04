using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShineYatraAdmin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
          name: "ValidateLoginUser",
          url: "{controller}/{action}",
          defaults: new { controller = "Login", action = "ValidateUser" }
          );

            routes.MapRoute(
        name: "LogOut",
        url: "{controller}/{action}",
        defaults: new { controller = "Dashboard", action = "LogOut" }
        );
            routes.MapRoute(
        name: "GetInsertCompanyView",
        url: "{controller}/{action}",
        defaults: new { controller = "Company", action = "GetInsertCompanyView" }
        );

            routes.MapRoute(
        name: "GetEditCompanySettingView",
        url: "{controller}/{action}",
        defaults: new { controller = "Company", action = "GetEditCompanySettingView" }
        );

            routes.MapRoute(
         name: "SubmitCompanySetting",
         url: "{controller}/{action}",
         defaults: new { controller = "Company", action = "SubmitCompanySetting" }
         );

            routes.MapRoute(
          name: "GetAddFundView",
          url: "{controller}/{action}",
          defaults: new { controller = "Company", action = "GetAddFundView" }
          );

            routes.MapRoute(
         name: "SaveFundDetail",
         url: "{controller}/{action}",
         defaults: new { controller = "Company", action = "SaveFundDetail" }
         );

            routes.MapRoute(
           name: "GetFlightMenu",
           url: "{controller}/{action}",
           defaults: new { controller = "Flight", action = "GetFlightMenu" }
           );

            routes.MapRoute(
          name: "SearchFlight",
          url: "{controller}/{action}",
          defaults: new { controller = "Flight", action = "SearchFlight" }
          );

            routes.MapRoute(
         name: "BookingPersonDetail",
         url: "{controller}/{action}",
         defaults: new { controller = "Flight", action = "BookingPersonDetail" }
         );

            routes.MapRoute(
         name: "SearchHotel",
         url: "{controller}/{action}",
         defaults: new { controller = "Hotel", action = "SearchHotel" }
         );

            routes.MapRoute(
        name: "GetHotelDetailView",
        url: "{controller}/{action}",
        defaults: new { controller = "Hotel", action = "GetHotelDetailView" }
        );

            routes.MapRoute(
          name: "BookHotelView",
          url: "{controller}/{action}",
          defaults: new { controller = "Hotel", action = "BookHotelView" }
          );

            routes.MapRoute(
        name: "SubmitBookingDetail",
        url: "{controller}/{action}",
        defaults: new { controller = "Hotel", action = "SubmitBookingDetail" }
        );

            routes.MapRoute(
        name: "GetHotelCancelView",
        url: "{controller}/{action}",
        defaults: new { controller = "Hotel", action = "GetHotelCancelView" }
        );

            routes.MapRoute(
       name: "SubmitHotelCancelrequest",
       url: "{controller}/{action}",
       defaults: new { controller = "Hotel", action = "SubmitHotelCancelrequest" }
       );

            routes.MapRoute(
      name: "Payment",
      url: "{controller}/{action}",
      defaults: new { controller = "PayU", action = "Payment" }
      );

            routes.MapRoute(
      name: "Return",
      url: "{controller}/{action}",
      defaults: new { controller = "PayU", action = "Return" }
      );
        }
    }
}
