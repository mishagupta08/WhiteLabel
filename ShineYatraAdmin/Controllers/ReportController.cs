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
    public class ReportController : Controller
    {
        /// <summary>
        /// Get list of fund request from members
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Ledger()
        {
            ReportManager reportManager = new ReportManager();
            List<DistributorLedger> ledgerList = new List<DistributorLedger>();
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                ledgerList = await reportManager.GetLedgerList(userData[5]);
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.InnerException);
            }
            return View(ledgerList);
        }        
    }
}