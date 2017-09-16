namespace ShineYatraAdmin.Controllers
{
    #region namespace

    using Entity;
    using Properties;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Linq;
    using Business;
    using System;
    using System.Web.Security;
    using System.Configuration;

    #endregion namespace

    /// <summary>
    /// Holds dahboard functionality
    /// </summary>
    /// 
    [Authorize]
    public class DashboardController : Controller
    {              
        
        // GET: Dashboard
        public ActionResult Index()
        {
            try {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View();
        }

        public async Task<ActionResult> GetUserBalance()
        {
            UserManager userManager = new UserManager();
            float Balance = 0;
            try
            {
                WalletRequest balrequest = new WalletRequest();
                string[] userData = User.Identity.Name.Split('|');
                balrequest.action = "GET_WALLET_BALANCE";
                balrequest.domain_name = ConfigurationManager.AppSettings["DomainName"];
                balrequest.ledger_id = userData[4];
                balrequest.company_id = userData[2];
                WalletResponse bal_response = await userManager.GET_WALLET_BALANCE(balrequest);
                if (bal_response != null)
                {
                    Balance = bal_response.wallet_balance;
                }
             }
            catch(Exception Ex){
                Console.WriteLine(Ex.InnerException);
            }
            return Json(Balance);
        }

        /// <summary>
        /// Method for signout
        /// </summary>
        public ActionResult LogOut()
        {
            try
            {               
                FormsAuthentication.SignOut();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return RedirectToAction("Index", "Login");
        }

  
    }
}