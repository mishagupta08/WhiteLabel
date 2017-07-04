using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;

namespace ShineYatraAdmin.Business
{

    public class FundManager
    {
        /// <summary>
        /// method to add fund details company list
        /// </summary>
        /// <param name="CompanyFund"></param>
        /// <returns></returns>
        public async Task<string> SaveFundDetail(CompanyFund companyFund)
        {
            return await Program.SaveFundDetail(companyFund);
        }
    }
}
