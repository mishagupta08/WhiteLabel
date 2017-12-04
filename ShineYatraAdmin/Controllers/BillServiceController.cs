using ShineYatraAdmin.Business;
using ShineYatraAdmin.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class BillServiceController : Controller
    {
        // GET: BillService
        PrimarySettingManager primarySettingManager;

        public async Task<ActionResult> Electricity()
        {
            var viewModel = new BillServiceViewModel();
            primarySettingManager = new PrimarySettingManager();
            try
            {
                viewModel.service = "electricity";
                viewModel.serviceProviders = await primarySettingManager.GetPrimarySetting("6", "electricity", User.Identity.Name);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Insurance()
        {
            var viewModel = new BillServiceViewModel();
            primarySettingManager = new PrimarySettingManager();
            try
            {
                viewModel.service = "Insurance Premium";
                viewModel.serviceProviders = await primarySettingManager.GetPrimarySetting("6", "Insurance Premium", User.Identity.Name);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return View(viewModel);
        }

        public async Task<ActionResult> CreditCard()
        {
            var viewModel = new BillServiceViewModel();
            primarySettingManager = new PrimarySettingManager();
            try
            {
                viewModel.service = "Credit Card";
                viewModel.serviceProviders = await primarySettingManager.GetPrimarySetting("6", "Credit Card", User.Identity.Name);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Gas()
        {
            var viewModel = new BillServiceViewModel();
            primarySettingManager = new PrimarySettingManager();
            try
            {
                viewModel.service = "Gas";
                viewModel.serviceProviders = await primarySettingManager.GetPrimarySetting("6", "Gas", User.Identity.Name);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return View(viewModel);
        }

        public async Task<ActionResult> LandLine()
        {
            var viewModel = new BillServiceViewModel();
            primarySettingManager = new PrimarySettingManager();
            try
            {
                viewModel.service = "landline";
                viewModel.serviceProviders = await primarySettingManager.GetPrimarySetting("6", "landline", User.Identity.Name);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Internet()
        {
            var viewModel = new BillServiceViewModel();
            primarySettingManager = new PrimarySettingManager();
            try
            {
                viewModel.service = "Internet ISP";
                viewModel.serviceProviders = await primarySettingManager.GetPrimarySetting("6", "Internet ISP", User.Identity.Name);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return View(viewModel);
        }

        /// <summary>
        // GET: Bill PrimarySetting page details
        /// </summary>       
        /// <returns></returns>
        public async Task<ActionResult> GetBillServiceProvidersFields(string selectedBillService, string selectedBillServiceprovider)
        {
            primarySettingManager = new PrimarySettingManager();
            var fieldListModel = new BillServiceFieldViewModel();
            try
            {
                fieldListModel.fieldList = await this.primarySettingManager.GetBillServiceFields("6", selectedBillServiceprovider, User.Identity.Name);
                fieldListModel.ZoneList = await GetDistrictList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return PartialView(fieldListModel);
        }

        /// <summary>
        // GET: Bill PrimarySetting page details
        /// </summary>       
        /// <returns></returns>
        public async Task<List<ZoneList>> GetDistrictList()
        {
            List<ZoneList> zoneList = null;
            primarySettingManager = new PrimarySettingManager();
            try
            {
                zoneList = await this.primarySettingManager.GetZoneList(User.Identity.Name);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return zoneList;
        }

        public async Task<ActionResult> GetSubZoneList(string zoneid)
        {
            List<SubZoneList> zoneList = null;
            primarySettingManager = new PrimarySettingManager();
            try
            {
                zoneList = await this.primarySettingManager.GetSubZoneList(zoneid, User.Identity.Name);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }



    }
}