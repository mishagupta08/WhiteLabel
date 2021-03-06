﻿using System.Collections.Generic;

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