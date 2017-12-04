using ShineYatraAdmin.Business;
using ShineYatraAdmin.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ReportManager _reportManager;

        public ReportController()
        {
            _reportManager = new ReportManager();
        }

        /// <summary>
        /// Get list of fund request from members
        /// </summary>
        /// <returns></returns>
        /// 
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

        [HttpPost]
        public async Task<ActionResult> GetLedgerList(DistributorLedgerRequest request)
        {            
            string ledgerList = null;
            MatchCollection matches = null;
            var response = "";
            try
            {
                var userData = User.Identity.Name.Split('|');
                request.Ledger_id = userData[5];
                request.action = "DISTRIBUTOR_LEDGER";

                ledgerList = await _reportManager.GetLedgerList(request);
                var pattern = @"\[(.*?)\]";
                if (ledgerList.ToUpper().Contains("SUCCESS"))
                {
                    matches = Regex.Matches(ledgerList, pattern);
                }
                foreach (Match match in matches)
                {
                    response = match.Value;
                }
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}