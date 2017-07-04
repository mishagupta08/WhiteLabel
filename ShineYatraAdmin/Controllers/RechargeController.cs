using System;
using System.Web.Mvc;
using ShineYatraAdmin.Business;
using ShineYatraAdmin.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ShineYatraAdmin.Properties;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class RechargeController : Controller
    {

        UserManager userManager = new UserManager();        
        CompanyManager companyManager = new CompanyManager();
        ServiceManager serviceManager = new ServiceManager();
        Service serviceModel;
        UserViewModel userViewModel;

        public ActionResult prepaid()
        {
            RechargeRequest request = new RechargeRequest();
            return View(request);
        }

        public ActionResult postpaid()
        {
            return View();
        }

        public ActionResult dth()
        {
            return View();
        }


    }
}