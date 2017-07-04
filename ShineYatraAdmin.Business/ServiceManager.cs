﻿namespace ShineYatraAdmin.Business
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
        public async Task<IList<CompanyCommissionGroup>> GetServiceGroupList(string serviceId,string memberId,string companyId,string type,string sub_category)
        {
            return await Program.GetServiceGroupList(serviceId,memberId,companyId,type,sub_category);
        }       
    }
}
