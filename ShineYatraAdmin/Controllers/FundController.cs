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
            companyFund.TransactionDetail = new TransactionDetail();
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

        public async Task<ActionResult> FundTransfer(CompanyFund fundModel)
        {
            if (fundModel == null || fundModel.TransactionDetail == null || string.IsNullOrEmpty(fundModel.TransactionDetail.username) || string.IsNullOrEmpty(fundModel.TransactionDetail.password))
            {
                return Json("Please fill Complete detail.");
            }

            FundManager fundManger = new FundManager();
            try
            {
                //fundModel.action = "INSERT_FUND_REQUEST";
                fundModel.TransactionDetail.action = "GetWalletAmount";
                var userDetail = await fundManger.WalletFunction(fundModel.TransactionDetail);
                if (userDetail == null || string.IsNullOrEmpty(userDetail.msg) || !userDetail.msg.Contains("success"))
                {
                    return Json("Invalid Credential");
                }
                else
                {
                    var walletAmount = Convert.ToDecimal(userDetail.swallet);
                    if (walletAmount < fundModel.TransactionDetail.amount)
                    {
                        return Json("Insufficient balance.");
                    }
                    else
                    {
                        fundModel.TransactionDetail.action = "DeductAmount";
                        fundModel.TransactionDetail.txnData = Guid.NewGuid().ToString();
                        var response = await fundManger.WalletFunction(fundModel.TransactionDetail);
                        if(response == null || string.IsNullOrEmpty(response.msg) || !response.msg.Contains("success"))
                        {
                            return Json("Something went wrong, Please try again later.");
                        }

                        // save voucher no. code here
                    }
                }

                //return Json(response);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException);
            }

            return Json(string.Empty);
        }
    }
}