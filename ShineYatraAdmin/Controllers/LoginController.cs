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
        /// <summary>
        /// Hold company id
        /// </summary>
        private string CompanyId = ConfigurationManager.AppSettings.Get("CompanyId");

        /// <summary>
        /// gets or sets login action
        /// </summary>
        private const string LoginAction = "VALIDATELOGIN";

        /// <summary>
        /// Gets or sets Login Model
        /// </summary>
        LoginModel loginModel = new LoginModel();

        /// <summary>
        /// gets or sets login manager
        /// </summary>
        LoginManager loginManager = new LoginManager();

        // GET: Login
        public async Task<ActionResult> Index(string returnUrl)
        {            
            try
            {
                CommonController commonController = new CommonController();
                               
                await commonController.GetCompanySettings();

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    ViewBag.returnUrl = returnUrl;
                }
                 
                if (Request.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(loginModel);
        }

        /// <summary>
        /// validate user detail
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>validation result</returns>
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

                loginDetail.action = LoginAction;
                loginDetail.domain_name = ConfigurationManager.AppSettings["DomainName"];

                var result = await loginManager.ValidateUser(loginDetail);

                if (result == null)
                {
                    return Json(Resources.LoginError);
                }

                string userIdentity = result.user_name + "|" + result.member_id + "|" + result.company_id+"|"+ result.first_name+" "+ result.last_name+"|"+result.mobileNo +"|"+ result.ledger_id +"|"+result.role_id +"|"+result.emailId;
                FormsAuthentication.SetAuthCookie(userIdentity, false);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(string.Empty);
        }
    }
}