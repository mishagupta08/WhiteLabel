﻿using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ShineYatraAdmin.Business;
using ShineYatraAdmin.Entity;

namespace ShineYatraAdmin.Controllers
{
    public class CommonController : Controller
    {
        public async Task<ActionResult> GetUserBalance()
        {
            var userManager = new UserManager();
            float balance = 0;
            try
            {
                var balrequest = new WalletRequest();
                var userData = User.Identity.Name.Split('|');
                balrequest.action = "GET_WALLET_BALANCE";
                balrequest.domain_name = ConfigurationManager.AppSettings["DomainName"];
                balrequest.member_id = userData[1];
                balrequest.company_id = userData[2];
                var balResponse = await userManager.GET_WALLET_BALANCE(balrequest);
                if (balResponse != null)
                {
                    balance = balResponse.wallet_balance;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(balance);
        }

        public async Task<ActionResult> GetCompanyThemeAndSetting()
        {            
            try
            {
                if (System.Web.HttpContext.Current.Session["CompanyTheme"] != null)
                {
                }
                else
                {
                    await GetCompanyTheme();
                }
                if (System.Web.HttpContext.Current.Session["web_pg_api_enabled"] != null)
                {
                }
                else
                {
                    await GetCompanySettings();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(true);
        }

        public async Task<Boolean> GetCompanyTheme()
        {
            try
            {
                var theme = new ThemeManager();
                var domain = ConfigurationManager.AppSettings["DomainName"];                
               
                    var ltheme = await theme.GetCompanyTheme(domain);
                    if (ltheme != null)
                        System.Web.HttpContext.Current.Session["CompanyTheme"] = (from r in ltheme select r.theme_name).FirstOrDefault();
                    if (string.IsNullOrEmpty(Convert.ToString(System.Web.HttpContext.Current.Session["CompanyTheme"])))
                    {
                        System.Web.HttpContext.Current.Session["CompanyTheme"] = "elite";
                    }                              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return true;
        }

        public async Task<Boolean> GetCompanySettings()
        {
            try
            {
                var companyManager = new CompanyManager();
                var domain = ConfigurationManager.AppSettings["DomainName"];
               
                    var setting = await companyManager.GetCompanyExtraSetting(domain);
                    if (setting != null)
                    {
                        System.Web.HttpContext.Current.Session["web_pg_api_enabled"] = setting.web_pg_api_enabled;
                        System.Web.HttpContext.Current.Session["otp_login_enabled"] = setting.otp_login_enabled;
                        System.Web.HttpContext.Current.Session["otp_service_enabled"] = setting.otp_login_service;
                    }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return true;
        }
    }
}