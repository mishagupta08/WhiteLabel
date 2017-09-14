using System;
using System.Web.Mvc;
using ShineYatraAdmin.Business;
using ShineYatraAdmin.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class RechargeController : Controller
    {

        UserManager userManager;
        RechargeManager rechargeManager;     

        /// <summary>
        /// Get prepaid,postpaid, dth recharge page
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string type)
        {
            userManager = new UserManager();
            RechargeViewModel viewModel = new RechargeViewModel();
            try
            {
                if (!string.IsNullOrEmpty(type))
                {                    
                    viewModel.rechargeType = type;

                    WalletRequest balrequest = new WalletRequest();
                    balrequest.action = "GET_WALLET_BALANCE";
                    balrequest.domain_name = "nbfcp.bisplindia.in";
                    balrequest.ledger_id = "100";
                    balrequest.company_id = "1";
                    WalletResponse bal_response = await userManager.GET_WALLET_BALANCE(balrequest);
                    if (bal_response != null)
                    {
                        viewModel.walletBalance = bal_response.wallet_balance;
                    }
                    if (!string.IsNullOrEmpty(TempData["status"].ToString()))
                    {
                        viewModel.status = TempData["status"].ToString();
                    }
                    else
                    {
                        viewModel.status = "";
                    }
                    
                }
                else
                {
                    return RedirectToAction("Index","Dashboard");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }    
            return View(viewModel);
        }

        /// <summary>
        /// Get service provider list for the selected recharge type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ActionResult> getServiceProviderList(string type)
        {
            List<ServiceDetail> serviceDetail = new List<ServiceDetail>();
            rechargeManager = new RechargeManager();
            try {
                ServicesRequest recharge = new ServicesRequest();
                recharge.type = type;               
                serviceDetail= await rechargeManager.getserviceProviderlist(recharge);                                             
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return Json(serviceDetail);
        }

        /// <summary>
        /// validate the recharge before final transaction occur
        /// </summary>
        /// <param name="frmCollection"></param>
        /// <returns></returns>
        public async Task<ActionResult> ValidateTransaction(FormCollection frmCollection)
        {
            string response = string.Empty;
            ServicesRequest mobileDetails = new ServicesRequest();
            rechargeManager = new RechargeManager();
            try
            {
                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");                
                mobileDetails.agentid = "123456";
                mobileDetails.amount = 10;
                mobileDetails.account = Convert.ToString(frmCollection["account"]);
                mobileDetails.spkey = Convert.ToString(frmCollection["spkey"]);
                string serviceProvider = Convert.ToString(frmCollection["ProviderName"]);
                ValidateTransaction validateTransaction = await rechargeManager.ValidateTransaction(mobileDetails);
                if (validateTransaction != null)
                {
                    if (validateTransaction.ipay_errordesc.ToLower().Equals("transaction successful"))
                    {
                        RechargeDetails details = new RechargeDetails();
                        details.account_no = mobileDetails.account;
                        details.action = "INSERT_SERVICE_RECHARGE_REQUEST_INSTANTPAY";
                        details.member_id = 1;
                        details.request_token = GuidString;
                        details.service_provider = serviceProvider;
                        details.amount = mobileDetails.amount;
                        
                        RechargeDBTxnResponse resp = await rechargeManager.SaveRechargeRequest(details);
                        response = "success";                        
                    }
                    else
                    {
                        response = validateTransaction.ipay_errordesc;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(response);
        }

        /// <summary>
        /// Make final transaction after validation
        /// </summary>
        /// <param name="frmCollection"></param>
        /// <returns></returns>
        public async Task<ActionResult> Transaction(FormCollection frmCollection)
        {
            ServicesRequest mobileDetails = new ServicesRequest();
            rechargeManager = new RechargeManager();
            string response = string.Empty;
            string rechargeType = string.Empty;
            try
            {
                mobileDetails.agentid = "123456";
                mobileDetails.format = "json";
                mobileDetails.agentid = "123456";
                mobileDetails.amount = 10;
                mobileDetails.account = Convert.ToString(frmCollection["account"]);
                mobileDetails.spkey = Convert.ToString(frmCollection["spkey"]);
                
                string serviceProvider = Convert.ToString(frmCollection["ProviderName"]);
                string paymentMode = Convert.ToString(frmCollection["PaymentMode"]);
                double walletBalance = Convert.ToDouble(frmCollection["walletBalance"]);
                rechargeType = Convert.ToString(frmCollection["rechargeType"]);
                mobileDetails.type = rechargeType;
                if (paymentMode == "bank" || (paymentMode == "wallet" && walletBalance < mobileDetails.amount))
                {
                    PayUController cntrl = new PayUController();
                    PayuRequest payrequest = new PayuRequest();
                    payrequest.FirstName = "Misha";
                    payrequest.TransactionAmount = 1;
                    payrequest.Email = "guptamisha88@gmail.com";
                    payrequest.Phone = "8107737208";
                    payrequest.ProductInfo = "Recharge Payment";
                    payrequest.udf1 = new JavaScriptSerializer().Serialize(mobileDetails);
                    payrequest.surl = "http://" + Request.Url.Authority + "/Recharge/Return";
                    payrequest.furl = "http://" + Request.Url.Authority + "/Recharge/Return";
                    cntrl.Payment(payrequest);
                }
                else
                {
                    TransactionStatus transactionResponse = await rechargeManager.Transaction(mobileDetails);
                    if (transactionResponse != null)
                    {
                        if (transactionResponse.status != null && transactionResponse.status.ToLower().Equals("success"))
                        {
                            response = "success";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(transactionResponse.status))
                            {
                                response = transactionResponse.status;
                            }
                            else
                            {
                                response = transactionResponse.ipay_errordesc;
                            }

                        }
                        TempData["status"] = response;
                    }
                }               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return RedirectToAction("Index","Recharge",new {type=rechargeType});
        }

        /// <summary>
        /// Return method after payment transaction
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Return(FormCollection form)
        {            
            string order_id = string.Empty;
            string status = string.Empty;
            ServicesRequest mobileDetails = new ServicesRequest();
            rechargeManager = new RechargeManager();

            try
            {
                if (form["status"].ToString() == "success")
                {                    
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    mobileDetails = (ServicesRequest)serializer.DeserializeObject(form["requestJson"].ToString());
                    
                    TransactionStatus transactionResponse = await rechargeManager.Transaction(mobileDetails);
                    if (transactionResponse != null)
                    {
                        if (transactionResponse.status != null && transactionResponse.status.ToLower().Equals("success"))
                        {
                            status = "success";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(transactionResponse.status))
                            {
                                status = transactionResponse.status;
                            }
                            else
                            {
                                status = transactionResponse.ipay_errordesc;
                            }

                        }
                       
                    }
                }
                else
                {
                    status = "Some problem occured while making your transaction, please try after some time";
                }
                TempData["status"] = status;
            }                                
            catch (Exception ex)
            {
                Response.Write("<span style='color:red'>" + ex.Message + "</span>");
            }
            return RedirectToAction("Index","Recharge", new { type = mobileDetails.type });
        }       
    }
}