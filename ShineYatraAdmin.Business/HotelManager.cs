using System;
using ShineYatraAdmin.Entity;
using System.Threading.Tasks;
using ShineYatraAdmin.Repository;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ShineYatraAdmin.Entity.HotelDetail;

namespace ShineYatraAdmin.Business
{
    public class HotelManager
    {
        /// <summary>
        /// Searchflight details from source to destination
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<ArzHotelAvailResp> SearchHotel(HotelRequest search)
        {
            ArzHotelAvailResp arrayOfFlightsDetail = new ArzHotelAvailResp();

            try
            {
                arrayOfFlightsDetail = await HotelApi.SearchHotel(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return arrayOfFlightsDetail;
        }

        /// <summary>
        /// method to get hotel detail
        /// </summary>
        /// <param name="searchHotelDetail"></param>
        /// <returns></returns>
        public async Task<Entity.HotelDetail.ArzHotelDescResp> GetHotelDetail(HotelDescriptionRequest searchHotelDetail)
        {
            Entity.HotelDetail.ArzHotelDescResp arrayOfFlightsDetail = new Entity.HotelDetail.ArzHotelDescResp();

            try
            {
                arrayOfFlightsDetail = await HotelApi.GetHotelDetail(searchHotelDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return arrayOfFlightsDetail;
        }

        /// <summary>
        /// method to get hotel detail
        /// </summary>
        /// <param name="searchHotelDetail"></param>
        /// <returns></returns>
        public async Task<Entity.HotelDetail.Hotel> SearchHotelWithDetail(HotelRequest searchHotelDetail)
        {
            Entity.HotelDetail.Hotel arrayOfFlightsDetail = new Entity.HotelDetail.Hotel();

            try
            {
                arrayOfFlightsDetail = await HotelApi.SearchHotelWithDetail(searchHotelDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return arrayOfFlightsDetail;
        }

        /// <summary>
        /// method to get hotel detail
        /// </summary>
        /// <param name="searchHotelDetail"></param>
        /// <returns></returns>
        public async Task<ArzHotelProvResp> ProvisionBooking(ProvisionalBooking provisionalBooking)
        {
            ArzHotelProvResp arrayOfFlightsDetail = new ArzHotelProvResp();

            try
            {
                arrayOfFlightsDetail = await HotelApi.ProvisionBooking(provisionalBooking);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return arrayOfFlightsDetail;
        }

        /// <summary>
        /// method to get hotel detail
        /// </summary>
        /// <param name="searchHotelDetail"></param>
        /// <returns></returns>
        public async Task<ArzHotelBookingResp> BookingConfirmation(HotelDescriptionRequest provisionalBooking)
        {
            ArzHotelBookingResp arrayOfFlightsDetail = new ArzHotelBookingResp();

            try
            {
                arrayOfFlightsDetail = await HotelApi.BookingConfirmation(provisionalBooking);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return arrayOfFlightsDetail;
        }

        /// <summary>
        /// method to get hotel detail
        /// </summary>
        /// <param name="searchHotelDetail"></param>
        /// <returns></returns>
        public async Task<ArzHotelCancellationRes> BookingCancellation(HotelDescriptionRequest provisionalBooking)
        {
            ArzHotelCancellationRes arrayOfFlightsDetail = new ArzHotelCancellationRes();

            try
            {
                arrayOfFlightsDetail = await HotelApi.BookingCancellation(provisionalBooking);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return arrayOfFlightsDetail;
        }

        public async Task<List<INSERT_SERVICE_HOTEL_REQUEST>> InsertServiceBookingRequest(HotelBoookingDetail bookticket)
        {
            List<INSERT_SERVICE_HOTEL_REQUEST> response = null;
            try
            {
                response = await Program.InsertHotelServiceBookingRequest(bookticket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return response;
        }

        public async Task<List<HotelBookingContainer>> GetHotelsTransactionSummaryList(string member_id)
        {
            List<HotelBookingContainer> response = null;
            try
            {
                response = await HotelApi.GetHotelsTransactionSummaryList(member_id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return response;
        }


    }
}
