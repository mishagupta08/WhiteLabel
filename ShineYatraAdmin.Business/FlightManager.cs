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
        public async Task<ArrayOfOrigindestinationoption> SearchFlight(Request search)
        {
            ArrayOfOrigindestinationoption arrayOfFlightsDetail  = new ArrayOfOrigindestinationoption();
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

        /// <summary>
        /// Searchflight details from source to destination
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<OriginDestinationOption> FlightPricing(Request search)
        {
            var flightsDetail = new OriginDestinationOption();

            try
            {
                flightsDetail = await Program.FlightPricing(search);                
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

        public async Task<List<INSERT_SERVICE_BOOKING_REQUEST>> InsertServiceBookingRequest(BookingDetail bookticket)
        {
            List<INSERT_SERVICE_BOOKING_REQUEST> response = null;
            try
            {
                response = await Program.InsertServiceBookingRequest(bookticket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return response;
        }
        

        public async Task<List<BookingDetail>> getBookingDetails(string transactionId,string memberId)
        {
            List<BookingDetail> response = null;
            try
            {
                response = await Program.GetServiceBookingRequest(transactionId,memberId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return response;
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

        public async Task<UPDATE_TRANSACTION_STATUS> UpdateServiceBookingRequest(int TransactionId, string memberId, string api_txn_id, string status)
        {
            UPDATE_TRANSACTION_STATUS updateresponse = null;
            try
            {
                updateresponse = await Program.UpdateServiceBookingRequest(TransactionId,memberId,api_txn_id,status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return updateresponse;
        }

        /// <summary>
        /// method to save fund detail while payment from payment gateway
        /// </summary>
        /// <param name="fundDetail"></param>
        /// <returns></returns>
        public static async Task<string> SavePaymntGatewayTransactions(CompanyFund fundDetail)
        {
            return await Program.SavePaymntGatewayTransactions(fundDetail);
        }
    }
}
