
using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Business
{
    public class ReportManager
    {
        public async Task<string> GetLedgerList(DistributorLedgerRequest request)
        {
            return await Reports.GetLedgerList(request);
        }
    }
}
