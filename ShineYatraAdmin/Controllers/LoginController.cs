namespace ShineYatraAdmin.Controllers
{
    #region namespace

    using System.Web.Mvc;
    using ShineYatraAdmin.Entity;
    using System.Threading.Tasks;
    using Business;
    using Properties;
    using System.Configuration;
    using System.Web.Security;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion namespace

    /// <summary>
    /// Hold Login Functionality
    /// </summary>
    public class LoginController : Controller
    {                            
        LoginManager loginManager = null; 
        CommonController commonController = null;

        //function to Call login page
        public async Task<ActionResult> Index()
        {
            LoginModel loginModel = new LoginModel();
            commonController = new CommonController();           
            try
            {                                
                await commonController.GetCompanySettings();                
                if (Request.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, "", ConfigurationManager.AppSettings["DomainName"]);
            }
            return View(loginModel);
        }
        
        /// validate user login detail        
        public async Task<ActionResult> ValidateUser(LoginModel loginDetail)
        {
            try
            {
                if (loginDetail == null || string.IsNullOrEmpty(loginDetail.username))
                {
                    return Json("Username is empty.");
                }

                if (string.IsNullOrEmpty(loginDetail.password))
                {
                    return Json("Password is empty.");
                }

                loginManager = new LoginManager();

                loginDetail.action = "VALIDATELOGIN";
                loginDetail.domain_name = ConfigurationManager.AppSettings["DomainName"];

                var result = await loginManager.ValidateUser(loginDetail);

                if (result == null)
                {
                    return Json(Resources.LoginError);
                }
                Session["WalletBalance"] = result.wallet_balance;
                Session["CompanyWalletBalance"] = result.company_wallet_balance;
                string userIdentity = result.user_name + "|" + result.member_id + "|" + result.company_id+"|"+ result.first_name+" "+ result.last_name+"|"+result.mobileNo +"|"+ result.ledger_id +"|"+result.role_id +"|"+result.emailId;
                FormsAuthentication.SetAuthCookie(userIdentity, false);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, "", ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(string.Empty);
        }
    }
}