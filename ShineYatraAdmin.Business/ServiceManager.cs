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
        /// <param name="companyId"></param>
        /// <returns> list of company services</returns>
        public async Task<IList<CompanyCommissionGroup>> GetServiceGroupList(string serviceId, string memberId, string companyId, string type, string sub_category)
        {
            return await Program.GetServiceGroupList(serviceId, memberId, companyId, type, sub_category);
        }

        /// <summary>
        /// method to get service group list
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns> list of company services</returns>
        public async Task<IList<CompanyCommissionGroup>> GetGroupList(string serviceId, string memberId, string type, string sub_category)
        {
            return await Program.GetGroupList(serviceId, memberId, type, sub_category);
        }

        /// <summary>
        /// method to get service allotted group details for a paticular service
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns> list of company services</returns>
        public async Task<IList<CompanyCommissionGroup>> GetSErviceAllottedGroupDetails(string memberId, string service_code, string serviceid, string category, string sub_category,string sub_service_id)
        {
            return await Program.GetServiceAllottedGroupDetails(memberId,"","","","",sub_service_id);
        }
    }
}
