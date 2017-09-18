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
    using System.Collections.Generic;

    #endregion namespace
    [Authorize]
    public class CompanyController : Controller
    {
        /// <summary>
        /// object to  hold Comapny details
        /// </summary>
        Company companyModel;

        /// <summary>
        /// object for access Comapny functions
        /// </summary>
        CompanyManager companyManager = new CompanyManager();

        /// <summary>
        /// object for access service functions
        /// </summary>
        ServiceManager serviceManager = new ServiceManager();

        /// <summary>
        /// object for access Company functions
        /// </summary>
        CompanyViewModel companyViewModel;

        /// <summary>
        /// constant for active status
        /// </summary>
        private const string Active = "Y";


        /// <summary>
        /// method to get selected menu
        /// </summary>
        /// <param name="menu"></param>
        /// <returns>selected menu view</returns>
        public async Task<ActionResult> GetSelectedMenu(string menu, bool isRefresh, bool isBack, string sortColumn, string sortOrder)
        {
            this.companyViewModel = new CompanyViewModel();
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                companyViewModel.LoginUserName = userData[0];
                this.companyViewModel.SelectedMenu = menu;
                this.companyViewModel.SearchListParameter = new SearchParameter();
                this.companyViewModel.AssignSearchList();
                
                this.companyViewModel.CompanyList = await this.companyManager.GetCompany(userData[1], string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View("Index", this.companyViewModel);
        }

        /**************Company Menu function* Start *************/

        /// <summary>
        /// Method to get company list
        /// </summary>
        /// <returns></returns>
        private async Task GetCompanyListByPageIndex(int pageIndex)
        {

            try
            {
                string[] userData = User.Identity.Name.Split('|');
                this.companyViewModel.CompanyList = await this.companyManager.GetCompany(userData[1], string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }    

        /// <summary>
        /// method to get insert company view
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetInsertCompanyView(string companyId)
        {
            this.companyViewModel = new CompanyViewModel();
            try
            {
                this.companyViewModel.AssignCountryList();
                if (!string.IsNullOrEmpty(companyId) && !companyId.Equals("0"))
                {
                    string[] userData = User.Identity.Name.Split('|');
                    this.companyViewModel.CompanyList = await this.companyManager.GetCompany(userData[1], companyId);
                }
                if (this.companyViewModel.CompanyList != null && this.companyViewModel.CompanyList.Count > 0)
                {
                    this.companyViewModel.CompanyDetail = this.companyViewModel.CompanyList.FirstOrDefault();
                }

            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            if (string.IsNullOrEmpty(companyId) || companyId.Equals("0"))
            {
                return PartialView("ManageCompany\\addCompany", this.companyViewModel);
            }
            else
            {
                return PartialView("ManageCompany\\editCompany", this.companyViewModel);
            }
        }

        /// <summary>
        /// method to add edit company
        /// </summary>
        /// <param name="companyDashboard"></param>
        /// <returns></returns>
        public async Task<ActionResult> AddEditCompany(CompanyViewModel companyDashboard)
        {
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                if (companyDashboard == null || companyDashboard.CompanyDetail == null)
                {
                    return Json(string.Empty);
                }

                //if (string.IsNullOrEmpty(companyDashboard.CompanyDetail.company_id))
                if (companyDashboard.CompanyDetail.company_id == 0)
                {
                    companyDashboard.CompanyDetail.action = "InsertCompany";
                    companyDashboard.CompanyDetail.add_user_id = userData[1];
                    companyDashboard.CompanyDetail.member_id = userData[1];
                }
                else
                {
                    companyDashboard.CompanyDetail.action = "UpdateCompany";
                    companyDashboard.CompanyDetail.update_user_id = userData[1];
                }

                companyDashboard.CompanyDetail.active_status = Active;
                companyDashboard.CompanyDetail.member_id = userData[1];
                var result = await this.companyManager.AddEditCompany(companyDashboard.CompanyDetail);

                if (result == null)
                {
                    return Json(string.Empty);
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(string.Empty);
        }

        /// <summary>
        /// method to get edit company setting view
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetEditCompanySettingView(string companySettingId, string companyName)
        {
            this.companyViewModel = new CompanyViewModel();
            this.companyViewModel.AssignSettingList();
            this.companyViewModel.CompanyName = companyName;
            try
            {
                var setting = await this.companyManager.GetCompanySetting(companySettingId);
                if (setting != null)
                {
                    this.companyViewModel.ApiSetting = new CompanyApiSetting();
                    this.companyViewModel.ApiSetting.company_id = setting.company_id;
                    this.companyViewModel.ApiSetting.app_ewallet_api_enabled = setting.app_ewallet_api_enabled;
                    this.companyViewModel.ApiSetting.app_login_api_enabled = setting.app_login_api_enabled;
                    this.companyViewModel.ApiSetting.cmp_setting_id = setting.cmp_setting_id;
                    this.companyViewModel.ApiSetting.app_pg_api_enabled = setting.app_pg_api_enabled;
                    this.companyViewModel.ApiSetting.web_ewallet_api_enabled = setting.web_ewallet_api_enabled;
                    this.companyViewModel.ApiSetting.web_login_api_enabled = setting.web_login_api_enabled;
                    this.companyViewModel.ApiSetting.web_pg_api_enabled = setting.web_pg_api_enabled;

                    this.companyViewModel.SmsSetting = new CompanySmsSetting();
                    this.companyViewModel.SmsSetting.company_id = setting.company_id;
                    this.companyViewModel.SmsSetting.sms_api_integrated = setting.sms_api_integrated;
                    this.companyViewModel.SmsSetting.sms_api_password = setting.sms_api_password;
                    this.companyViewModel.SmsSetting.sms_api_sender_id = setting.sms_api_sender_id;
                    this.companyViewModel.SmsSetting.sms_api_url = setting.sms_api_url;
                    this.companyViewModel.SmsSetting.sms_api_username = setting.sms_api_username;
                    this.companyViewModel.SmsSetting.cmp_setting_id = setting.cmp_setting_id;

                    this.companyViewModel.EmailSetting = new CompanyEmailSetting();
                    this.companyViewModel.EmailSetting.company_id = setting.company_id;
                    this.companyViewModel.EmailSetting.cmp_setting_id = setting.cmp_setting_id;
                    this.companyViewModel.EmailSetting.email_id = setting.email_id;
                    this.companyViewModel.EmailSetting.email_password = setting.email_password;

                    this.companyViewModel.OtpSetting = new CompanyOtpSetting();
                    this.companyViewModel.OtpSetting.company_id = setting.company_id;
                    this.companyViewModel.OtpSetting.cmp_setting_id = setting.cmp_setting_id;
                    this.companyViewModel.OtpSetting.otp_login_enabled = setting.otp_login_enabled;
                    this.companyViewModel.OtpSetting.otp_login_service = setting.otp_login_service;

                    this.companyViewModel.CommissionSetting = new CompanyCommissionSetting();
                    this.companyViewModel.CommissionSetting.company_id = setting.company_id;
                    this.companyViewModel.CommissionSetting.cmp_setting_id = setting.cmp_setting_id;
                    this.companyViewModel.CommissionSetting.postpaid_margin = setting.master_postpaid_margin;
                    this.companyViewModel.CommissionSetting.prepaid_margin = setting.master_prepaid_margin;
                    this.companyViewModel.CommissionSetting.dth_margin = setting.master_dth_margin;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return PartialView("ManageCompany\\companySetting", this.companyViewModel);
        }


        /// <summary>
        /// method to submit company setting
        /// </summary>
        /// <param name="settingModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> SubmitCompanySetting(CompanyViewModel settingModel)
        {
            try
            {
                if (settingModel == null)
                {
                    return null;
                }

                var result = await this.companyManager.SaveCompanySetting(settingModel);
                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(string.Empty);
        }

        //public ActionResult GetAddFundView(string companyId, string companyName)
        //{
        //    this.dashboardModel = new DashboardModel();
        //    this.dashboardModel.CompanyName = companyName;
        //    return PartialView("ManageCompany\\addFund", this.dashboardModel);
        //}

        //public async Task<ActionResult> SaveFundDetail(DashboardModel fundModel)
        //{
        //    var response = await this.dashboardManager.SaveAddFundDetail(fundModel.CompanyName);
        //    return Json("ManageCompany\\addFund");
        //}

        /**************Company Menu function* End *************/


        /// <summary>
        /// method to get company recharge commission structure
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult> GetRechargeCommissionStructure(string companyId)
        {
            this.companyModel = new Company();
            try
            {
                this.companyModel.commissionstucture = await this.companyManager.GetRechargeCommissionStructure(companyId);
                return PartialView("_structure", this.companyModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return PartialView("_structure", this.companyModel);
        }

        /// <summary>
        /// method to get recharge commission information
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetRechargeCompanySettingView(string comp_seting_id)
        {

            CompanyCommissionSetting CompanyCommissionSetting = new CompanyCommissionSetting();
            try
            {
                var setting = await this.companyManager.GetCompanySetting(comp_seting_id);
                if (setting != null)
                {
                    CompanyCommissionSetting.company_id = setting.company_id;
                    CompanyCommissionSetting.cmp_setting_id = setting.cmp_setting_id;
                    CompanyCommissionSetting.postpaid_margin = setting.master_postpaid_margin;
                    CompanyCommissionSetting.prepaid_margin = setting.master_prepaid_margin;
                    CompanyCommissionSetting.dth_margin = setting.master_dth_margin;
                }

                return PartialView("RechargeCommissionPercentage", CompanyCommissionSetting);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return PartialView("RechargeCommissionPercentage", CompanyCommissionSetting);
        }

        /// <summary>
        /// method to submit company recharge setting
        /// </summary>
        /// <param name="settingModel"></param>
        /// <returns></returns>
        public async Task<ActionResult> SubmitCompanyRechargeSetting(CompanyCommissionSetting settingModel)
        {
            try
            {
                if (settingModel == null)
                {
                    return null;
                }
                companyViewModel = new CompanyViewModel();
                companyViewModel.CommissionSetting = settingModel;
                var result = await this.companyManager.SaveCompanySetting(companyViewModel);
                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return null;
        }
    }
}