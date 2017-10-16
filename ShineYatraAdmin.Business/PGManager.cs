using System.Threading.Tasks;
using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;

namespace ShineYatraAdmin.Business
{
    public class PgManager
    {
        /// <summary>
        /// method to save fund detail while payment from payment gateway
        /// </summary>
        /// <param name="fundDetail"></param>
        /// <returns></returns>
        public async Task<string> SavePaymntGatewayTransactions(CompanyFund fundDetail)
        {
            return await Program.SavePaymntGatewayTransactions(fundDetail);
        }
    }
}
