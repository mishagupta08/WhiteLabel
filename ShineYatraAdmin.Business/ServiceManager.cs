using System;

namespace ShineYatraAdmin.Business
{
    #region namespace

    using Entity;
    using Repository;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #endregion namespace

    public class ServiceManager
    {
        /// <summary>
        /// method to get service group list
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="companyId"></param>
        /// <param name="serviceId"></param>
        /// <param name="type"></param>
        /// <param name="sub_category"></param>
        /// <returns> list of company services</returns>
        public async Task<IList<CompanyCommissionGroup>> GetServiceGroupList(string serviceId, string memberId, string companyId, string type, string sub_category)
        {
            return await Program.GetServiceGroupList(serviceId, memberId, companyId, type, sub_category);
        }

        /// <summary>
        /// method to get service group list
        /// </summary>        
        /// <param name="serviceId"></param>
        /// <param name="memberId"></param>
        /// <param name="type"></param>
        /// <param name="sub_category"></param>
        /// <returns> list of company services</returns>
        public async Task<IList<CompanyCommissionGroup>> GetGroupList(string serviceId, string memberId, string type, string sub_category)
        {
            return await Program.GetGroupList(serviceId, memberId, type, sub_category);
        }

        /// <summary>
        /// method to get service allotted group details for a paticular service
        /// </summary>        
        /// <param name="allotedServiceCGsDetailsRequest"></param>
        /// <returns> list of company services</returns>
        public async Task<IList<CompanyCommissionGroup>> GetServiceAllottedGroupDetails(AllotedServiceCGsDetailsRequest allotedServiceCGsDetailsRequest)
        {
            return await Program.GetServiceAllottedGroupDetails(allotedServiceCGsDetailsRequest);
        }

        /// <summary>
        /// Update Service request status in database 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="memberId"></param>
        /// <param name="apiTxnId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<UPDATE_TRANSACTION_STATUS> UpdateServiceBookingRequest(string transactionId, string memberId, string apiTxnId, string status)
        {
            UPDATE_TRANSACTION_STATUS updateresponse = null;
            try
            {
                updateresponse = await Program.UpdateServiceBookingRequest(transactionId, memberId, apiTxnId, status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return updateresponse;
        }
    }
}
