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
        
        public async Task<IList<CompanyCommissionGroup>> GetPrimarySetting(string serviceId, string sub_service, string userData)
        {
            return await Program.GetPrimarySetting(serviceId, sub_service, userData);
        }        
       
        public async Task<List<BillServicesFields>> GetBillServiceFields(string serviceId, string sub_service_provider, string userData)
        {
            return await BillService.GetBillServiceFields(serviceId, sub_service_provider, userData);
        }

        public async Task<List<ZoneList>> GetZoneList(string userData)
        {
            return await BillService.GetZoneList(userData);
        }

        public async Task<List<SubZoneList>> GetSubZoneList(string zoneId, string userData)
        {
            return await BillService.GetSubZoneList(zoneId,userData);
        }

    }
}
