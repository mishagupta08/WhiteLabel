
using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Business
{
    public class ReportManager
    {
        public async Task<List<DistributorLedger>> GetLedgerList(string ledgerId)
        {
            return await Program.GetLedgerList(ledgerId);
        }
    }
}
