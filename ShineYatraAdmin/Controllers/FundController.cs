using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShineYatraAdmin.Entity;
using System.Threading.Tasks;
using ShineYatraAdmin.Business;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class FundController : Controller
    {
        FundManager fundManger = new FundManager();
        CompanyFund companyFund = null;

        public ActionResult FundRequest()
        {
            CompanyFund companyFund = new CompanyFund();
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                companyFund.member_id = userData[1];
                companyFund.company_id = userData[2];
                companyFund.service_id = 0;
                companyFund.cancel_request_id = 0;
                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                companyFund.request_token = GuidString;
                companyFund.txn_type = "FUND";
                companyFund.domain_name = "whitelabel.bisplindia.in";
                companyFund.AssignDepositModeList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(companyFund);
        }

        public async Task<ActionResult> SaveFundDetail(CompanyFund fundModel)
        {
            FundManager fundManger = new FundManager();
            try
            {
                fundModel.action = "INSERT_FUND_REQUEST";
                var response = await fundManger.SaveFundDetail(fundModel);
                return Json(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(string.Empty);
        }
    }
}