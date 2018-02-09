using System.Collections.Generic;

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

            //----View mapping start----

            // check in company Id folder
            var userData = User.Identity.Name.Split('|');
            var path = "~//Views//Dashboard//" + System.Web.HttpContext.Current.Session["CompanyTheme"].ToString().ToLower() + "//" + userData[2] + "//Index.cshtml";
            var serverPath = Server.MapPath(path);
            var isExist = System.IO.File.Exists(serverPath);

            var viewFolder = string.Empty;

            if (isExist)
            {
                viewFolder =  System.Web.HttpContext.Current.Session["CompanyTheme"].ToString().ToLower() + "//" + userData[2] + "//Index";
            }
            else
            {
                // check in Theme folder
                path = "~//Views//Dashboard//" + System.Web.HttpContext.Current.Session["CompanyTheme"].ToString().ToLower() + "//Index.cshtml";
                serverPath = Server.MapPath(path);
                isExist = System.IO.File.Exists(serverPath);
                if (isExist)
                {
                    viewFolder =  System.Web.HttpContext.Current.Session["CompanyTheme"].ToString().ToLower() + "//Index";
                }
                else
                {
                    viewFolder = "Index";
                }
            }

            //----View mapping end----

            //var viewFolder = System.Web.HttpContext.Current.Session["CompanyTheme"].ToString().ToLower() + "//Index";
            return View(viewFolder);
            //return View();
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
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return RedirectToAction("Index", "Login");
        }

  
    }
}