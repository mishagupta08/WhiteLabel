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
        // GET: Fund
        public ActionResult FundRequest()
        {
            this.companyFund = new CompanyFund();
            try
            {

                this.companyFund.member_id = ShineYatraSession.LoginUser.member_id;
                this.companyFund.company_id = ShineYatraSession.LoginUser.company_id;
                this.companyFund.service_id = 0;
                this.companyFund.cancel_request_id = 0;
                this.companyFund.txn_type = "FUND";
                this.companyFund.AssignDepositModeList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(companyFund);
        }

        public async Task<ActionResult> SaveFundDetail(CompanyFund fundModel)
        {
            try
            {
                fundModel.action = "INSERT_TRANSACTION_REQUEST";
                var response = await this.fundManger.SaveFundDetail(fundModel);
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