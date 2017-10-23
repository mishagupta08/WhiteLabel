namespace ShineYatraAdmin.Business
{
    #region namespace

    using Entity;
    using Repository;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    #endregion namespace

    /// <summary>
    /// Holds Company Manager
    /// </summary>
    public class CompanyManager
    {
        /// <summary>
        /// method to get company services list
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns> list of company services</returns>
        public async Task<IList<Service>> GetCompanyServices(string companyId)
        {
            return await Program.GetCompanyServices(companyId);
        }

        /// <summary>
        /// method to add edit company
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<string> EditCompanyServiceGroup(int company_id, int price_group_id, int service_id, string member_id,string category,string sub_category)
        {
            return await Program.EditCompanyServiceGroup(company_id, price_group_id, service_id, member_id,category,sub_category);
        }

        /// <summary>
        /// method to get details of all price groups according to company and services
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<IList<CompanyCommissionGroup>> GetCompanyCommissionStructure(string currentGroupId,string serviceid,string subcategory)
        {
            return await Program.GetCompanyCommissionStructure(currentGroupId,serviceid,subcategory);
        }

        /// <summary>
        /// method to get details of all price groups according to company and services
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<IList<CompanyCommissionGroup>> GetRechargeCommissionStructure(string companyId)
        {
            return await Program.GetRechargeCommissionStructure(companyId);
        }

        /// <summary>
        /// method to get company list
        /// </summary>
        /// <param name="loginDetail"></param>
        /// <returns> list of company</returns>
        public async Task<IList<Company>> GetCompany(string memberId, string companyId)
        {
            return await Program.GetCompany(memberId, companyId);
        }

        /// <summary>
        /// method to add edit company
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<string> AddEditCompany(Company company)
        {
            return await Program.AddEditCompany(company);
        }

        /// <summary>
        /// method to save company setting
        /// </summary>
        /// <param name="companySettingModel"></param>
        /// <returns></returns>
        public async Task<string> SaveCompanySetting(CompanyViewModel companySettingModel)
        {
            return await Program.SaveCompanySetting(companySettingModel);
        }

        /// <summary>
        /// method to 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<CompanySetting> GetCompanySetting(string companyId)
        {
            return await Program.GetCompanySetting(companyId);
        }
        

        /// <summary>
        /// method to 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<CompanySetting> GetCompanyExtraSetting(string companyId)
        {
            return await Program.GetCompanyExtraSetting(companyId);
        }


    }
}
