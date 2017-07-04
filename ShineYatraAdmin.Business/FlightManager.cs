using System;
using ShineYatraAdmin.Entity;
using System.Threading.Tasks;
using ShineYatraAdmin.Repository;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ShineYatraAdmin.Business
{
    public class FlightManager
    {
        /// <summary>
        /// Searchflight details from source to destination
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<ArrayOfFlightsDetail> SearchFlight(Request search)
        {
            ArrayOfFlightsDetail arrayOfFlightsDetail  = new ArrayOfFlightsDetail();
            try
            {
                 arrayOfFlightsDetail = await Program.SearchFlight(search);               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return arrayOfFlightsDetail;
        }


        public async Task<ArrayOfFlightsDetail> SearchFlightOnID(Request search)
        {
            ArrayOfFlightsDetail arrayOfFlightsDetail = new ArrayOfFlightsDetail();
            try
            {
                arrayOfFlightsDetail = await Program.SearchFlight(search);
                arrayOfFlightsDetail.FlightsDetail = arrayOfFlightsDetail.FlightsDetail.Where(o => o.Id == search.Id).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return arrayOfFlightsDetail;
        }

        /// <summary>
        /// Searchflight details from source to destination
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<ArrayOfFlightsDetail> FlightPricing(Request search)
        {
            var flightsDetail = new ArrayOfFlightsDetail();

            try
            {
                flightsDetail = await Program.FlightPricing(search);
                ///searchResponse = "&lt;?xml version =\"1.0\" encoding=\"utf-16\"?&gt;&lt;ArrayOfFlightsDetail xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"&gt;&lt;FlightsDetail&gt;&lt;Id&gt;arzoo11&lt;/Id&gt;&lt;ArrivalAirportCode&gt;DEL&lt;/ArrivalAirportCode&gt;&lt;ArrivalDateTime&gt;2017-05-20T18:10:00&lt;/ArrivalDateTime&gt;&lt;DepartureAirportCode&gt;BOM&lt;/DepartureAirportCode&gt;&lt;DepartureDateTime&gt;2017-05-20T16:00:00&lt;/DepartureDateTime&gt;&lt;FlightNumber&gt;687&lt;/FlightNumber&gt;&lt;OperatingAirlineCode&gt;AI&lt;/OperatingAirlineCode&gt;&lt;StopQuantity&gt;0&lt;/StopQuantity&gt;&lt;ImageFileName&gt;http://live.arzoo.com/FlightWS/image/AirIndia.gif&lt;/ImageFileName&gt;&lt;Availability&gt;9&lt;/Availability&gt;&lt;AirLineName&gt;Air India&lt;/AirLineName&gt;&lt;IsReturnFlight&gt;false&lt;/IsReturnFlight&gt;&lt;BookingClassFare&gt;&lt;adultFare&gt;3400&lt;/adultFare&gt;&lt;bookingclass&gt;T&lt;/bookingclass&gt;&lt;classType&gt;Economy&lt;/classType&gt;&lt;farebasiscode&gt;euK6vFaDvVLgq1HQxhtqJg==&lt;/farebasiscode&gt;&lt;Rule&gt;This fare is Non Refundable &amp;lt;br&amp;gt; Baggage : 25K&amp;lt;br&amp;gt;Booking Class : T|Re-Schedule Charges: Rs. 750 per sector + Fare difference (If any) +admin fee 500 + Service Fee of Rs. 250  Sector .|Cancellation Charges : Basic fare +Airline administration fee 500  + Service Charges 250 Per Passenger Per Sector . |&lt;/Rule&gt;&lt;adultCommission&gt;0&lt;/adultCommission&gt;&lt;childCommission&gt;0&lt;/childCommission&gt;&lt;commissionOnTCharge&gt;0&lt;/commissionOnTCharge&gt;&lt;/BookingClassFare&gt;&lt;/FlightsDetail&gt; &lt;/ArrayOfFlightsDetail&gt;";
                //searchResponse = HttpUtility.HtmlDecode(searchResponse);
                //var serializer = new XmlSerializer(typeof(ArrayOfFlightsDetail));
                //using (TextReader reader = new StringReader(searchResponse))
                //{
                //    flightsDetail = (ArrayOfFlightsDetail)serializer.Deserialize(reader);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return flightsDetail;
        }

        /// <summary>
        /// method to get flight ciy list
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair>> GetFlightCityList(bool isFlight)
        {
            List<KeyValuePair> newlist = null;
            try
            {
                newlist =  await Program.GetFlightCityList(isFlight);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return newlist;
        }

        /// <summary>
        /// function to cALL book ticket api
        /// </summary>
        /// <param name="bookticket"></param>
        /// <returns></returns>
        public async Task<Bookingresponse> BookTicket(Request bookticket)
        {
            Bookingresponse bookresponse = null;
            try
            {
                bookresponse= await Program.BookTicket(bookticket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return bookresponse;
        }

        /// <summary>
        /// function to cALL book ticket api
        /// </summary>
        /// <param name="bookticket"></param>
        /// <returns></returns>
        public async Task<CancelationDetails> CancelFlightTicket(EticketRequest bookticket)
        {
            CancelationDetails bookresponse = null;
            EticketDetails eticketDetail = new EticketDetails();
            try
            {
                bookresponse = await Program.CancelFlightTicket(bookticket);               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return bookresponse;
        }

    }
}
