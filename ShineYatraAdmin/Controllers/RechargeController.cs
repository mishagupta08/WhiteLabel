using System;
using System.Web.Mvc;
using ShineYatraAdmin.Business;
using ShineYatraAdmin.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Web;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class RechargeController : Controller
    {

        UserManager _userManager;
        RechargeManager _rechargeManager;



        /// <summary>
        /// Get prepaid,postpaid, dth recharge page
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string type)
        {
            _userManager = new UserManager();
            ServiceManager serviceManager = new ServiceManager();
            RechargeViewModel viewModel = new RechargeViewModel();
            try
            {
                if (!string.IsNullOrEmpty(type))
                {
                    viewModel.rechargeType = type;

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
                    return RedirectToAction("Index", "Dashboard");
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
            List<SelectListItem> serviceDetail = new List<SelectListItem>();
            _rechargeManager = new RechargeManager();
            try
            {
                //ServicesRequest recharge = new ServicesRequest();
                var serviceManager = new ServiceManager();
                var userData = User.Identity.Name.Split('|');
                var serviceCgDetailsRequest = new AllotedServiceCGsDetailsRequest
                {
                    member_id = userData[1],
                    action = "GET_ALLOTED_SERVICE_COMMISSION_GROUPS_DETAILS",
                    service_id = "4",
                    sub_service_id = "0",
                    category = "Recharge",
                    sub_category = type,
                    service_code = "0"
                };

                var allflightDiscountDetails =
                    await serviceManager.GetServiceAllottedGroupDetails(serviceCgDetailsRequest);
                foreach (var record in allflightDiscountDetails)
                {
                    serviceDetail.Add(new SelectListItem
                    {
                        Value = record.sub_service_id.ToString(),
                        Text = record.sub_service_name
                    });
                }
                //recharge.type = type;               
                //serviceDetail= await _rechargeManager.getserviceProviderlist(recharge);                                             
            }
            catch (Exception ex)
            {
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
            var response = string.Empty;
            var mobileDetails = new ServicesRequest();
            _rechargeManager = new RechargeManager();
            try
            {               
                mobileDetails.agentid = "123456";
                mobileDetails.amount = 10;
                mobileDetails.account = Convert.ToString(frmCollection["account"]);
                mobileDetails.spkey = Convert.ToString(frmCollection["spkey"]);
                var validateTransaction = await _rechargeManager.ValidateTransaction(mobileDetails);
                if (validateTransaction != null)
                {
                    if (validateTransaction.ipay_errordesc.ToLower().Equals("transaction successful"))
                    {
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
            var mobileDetails = new ServicesRequest();
            var flightmanager = new FlightManager();

            _rechargeManager = new RechargeManager();
            _userManager = new UserManager();
            var response = string.Empty;
            var rechargeType = string.Empty;
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
                rechargeType = Convert.ToString(frmCollection["rechargeType"]);

                WalletRequest balrequest = new WalletRequest();
                double walletBalance = 0;
                string[] userData = User.Identity.Name.Split('|');
                balrequest.action = "GET_WALLET_BALANCE";
                balrequest.domain_name = ConfigurationManager.AppSettings["DomainName"];
                balrequest.member_id = userData[1];
                balrequest.company_id = userData[2];
                WalletResponse balResponse = await _userManager.GET_WALLET_BALANCE(balrequest);
                if (balResponse != null)
                {
                    walletBalance = balResponse.wallet_balance;
                }

                mobileDetails.type = rechargeType;
                bool isPaymentGatewayactive = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPaymentGatewayactive"]);
                if (isPaymentGatewayactive && (paymentMode == "bank" || (paymentMode == "wallet" && walletBalance < mobileDetails.amount)))
                {
                    var g = Guid.NewGuid();
                    var guidString = Convert.ToBase64String(g.ToByteArray());
                    guidString = guidString.Replace("=", "");
                    guidString = guidString.Replace("+", "");

                    string mobileDetailsJson = new JavaScriptSerializer().Serialize(mobileDetails);
                    var cookie = new HttpCookie("Cookie-" + guidString, mobileDetailsJson)
                    {
                        Expires = DateTime.Now.AddYears(1)
                    };
                    HttpContext.Response.Cookies.Add(cookie);


                    var paymentDetail = new CompanyFund
                    {
                        action = "INSERT_PG_REQUEST_FOR_SERVICE",
                        member_id = userData[1],
                        domain_name = ConfigurationManager.AppSettings["DomainName"],
                        request_token = guidString,
                        txn_type = "PG_REQUEST",
                        deposit_mode = "PG",
                        remarks = "Recharge payment by payment gateway",
                        amount = paymentMode == "bank" ? mobileDetails.amount : mobileDetails.amount - walletBalance
                    };

                    var balanceTxnId = await flightmanager.SavePaymntGatewayTransactions(paymentDetail);

                    if (!balanceTxnId.ToLower().Contains("failed"))
                    {
                        PayUController cntrl = new PayUController();
                        PayuRequest payrequest = new PayuRequest
                        {
                            FirstName = "Misha",
                            TransactionAmount = 1,
                            Email = "guptamisha88@gmail.com",
                            Phone = "8107737208",
                            ProductInfo = "Recharge Payment",
                            udf1 = Convert.ToString(guidString),
                            udf2 = Convert.ToString(balanceTxnId),
                            surl = "http://" + Request.Url.Authority + "/Recharge/Return",
                            furl = "http://" + Request.Url.Authority + "/Recharge/Return"
                        };
                        cntrl.Payment(payrequest);
                    }
                }
                else
                {
                    var result = await SaveRechargeDetailsToDb(mobileDetails, "");

                    if (result != "error")
                    {
                        TransactionStatus transactionResponse = await _rechargeManager.Transaction(mobileDetails);
                        if (transactionResponse != null)
                        {
                            if (transactionResponse.status != null && transactionResponse.status.ToLower().Equals("success"))
                            {
                                response = "success";
                                UPDATE_TRANSACTION_STATUS updatestatus = await flightmanager.UpdateServiceBookingRequest(result, userData[1], transactionResponse.ipay_id, "COMPLETED");
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
                                UPDATE_TRANSACTION_STATUS updatestatus = await flightmanager.UpdateServiceBookingRequest(result, userData[1], transactionResponse.ipay_id, "Failed");
                            }
                            TempData["status"] = response;
                        }
                    }
                    else
                    {
                        TempData["status"] = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return RedirectToAction("Index", "Recharge", new { type = rechargeType });
        }





        /// <summary>
        /// Return method after payment transaction
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Return(FormCollection form)
        {
            var status = string.Empty;
            _rechargeManager = new RechargeManager();
            var flightManager = new FlightManager();
            var mobileDetail = new ServicesRequest();

            try
            {
                var userData = User.Identity.Name.Split('|');
                var cookieId = form["udf1"];
                var balanceTxId = form["udf2"];
                //getbooking                 
                var myCookie = Request.Cookies["Cookie-" + cookieId];

                // Read the cookie information and display it.
                if (myCookie != null)
                {
                    var serializer = new JavaScriptSerializer();
                    mobileDetail = serializer.Deserialize<ServicesRequest>(myCookie.Value);
                }

                if (form["status"] == "success")
                {
                    var myInfo = "PG_REQUEST," + balanceTxId + "," + Convert.ToString(form["mode"]) + "," +
                                 Convert.ToString(form["txnid"]) + "," + Convert.ToString(form["bank_ref_num"]);
                    var saveBookingResonse = await SaveRechargeDetailsToDb(mobileDetail, myInfo);

                    if (saveBookingResonse != "error")
                    {
                        var serializer = new JavaScriptSerializer();
                        var transactionResponse = await _rechargeManager.Transaction(mobileDetail);
                        if (transactionResponse != null)
                        {
                            if (transactionResponse.status != null &&
                                transactionResponse.status.ToLower().Equals("success"))
                            {
                                status = "success";
                                var updatestatus =
                                    await flightManager.UpdateServiceBookingRequest(saveBookingResonse, userData[1],
                                        transactionResponse.ipay_id, "COMPLETED");

                            }
                            else
                            {
                                status = !string.IsNullOrEmpty(transactionResponse.status) ? transactionResponse.status : transactionResponse.ipay_errordesc;
                                var updatestatus =
                                    await flightManager.UpdateServiceBookingRequest(saveBookingResonse, userData[1],
                                        transactionResponse.ipay_id, "Failed");
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
            return RedirectToAction("Index", "Recharge", new { type = mobileDetail.type });
        }

        private async Task<string> SaveRechargeDetailsToDb(ServicesRequest mobileDetails, string myInfo)
        {
            string result = string.Empty;
            _rechargeManager = new RechargeManager();
            try
            {
                var userData = User.Identity.Name.Split('|');
                var domain = ConfigurationManager.AppSettings["DomainName"];
                var g = Guid.NewGuid();
                var guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");

                var rechargedetails = new InsertServiceRechargeRequest
                {
                    action = "INSERT_SERVICE_RECHARGE_REQUEST",
                    domain_name = domain,
                    request_token = guidString,
                    txn_type = "RECHARGE",
                    category = "PREPAID",
                    member_id = userData[1],
                    amount = mobileDetails.amount,
                    service_id = "4",
                    sub_service_id = mobileDetails.spkey,
                    account_number = mobileDetails.account,
                    std_code = "",
                    recharge_number = mobileDetails.account,
                    circle_name = "",
                    remarks = "Mobile Recharge",
                    my_info = myInfo
                };
                var rechargeResponse = await _rechargeManager.SaveRechargeRequest(rechargedetails);
                if (rechargeResponse != null && !string.IsNullOrEmpty(rechargeResponse.txn_id))
                {
                    result = rechargeResponse.txn_id;
                }
                else
                {
                    result = "error";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (result);

        }

    }

}