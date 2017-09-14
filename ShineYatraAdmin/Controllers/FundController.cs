using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShineYatraAdmin.Entity;
using System.Threading.Tasks;
using ShineYatraAdmin.Business;
using System.Configuration;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class FundController : Controller
    {
        FundManager fundManger = new FundManager();        

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
                        /**Insert into database for fund request.**/

                        var fundDetail = new FundRequestContainer();
                        string[] userData = User.Identity.Name.Split('|');

                        fundDetail.action = "WALLET_CREDIT_3RD_PARTY_REQUEST";
                        fundDetail.domain_name = ConfigurationManager.AppSettings["DomainName"];
                        fundDetail.request_token = "wt_" + Guid.NewGuid().ToString().Substring(0, 4);
                        fundDetail.txn_type = "FUND_WALLET";
                        fundDetail.member_id = userData[1];
                        fundDetail.amount = (float)(fundModel.TransactionDetail.amount);
                        fundDetail.remarks = "Fund transfer";

                        fundDetail.status = string.Empty;
                        fundDetail.ref_no = string.Empty;
                        fundDetail.txn_id = string.Empty;

                        var refNO = await fundManger.WalletCreditRequest(fundDetail);
                        if (refNO == null)
                        {
                            return Json("Something went wrong, Please try again later.");
                        }
                        else
                        {
                            fundModel.TransactionDetail.action = "DeductAmount";
                            fundModel.TransactionDetail.txnData = refNO;
                            fundModel.TransactionDetail.remark = "DealPortal";

                            var response = await fundManger.WalletFunction(fundModel.TransactionDetail);
                            if (response == null || string.IsNullOrEmpty(response.msg) || !response.msg.Contains("success") || string.IsNullOrEmpty(response.voucherno))
                            {
                                return Json("Something went wrong, Please try again later.");
                            }
                            else
                            {
                                // ceck transaction confirmation status
                                fundModel.TransactionDetail.action = "WalletDeductConfirmation";
                                fundModel.TransactionDetail.voucherNo = response.voucherno;


                                var status = await fundManger.WalletFunction(fundModel.TransactionDetail);

                                if (status == null || string.IsNullOrEmpty(status.response) || !(status.response.Contains("OK")))
                                {
                                    return Json("Something went wrong, Please try again later.");
                                }
                                else
                                {
                                    // update 3rd party req. status
                                    fundDetail = new FundRequestContainer();
                                    fundDetail.action = "UPDATE_3RD_PARTY_WALLET_REQUEST";
                                    fundDetail.member_id = userData[1];
                                    fundDetail.ref_no = response.voucherno;
                                    fundDetail.status = "approved";
                                    fundDetail.txn_id = refNO;

                                    fundDetail.txn_type = string.Empty;
                                    fundDetail.amount = (float)(fundModel.TransactionDetail.amount);
                                    fundDetail.remarks = string.Empty;
                                    fundDetail.domain_name = string.Empty;
                                    fundDetail.request_token = string.Empty;

                                    var res = await fundManger.UpdateWalletCreditRequest(fundDetail);
                                    return Json(res);
                                }
                            }
                        }
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

        /// <summary>
        /// Get list of fund request from members
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MemberFundRequest() {

            List<CompanyFund> fundRequestList = new List<CompanyFund>();
            try {
                string[] userData = User.Identity.Name.Split('|');
                fundRequestList = await fundManger.getFundRequestList(userData[1]);
            }
            catch (Exception Ex) {
                Console.WriteLine(Ex.InnerException);
            }
            return View(fundRequestList);
        }
    }
}