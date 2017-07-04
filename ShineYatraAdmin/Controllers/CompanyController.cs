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
        /// Constant for ascending order
        /// </summary>
        public const string Asc = "Asc";

        /// <summary>
        /// Constant for Descending order
        /// </summary>
        public const string Desc = "Desc";

        /// <summary>
        /// constant for active status
        /// </summary>
        private const string Active = "Y";

        /// <summary>
        /// hold page size.
        /// </summary>
        private static int PAGESIZE = Convert.ToInt32(Resources.PageSize);

        /// <summary>
        /// hold page show count
        /// </summary>
        private static int PAGESHOWCOUNT = Convert.ToInt32(Resources.PageSize);

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
                companyViewModel.LoginUserName = ShineYatraSession.LoginUser.first_name;
                ShineYatraSession.SortCoulmn = sortColumn;
                ShineYatraSession.SortOrder = sortOrder;
                if (string.IsNullOrEmpty(menu))
                {
                    if (ShineYatraSession.SelectedMenu == null)
                    {
                        return null;
                    }

                    menu = ShineYatraSession.SelectedMenu;
                }

                if (ShineYatraSession.LoginUser == null)
                {
                    return null;
                }

                this.companyViewModel.SelectedMenu = menu;
                ShineYatraSession.SelectedMenu = menu;

                /******Paging setting start*****/
                var loadPageIndex = isRefresh || isBack ? ShineYatraSession.PageIndex : 1;
                if (isRefresh || isBack)
                {
                    companyViewModel.PageSlot = ShineYatraSession.PageIndex / PAGESIZE;
                    if (ShineYatraSession.PageIndex % PAGESIZE != 0)
                    {
                        companyViewModel.PageSlot++;
                    }
                }
                else
                {
                    companyViewModel.PageSlot = 0;
                }

                /******Paging setting end*****/

                if (menu == Resources.ManageCompanies)
                {
                    ShineYatraSession.TempCompanyList = null;
                    this.companyViewModel.SelectedMenu = menu;
                    this.companyViewModel.SearchListParameter = new SearchParameter();
                    this.companyViewModel.AssignSearchList();
                    await GetCompanyListByPageIndex(loadPageIndex);

                    if (ShineYatraSession.TempCompanyList != null)
                    {
                        CalculatePageCount(companyViewModel, ShineYatraSession.TempCompanyList.Count);
                    }
                }

                if (sortColumn != null && sortOrder != null)
                {
                    if (companyViewModel.SelectedMenu == Resources.ManageCompanies)
                    {
                        return PartialView("ManageCompany\\companies", this.companyViewModel);
                    }
                }
            }
            catch (Exception ex) {
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
                this.companyViewModel.CurrentPageIndex = pageIndex;
                ShineYatraSession.PageIndex = pageIndex;
                if (ShineYatraSession.TempCompanyList == null)
                {
                    ShineYatraSession.TempCompanyList = await this.companyManager.GetCompany(ShineYatraSession.LoginUser.member_id, string.Empty);
                }

                if (string.IsNullOrEmpty(ShineYatraSession.SortCoulmn))
                {
                    ShineYatraSession.SortCoulmn = "company_id";
                    ShineYatraSession.SortOrder = Asc;
                }

                if (ShineYatraSession.TempCompanyList != null && ShineYatraSession.SortCoulmn != null)
                {
                    var sortExpression = ShineYatraSession.SortCoulmn + " " + ShineYatraSession.SortOrder;
                    var companyList = ShineYatraSession.TempCompanyList.OrderBy(sortExpression);
                    if (companyList != null)
                    {
                        ShineYatraSession.TempCompanyList = companyList.ToList();
                    }

                    var list = await Task.Run(() => ShineYatraSession.TempCompanyList.Skip((pageIndex - 1) * PAGESIZE).Take(PAGESIZE));
                    if (list != null)
                    {
                        this.companyViewModel.CompanyList = list.ToList();
                        this.companyViewModel.FromPage = (PAGESIZE * (pageIndex - 1)) + 1;
                        this.companyViewModel.ToPage = this.companyViewModel.FromPage + list.Count() - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
        }

        /// <summary>
        /// method to calculate page size
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        public void CalculatePageCount(CompanyViewModel listData, int recordCount)
        {
            try
            {
                listData.RecordCount = recordCount;
                if (recordCount == 0)
                {
                    listData.PagingCount = 0;
                }
                else
                {
                    listData.PagingCount = recordCount / PAGESIZE;
                    if (recordCount % PAGESIZE > 0)
                    {
                        listData.PagingCount = listData.PagingCount + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
}

        /// <summary>
        /// method to search company list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> SearchCompanyList(CompanyViewModel searchParameter)
        {
            this.companyViewModel = new CompanyViewModel();
            try { 
            this.companyViewModel.CompanyList = await this.companyManager.GetCompany(ShineYatraSession.LoginUser.member_id, string.Empty);
            this.companyViewModel.AssignSearchList();
            this.companyViewModel.SearchListParameter = searchParameter.SearchListParameter;
            if (this.companyViewModel.CompanyList != null && this.companyViewModel.CompanyList.Count > 0)
            {
                if (!string.IsNullOrEmpty(this.companyViewModel.SearchListParameter.CompanyName))
                {
                    var companyName = this.companyViewModel.SearchListParameter.CompanyName.ToLower();
                    var list = this.companyViewModel.CompanyList.Where(c => c.company_name != null && c.company_name.ToLower().Contains(companyName));
                    if (list != null)
                    {
                        this.companyViewModel.CompanyList = list.ToList();
                    }
                }

                if (!string.IsNullOrEmpty(this.companyViewModel.SearchListParameter.Status) && !this.companyViewModel.SearchListParameter.Status.Equals("All"))
                {
                    var list = this.companyViewModel.CompanyList.Where(c => c.active_status != null && c.active_status.Contains(this.companyViewModel.SearchListParameter.Status));
                    if (list != null)
                    {
                        this.companyViewModel.CompanyList = list.ToList();
                    }
                }
            }

            ShineYatraSession.TempCompanyList = this.companyViewModel.CompanyList;
            await GetCompanyListByPageIndex(1);
            if (ShineYatraSession.TempCompanyList != null)
            {
                CalculatePageCount(this.companyViewModel, ShineYatraSession.TempCompanyList.Count);
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return PartialView("ManageCompany\\companies", this.companyViewModel);
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

                    this.companyViewModel.CompanyList = await this.companyManager.GetCompany(ShineYatraSession.LoginUser.member_id, companyId);


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
                if (companyDashboard == null || companyDashboard.CompanyDetail == null)
                {
                    return Json(string.Empty);
                }

                //if (string.IsNullOrEmpty(companyDashboard.CompanyDetail.company_id))
                if (companyDashboard.CompanyDetail.company_id == 0)
                {
                    companyDashboard.CompanyDetail.action = "InsertCompany";
                    companyDashboard.CompanyDetail.add_user_id = ShineYatraSession.LoginUser.member_id;
                    companyDashboard.CompanyDetail.member_id = ShineYatraSession.LoginUser.member_id;
                }
                else
                {
                    companyDashboard.CompanyDetail.action = "UpdateCompany";
                    companyDashboard.CompanyDetail.update_user_id = ShineYatraSession.LoginUser.member_id;
                }

                companyDashboard.CompanyDetail.active_status = Active;
                companyDashboard.CompanyDetail.member_id = ShineYatraSession.LoginUser.member_id;
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
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return PartialView("ManageCompany\\companySetting", this.companyViewModel);
        }

        /// <summary>
        /// Method to get record by page index for all views
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="View"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetRecordsByPageIndex(int pageIndex, string View, int paging_count, int RecordCount)
        {
            try
            {
                this.companyViewModel = new CompanyViewModel();
                this.companyViewModel.CurrentPageIndex = pageIndex;
                this.companyViewModel.PagingCount = paging_count;
                this.companyViewModel.RecordCount = RecordCount;
                ShineYatraSession.PageIndex = pageIndex;

                if (View != null && View == Resources.ManageCompanies)
                {
                    await GetCompanyListByPageIndex(pageIndex);
                    return PartialView("ManageCompany\\companyList", this.companyViewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return null;
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
            catch (Exception ex) {
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
            catch (Exception ex) {
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