using ShineYatraAdmin.Business;
using ShineYatraAdmin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        /// <summary>
        /// Get list of fund request from members
        /// </summary>
        /// <returns></returns>
        public ActionResult Ledger()
        {            
            try
            {
                
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.InnerException);
            }
            return View();
        }

        public async Task<ActionResult> GetLedgerList(FormCollection frm)
        {
            ReportManager reportManager = new ReportManager();
            List<DistributorLedger> ledgerList = new List<DistributorLedger>();
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                var request = new DistributorLedgerRequest
                {
                    Ledger_id = userData[5],
                    action = "DISTRIBUTOR_LEDGER",
                    To_date = frm.GetValue("toDate").AttemptedValue,
                    From_date = frm.GetValue("fromDate").AttemptedValue
                };

                ledgerList = await reportManager.GetLedgerList(request);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.InnerException);
            }
            return PartialView("LedgerList",ledgerList);
        }
    }
}