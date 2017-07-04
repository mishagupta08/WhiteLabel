using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Business
{
    public class PrimarySettingManager
    {
        /// <summary>
        /// method to get airline primary commission structure
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<IList<CompanyCommissionGroup>> GetPrimarySetting(string serviceId, string sub_service)
        {
            return await Program.GetPrimarySetting(serviceId, sub_service);
        }
        
        /// <summary>
        /// method to save recharge defaut settings
        /// </summary>        
        /// <returns></returns>
        public async Task<DefaultRechargeSetting> GetRechargeDefaultSetting()
        {
            IList<DefaultRechargeSetting> settingList = await Program.GetRechargeDefaultSetting();
            return settingList.FirstOrDefault();
        }


        /// <summary>
        /// method to save recharge defaut settings
        /// </summary>        
        /// <returns></returns>
        public async Task<string> SaveRechargeDefaultSetting(DefaultRechargeSetting addSetting)
        {
            return await Program.SaveRechargeDefaultSetting(addSetting);
        }
        

        /// <summary>
        /// method to update primay setting margin for airline bus and hotel
        /// </summary>        
        /// <returns></returns>
        public async Task<string> UpdatePrimaryMargin(PrimarySettingMargin Row)
        {
            return await Program.UpdatePrimaryMargin(Row);
        }

        /// <summary>
        /// get list of sub services
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public async Task<IList<SubCategory>> GetSubServiceList(string serviceId)
        {
            return await Program.GetSubServiceList(serviceId);
        }

    }
}
