using System;
using System.Web.Mvc;
using ShineYatraAdmin.Business;
using ShineYatraAdmin.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class RechargeController : Controller
    {

        private UserManager _userManager;
        private RechargeManager _rechargeManager;
        private ServiceManager _serviceManager;


        /// <summary>
        /// Get prepaid,postpaid, dth recharge page
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string type)
        {
            _userManager = new UserManager();
            _serviceManager = new ServiceManager();
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
            _serviceManager = new ServiceManager();
            try
            {               
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

                var allflightDiscountDetails = await _serviceManager.GetServiceAllottedGroupDetails(serviceCgDetailsRequest);
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
            var pgManager = new PgManager();
            _serviceManager = new ServiceManager();

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

                var serviceProvider = Convert.ToString(frmCollection["ProviderName"]);
                var paymentMode = Convert.ToString(frmCollection["PaymentMode"]);
                rechargeType = Convert.ToString(frmCollection["rechargeType"]);
                double walletBalance = 0;
                var userData = User.Identity.Name.Split('|');

                 

                var balrequest = new WalletRequest
                {
                    action = "GET_WALLET_BALANCE",
                    domain_name = ConfigurationManager.AppSettings["DomainName"],
                    member_id = userData[1],
                    company_id = userData[2]
                };
                                
                mobileDetails.type = rechargeType;
                var g = Guid.NewGuid();
                var guidString = Convert.ToBase64String(g.ToByteArray());
                guidString = guidString.Replace("=", "");
                guidString = guidString.Replace("+", "");

                var mobileDetailsJson = new JavaScriptSerializer().Serialize(mobileDetails);
                var cookie = new HttpCookie("Cookie-" + guidString, mobileDetailsJson)
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                HttpContext.Response.Cookies.Add(cookie);
                var isPaymentGatewayactive = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPaymentGatewayactive"]);

                var balResponse = await _userManager.GET_WALLET_BALANCE(balrequest);
                if (balResponse != null)
                {
                    walletBalance = balResponse.wallet_balance;
                }

                if (isPaymentGatewayactive && (paymentMode == "bank" || (paymentMode == "wallet" && walletBalance < mobileDetails.amount)))
                {                    
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
                    mobileDetails.pg_amount = paymentDetail.amount;
                    var balanceTxnId = await pgManager.SavePaymntGatewayTransactions(paymentDetail);

                    if (!balanceTxnId.ToLower().Contains("failed"))
                    {
                        var cntrl = new PayUController();
                        var payrequest = new PayuRequest
                        {
                            FirstName = "Misha",
                            TransactionAmount = paymentDetail.amount,
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
                        return RedirectToAction("RechargeStatus", "Recharge", new { txnId = result, info = guidString });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }                
            return RedirectToAction("RechargeStatus", "Recharge", new {txnId = "", info = "" });
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
            _serviceManager = new ServiceManager();
            var mobileDetail = new ServicesRequest();

            try
            {
                
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
                        return RedirectToAction("RechargeStatus", "Recharge", new { txnId = saveBookingResonse, info = cookieId });
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return RedirectToAction("RechargeStatus", "Recharge", new { txnId = "", info = "" });
        }

        public async Task<ActionResult> RechargeStatus(string txnId, string info)
        {
            var status = "Failed";
            var userData = User.Identity.Name.Split('|');
            TransactionStatus transactionResponse = null;            
            try
            {
                if (string.IsNullOrEmpty(txnId))
                {
                    TempData["RechargeStatus"] = "Failed";
                    return View();
                }
                
                var mobileDetail = new ServicesRequest();
                var myCookie = Request.Cookies["Cookie-" + info];

                // Read the cookie information and display it.
                if (myCookie != null)
                {
                    var serializer = new JavaScriptSerializer();
                    mobileDetail = serializer.Deserialize<ServicesRequest>(myCookie.Value);
                }
                _rechargeManager = new RechargeManager();
                transactionResponse = await _rechargeManager.Transaction(mobileDetail);
                if (transactionResponse != null)
                {
                    _serviceManager = new ServiceManager();
                    if (transactionResponse.status != null &&
                        transactionResponse.status.ToLower().Equals("success"))
                    {
                        status = "success";
                        var updatestatus =
                            await _serviceManager.UpdateServiceBookingRequest(txnId, userData[1],transactionResponse.ipay_id, "COMPLETED");
                        TempData["RechargeStatus"] = "Success";
                    }
                    else
                    {
                        status = !string.IsNullOrEmpty(transactionResponse.status) ? transactionResponse.status : transactionResponse.ipay_errordesc;
                        var updatestatus =
                            await _serviceManager.UpdateServiceBookingRequest(txnId, userData[1],"", "Failed");
                        TempData["RechargeStatus"] = "Failed";
                    }
                }            
            }
            catch (Exception ex)
            {
                throw ex;
            }
             return View(transactionResponse);
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

                var serviceCgDetailsRequest = new AllotedServiceCGsDetailsRequest
                {
                    member_id = userData[1],
                    action = "GET_ALLOTED_SERVICE_COMMISSION_GROUPS_DETAILS",
                    service_id = "4",
                    sub_service_id = "0",
                    category = "Recharge",
                    sub_category = mobileDetails.type,
                    service_code = "0"
                };
                int ssid = Convert.ToInt16(mobileDetails.spkey);

                var allflightDiscountDetailsList = await _serviceManager.GetServiceAllottedGroupDetails(serviceCgDetailsRequest);
                var flightDiscountDetails = allflightDiscountDetailsList.FirstOrDefault(o => o.sub_service_id== ssid);
                if (flightDiscountDetails != null)
                {
                    mobileDetails.discount = (flightDiscountDetails.SELF_COMM_PER * mobileDetails.amount) / 100;
                }

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
                    my_info = myInfo,
                    other_amount = 0,
                    total_paid_amount = 0,
                    pg_amount = mobileDetails.pg_amount,
                    discount = mobileDetails.discount
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