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
    public class UserController : Controller
    {

        UserManager userManager = new UserManager();        
        CompanyManager companyManager = new CompanyManager();
        ServiceManager serviceManager = new ServiceManager();
        Service serviceModel;
        UserViewModel userViewModel;

        /// <summary>
        /// hold page size.
        /// </summary>
        private static int PAGESIZE = Convert.ToInt32(Resources.PageSize);

        /// <summary>
        /// hold page show count
        /// </summary>
        private static int PAGESHOWCOUNT = Convert.ToInt32(Resources.PageSize);

        /// <summary>
        /// Constant for ascending order
        /// </summary>
        public const string Asc = "Asc";

        /// <summary>
        /// Constant for Descending order
        /// </summary>
        public const string Desc = "Desc";


        /// <summary>
        /// GET: User list for company
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> wl_primary_users()
        {
            userViewModel = new UserViewModel();           
            try
            {
                /******Paging setting start*****/
                userViewModel.PageSlot = ShineYatraSession.PageIndex / PAGESIZE;
                if (ShineYatraSession.PageIndex % PAGESIZE != 0)
                {
                    userViewModel.PageSlot++;
                }
                userViewModel.AssignSearchList();
                /******Paging setting end*****/
                await GetCompanyListByPageIndex(1);
                //userViewModel.userDetailList  = await userManager.GetCompanyUserList("");

            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return View(userViewModel);
        }


        public ActionResult setSessionForSelectedUser(string member_id, string company_id) {
        try
            {               
                if(!string.IsNullOrEmpty(member_id) && !string.IsNullOrEmpty(company_id))
                {
                    Session["member_id"] = member_id;
                    Session["company_id"] = company_id;
                    return Json(true);
                }              
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return Json(false);
        }

        /// <summary>
        /// GET: User Commission page
        /// </summary>
        /// <returns></returns>-
        public async Task<ActionResult> commissionStructure()
        {
            userViewModel = new UserViewModel();
            IList<UserDetail> userList = new List<UserDetail>();
            try
            {
                string member_id = Session["member_id"].ToString();
                string company_id = Session["company_id"].ToString();
                userViewModel.services = await this.companyManager.GetCompanyServices("");
                userList = await this.userManager.GetCompanyUserList(member_id);
                userViewModel.userDetail = (from r in userList select r).FirstOrDefault();               
                userViewModel.membergroups = await this.userManager.GetUserAllottedGroups(member_id, company_id);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(userViewModel);
        }

        /// <summary>
        /// method to change commission or price group of a service for particular company
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult> getCommissionGroupDetails(string currentGroupId, string sectionName,string subSection)
        {
            userViewModel = new UserViewModel();
            try
            {
                userViewModel.commissionstucture = null;
                string member_id = Session["member_id"].ToString();
                string company_id = Session["company_id"].ToString();
                string service_id = Common.Common.getIdbyServiceName(sectionName);
                if (string.IsNullOrEmpty(subSection))
                {
                    if (sectionName.ToLower().Trim() == "flight")
                    {
                        subSection = "domestic";
                    }
                    else if (sectionName.ToLower().Trim() == "hotel")
                    {
                        subSection = "hotels";
                    }
                    else if (sectionName.ToLower().Trim() == "bus")
                    {
                        subSection = "bus";
                    }
                    else if (sectionName.ToLower().Trim() == "recharge")
                    {
                        subSection = "prepaid";
                    }
                }
                                
                if (!string.IsNullOrEmpty(currentGroupId))
                {
                    userViewModel.commissionstucture = await this.companyManager.GetCompanyCommissionStructure(currentGroupId, service_id, subSection);
                    userViewModel.membergroups = await this.userManager.GetUserAllottedGroups(member_id, company_id);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return PartialView("_structure", userViewModel);
        }

        /// <summary>
        /// method to get list of all price or commission group and create a slect item list for it. 
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult>  GetServiceGroupList(string serviceId, string currentGroupId)
        {

            this.serviceModel = new Service();
            List<SelectListItem> groupList = new List<SelectListItem>();
            
            try
            {
                string service_name = Common.Common.getServiceNamebyId(serviceId);
                string sub_category = service_name;
                if (service_name.ToUpper() == "FLIGHT")
                {
                    sub_category = "DOMESTIC";
                }
                string member_id = Session["member_id"].ToString();
                string company_id = Session["company_id"].ToString();
                this.serviceModel.service_group_list = await this.serviceManager.GetServiceGroupList(serviceId, member_id, company_id, service_name, sub_category);
                foreach (var group in this.serviceModel.service_group_list)
                {

                    if (currentGroupId == group.comp_group_id)
                    {
                        groupList.Add(new SelectListItem
                        {
                            Text = group.comp_group_name,
                            Value = group.comp_group_id,
                            Selected = true
                        });
                    }
                    else
                    {
                        groupList.Add(new SelectListItem
                        {
                            Text = group.comp_group_name,
                            Value = Convert.ToString(group.comp_group_id),
                            Selected = false
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(groupList);
        }

        /// <summary>
        /// method to change commission or price group of a service for particular company
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult> EditCompanyPriceGroup(int price_group_id, int service_id)
        {
            try
            {
                string member_id = Session["member_id"].ToString();                
                int company_id = int.Parse(!String.IsNullOrEmpty(Session["company_id"].ToString())? Session["company_id"].ToString():"0");
                var result = await this.companyManager.EditCompanyServiceGroup(company_id, price_group_id, service_id, member_id);
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
        /// Method to get record by page index for all views
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="View"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetRecordsByPageIndex(int pageIndex, string View, int paging_count, int RecordCount)
        {
            userViewModel = new UserViewModel();
            userViewModel.CurrentPageIndex = pageIndex;
            userViewModel.PagingCount = paging_count;
            userViewModel.RecordCount = RecordCount;
            ShineYatraSession.PageIndex = pageIndex;

            if (View != null && View == Resources.ManageCompanies)
            {
                await GetCompanyListByPageIndex(pageIndex);
                return PartialView("UserList", userViewModel);
            }

            return null;
        }

        /// <summary>
        /// Method to get company list
        /// </summary>
        /// <returns></returns>
        private async Task GetCompanyListByPageIndex(int pageIndex)
        {
            userViewModel.CurrentPageIndex = pageIndex;
            ShineYatraSession.PageIndex = pageIndex;
            if (ShineYatraSession.TempUSerList == null)
            {
                ShineYatraSession.TempUSerList = await userManager.GetCompanyUserList("");
            }

            if (string.IsNullOrEmpty(ShineYatraSession.SortCoulmn))
            {
                ShineYatraSession.SortCoulmn = "company_id";
                ShineYatraSession.SortOrder = Asc;
            }

            if (ShineYatraSession.TempUSerList != null && ShineYatraSession.SortCoulmn != null)
            {
                var sortExpression = ShineYatraSession.SortCoulmn + " " + ShineYatraSession.SortOrder;
                var companyList = ShineYatraSession.TempUSerList.OrderBy(sortExpression);
                if (companyList != null)
                {
                    ShineYatraSession.TempUSerList = companyList.ToList();
                }
                if (ShineYatraSession.TempUSerList != null)
                {
                    CalculatePageCount(userViewModel, ShineYatraSession.TempUSerList.Count);
                }
                var list = await Task.Run(() => ShineYatraSession.TempUSerList.Skip((pageIndex - 1) * PAGESIZE).Take(PAGESIZE));
                if (list != null)
                {
                    userViewModel.userDetailList = list.ToList();
                    userViewModel.FromPage = (PAGESIZE * (pageIndex - 1)) + 1;
                    userViewModel.ToPage = userViewModel.FromPage + list.Count() - 1;
                }
            }
        }

        /// <summary>
        /// method to calculate page size
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        public void CalculatePageCount(UserViewModel listData, int recordCount)
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

        /// <summary>
        /// method to search company list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> SearchUserList(UserViewModel searchParameter)
        {
            userViewModel = new UserViewModel();
            userViewModel.userDetailList = await userManager.GetCompanyUserList("");
            userViewModel.AssignSearchList();
            userViewModel.SearchListParameter = searchParameter.SearchListParameter;
            if (userViewModel.userDetailList != null && userViewModel.userDetailList.Count > 0)
            {
                if (!string.IsNullOrEmpty(userViewModel.SearchListParameter.CompanyName))
                {
                    var companyName = userViewModel.SearchListParameter.CompanyName.ToLower();
                    var list = userViewModel.userDetailList.Where(c => c.company_name != null && c.company_name.ToLower().Contains(companyName));
                    if (list != null)
                    {
                        userViewModel.userDetailList = list.ToList();
                    }
                }

                if (!string.IsNullOrEmpty(userViewModel.SearchListParameter.Status) && !userViewModel.SearchListParameter.Status.Equals("All"))
                {
                    var list = userViewModel.userDetailList.Where(c => c.active_status != null && c.active_status.Contains(userViewModel.SearchListParameter.Status));
                    if (list != null)
                    {
                        userViewModel.userDetailList = list.ToList();
                    }
                }
            }

            ShineYatraSession.TempUSerList = userViewModel.userDetailList;
            await GetCompanyListByPageIndex(1);
            if (ShineYatraSession.TempCompanyList != null)
            {
                CalculatePageCount(userViewModel, ShineYatraSession.TempCompanyList.Count);
            }

            return PartialView("UserList", userViewModel);
        }

        public ActionResult FundRequest()
        {            
            CompanyFund companyFund = new CompanyFund();
            try
            {                
                companyFund.member_id = Session["member_id"].ToString();
                companyFund.company_id = Session["company_id"].ToString();               
                companyFund.service_id = 0;
                companyFund.cancel_request_id = 0;
                companyFund.txn_type = "FUND";
                companyFund.AssignDepositModeList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return PartialView(companyFund);
        }

        public async Task<ActionResult> SaveFundDetail(CompanyFund fundModel)
        {
            FundManager fundManger = new FundManager();
            try
            {
                fundModel.action = "INSERT_TRANSACTION_REQUEST";
                var response = await fundManger.SaveFundDetail(fundModel);
                return Json(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return Json(string.Empty);
        }

    }
}