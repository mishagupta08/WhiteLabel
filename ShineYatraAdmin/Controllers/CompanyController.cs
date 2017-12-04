namespace ShineYatraAdmin.Controllers
{

    #region namespace

    using Entity;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Linq;
    using Business;
    using System;
    using System.Configuration;

    #endregion namespace
    [Authorize]
    public class CompanyController : Controller
    {
        /// <summary>
        /// object to  hold Comapny details
        /// </summary>
        Company _companyModel;

        /// <summary>
        /// object for access Comapny functions
        /// </summary>
        private CompanyManager _companyManager = new CompanyManager();        

        /// <summary>
        /// object for access Company functions
        /// </summary>
        CompanyViewModel _companyViewModel;

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
            _companyViewModel = new CompanyViewModel();
            try
            {
                var userData = User.Identity.Name.Split('|');
                _companyViewModel.LoginUserName = userData[0];
                _companyViewModel.SelectedMenu = menu;
                _companyViewModel.SearchListParameter = new SearchParameter();
                _companyViewModel.AssignSearchList();
                
                _companyViewModel.CompanyList = await _companyManager.GetCompany(userData[1], string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return View("Index", _companyViewModel);
        }

        /**************Company Menu function* Start *************/

        /// <summary>
        /// method to get insert company view
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetInsertCompanyView(string companyId)
        {
            _companyViewModel = new CompanyViewModel();
            try
            {
               _companyViewModel.AssignCountryList();
                var userData = User.Identity.Name.Split('|');
                if (!string.IsNullOrEmpty(companyId) && !companyId.Equals("0"))
                {                    
                    _companyViewModel.CompanyList = await _companyManager.GetCompany(userData[1], companyId);
                }
                if (_companyViewModel.CompanyList != null && _companyViewModel.CompanyList.Count > 0)
                {
                    _companyViewModel.CompanyDetail = _companyViewModel.CompanyList.FirstOrDefault();
                }

            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            if (string.IsNullOrEmpty(companyId) || companyId.Equals("0"))
            {
                return PartialView("ManageCompany\\addCompany", _companyViewModel);
            }
            else
            {
                return PartialView("ManageCompany\\editCompany", _companyViewModel);
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
                var userData = User.Identity.Name.Split('|');
                if (companyDashboard?.CompanyDetail == null)
                {
                    return Json(string.Empty);
                }
                
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
                var result = await _companyManager.AddEditCompany(companyDashboard.CompanyDetail);

                if (result == null)
                {
                    return Json(string.Empty);
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(string.Empty);
        }

        /// <summary>
        /// method to get edit company setting view
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetEditCompanySettingView(string companySettingId, string companyName)
        {
            _companyViewModel = new CompanyViewModel();
            _companyViewModel.AssignSettingList();
            _companyViewModel.CompanyName = companyName;
            try
            {
                var setting = await _companyManager.GetCompanySetting(companySettingId);
                if (setting != null)
                {
                    _companyViewModel.ApiSetting = new CompanyApiSetting
                    {
                        company_id = setting.company_id,
                        app_ewallet_api_enabled = setting.app_ewallet_api_enabled,
                        app_login_api_enabled = setting.app_login_api_enabled,
                        cmp_setting_id = setting.cmp_setting_id,
                        app_pg_api_enabled = setting.app_pg_api_enabled,
                        web_ewallet_api_enabled = setting.web_ewallet_api_enabled,
                        web_login_api_enabled = setting.web_login_api_enabled,
                        web_pg_api_enabled = setting.web_pg_api_enabled
                    };

                    _companyViewModel.SmsSetting = new CompanySmsSetting
                    {
                        company_id = setting.company_id,
                        sms_api_integrated = setting.sms_api_integrated,
                        sms_api_password = setting.sms_api_password,
                        sms_api_sender_id = setting.sms_api_sender_id,
                        sms_api_url = setting.sms_api_url,
                        sms_api_username = setting.sms_api_username,
                        cmp_setting_id = setting.cmp_setting_id
                    };

                    _companyViewModel.EmailSetting = new CompanyEmailSetting
                    {
                        company_id = setting.company_id,
                        cmp_setting_id = setting.cmp_setting_id,
                        email_id = setting.email_id,
                        email_password = setting.email_password
                    };

                    _companyViewModel.OtpSetting = new CompanyOtpSetting
                    {
                        company_id = setting.company_id,
                        cmp_setting_id = setting.cmp_setting_id,
                        otp_login_enabled = setting.otp_login_enabled,
                        otp_login_service = setting.otp_login_service
                    };

                    _companyViewModel.CommissionSetting = new CompanyCommissionSetting
                        {
                            company_id = setting.company_id,
                            cmp_setting_id = setting.cmp_setting_id,
                            postpaid_margin = setting.master_postpaid_margin,
                            prepaid_margin = setting.master_prepaid_margin,
                            dth_margin = setting.master_dth_margin
                        };

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return PartialView("ManageCompany\\companySetting", _companyViewModel);
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

                var result = await _companyManager.SaveCompanySetting(settingModel);
                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(string.Empty);
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
                _companyViewModel = new CompanyViewModel {CommissionSetting = settingModel};
                var result = await _companyManager.SaveCompanySetting(_companyViewModel);
                return Json(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return null;
        }
    }
}