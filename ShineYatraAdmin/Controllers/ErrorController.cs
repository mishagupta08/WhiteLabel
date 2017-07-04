using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShineYatraAdmin.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult page404()
        {
            return View();
        }

        // GET: Error
        public ActionResult sessiontimeout()
        {
            ShineYatraSession.LoginUser = null;
            ShineYatraSession.SelectedMenu = null;
            FormsAuthentication.SignOut();            

            return View();
        }

        // GET: Error
        public ActionResult customError()
        {
            return View();
        }
    }
}