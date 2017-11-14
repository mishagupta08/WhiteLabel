using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Business;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Controllers
{
    [Authorize]
    public class PrimarySettingController : Controller
    {
        PrimarySetting primarySetting;
        PrimarySettingManager primarySettingManager = new PrimarySettingManager();        

        /// <summary>
        // GET: Airline PrimarySetting page details
        /// </summary>       
        /// <returns></returns>
        //public async Task<ActionResult> master_airline_structure()
        //{
        //    primarySetting = new PrimarySetting();
        //    try
        //    {
        //        string[] userData = User.Identity.Name.Split('|');
        //        primarySetting.structure = await this.primarySettingManager.GetPrimarySetting("1", "","");
        //        primarySetting.memberid = userData[1];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);

        //    }
        //    return View(primarySetting);
        //}

        ///// <summary>
        //// GET: Bill PrimarySetting page details
        ///// </summary>       
        ///// <returns></returns>
        //public async Task<ActionResult> master_bill_structure()
        //{
        //    primarySetting = new PrimarySetting();
        //    try
        //    {
        //        primarySetting.bill_services = await this.primarySettingManager.GetSubServiceList("6");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);

        //    }
        //    return View(primarySetting);
        //}
       
        ///// <summary>
        //// GET: Bill PrimarySetting page details
        ///// </summary>       
        ///// <returns></returns>
        //public async Task<ActionResult> GetServiceSructure(string selectedBillService)
        //{
        //    primarySetting = new PrimarySetting();
        //    try
        //    {
        //        primarySetting.structure = await this.primarySettingManager.GetPrimarySetting("6", selectedBillService,"");                
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);

        //    }
        //    return PartialView(primarySetting);
        //}
               
        ///// <summary>
        //// GET: Recharge PrimarySetting page details
        ///// </summary>       
        ///// <returns></returns>

        //public async Task<ActionResult> master_recharge_structure()
        //{
        //    primarySetting = new PrimarySetting();
        //    try
        //    {
        //        primarySetting.structure = await primarySettingManager.GetPrimarySetting("4", "","");
        //        primarySetting.bill_services = new List<SubCategory>();
        //        primarySetting.defaultRechargeSetting = await primarySettingManager.GetRechargeDefaultSetting();
        //        string[] userData = User.Identity.Name.Split('|');
        //        primarySetting.memberid = userData[1];
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);
        //    }
        //    return View(primarySetting);
        //}


        //public async Task<ActionResult> DefaultRechargeSetting()
        //{
        //    primarySetting = new PrimarySetting();
        //    try
        //    {
        //        primarySetting.structure = new List<CompanyCommissionGroup>();
        //        primarySetting.bill_services = new List<SubCategory>();
        //        primarySetting.defaultRechargeSetting = await primarySettingManager.GetRechargeDefaultSetting();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);
        //    }
        //    return PartialView(primarySetting);
        //}

        ///// <summary>
        ///// method to save recharge defaut settings
        ///// </summary>        
        ///// <returns></returns>
        //public async Task<ActionResult> SaveRechargeDefaultSetting(DefaultRechargeSetting defautSettings)
        //{
        //    try
        //    {
        //        var result = await primarySettingManager.SaveRechargeDefaultSetting(defautSettings);
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);
        //    }
        //    return Json(null);
        //}        

        ///// <summary>
        ///// method to update margin for airline, bus and hotel
        ///// </summary>        
        ///// <returns></returns>
        //public async Task<ActionResult> UpdatePrimaryMargin(PrimarySettingMargin primaryMargin)
        //{
        //    try
        //    {
        //        var result = await primarySettingManager.UpdatePrimaryMargin(primaryMargin);
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);
        //    }
        //    return Json(null);
        //}        
    }
}