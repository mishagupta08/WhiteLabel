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
        /// GET: User list for company
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> wl_primary_users()
        {
            userViewModel = new UserViewModel();           
            try
            {
                userViewModel.AssignSearchList();                     
                userViewModel.userDetailList  = await userManager.GetCompanyUserList("");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return View(userViewModel);
        }

        /// <summary>
        /// GET: User list for company
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MemberUsers()
        {
            userViewModel = new UserViewModel();
            string[] userData = User.Identity.Name.Split('|');
            try
            {
                userViewModel.AssignSearchList();
                userViewModel.userDetailList = await userManager.GetMemberUsersList(userData[1],"5");
            }
            catch (Exception ex)
            {
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
                userViewModel.membergroups = await this.userManager.GetUserAllottedGroups(member_id, company_id, "", "", "");
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
        public async Task<ActionResult> getCommissionGroupDetails(string currentGroupId, string category, string sub_category)
        {
            userViewModel = new UserViewModel();
            try
            {
                userViewModel.commissionstucture = null;
                string member_id = Session["member_id"].ToString();
                string company_id = Session["company_id"].ToString();
                string service_id = Common.Common.getIdbyServiceName(category);
                if (string.IsNullOrEmpty(sub_category))
                {
                    if (category.ToLower().Trim() == "flight")
                    {
                        sub_category = "domestic";
                    }
                    else if (category.ToLower().Trim() == "hotel")
                    {
                        sub_category = "hotels";
                    }
                    else if (category.ToLower().Trim() == "bus")
                    {
                        sub_category = "bus";
                    }
                    else if (category.ToLower().Trim() == "recharge")
                    {
                        sub_category = "prepaid";
                    }
                }

                if (!string.IsNullOrEmpty(currentGroupId))
                {
                    userViewModel.commissionstucture = await this.companyManager.GetCompanyCommissionStructure(currentGroupId, service_id, sub_category);
                    userViewModel.membergroups = await this.userManager.GetUserAllottedGroups(member_id, company_id, service_id, category, sub_category);
                    userViewModel.category = category;
                    userViewModel.sub_category = sub_category;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return PartialView("_structure", userViewModel);
        }

        /// <summary>
        /// method to get list of all price or commission group and create a slect item list for it. 
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult> GetServiceGroupList(string serviceId, string currentGroupId, string sub_category)
        {

            this.serviceModel = new Service();
            List<SelectListItem> groupList = new List<SelectListItem>();

            try
            {
                string service_name = Common.Common.getServiceNamebyId(serviceId);
                if (string.IsNullOrEmpty(sub_category))
                {
                    if (service_name.ToUpper() == "FLIGHT")
                    {
                        sub_category = "DOMESTIC";
                    }
                    else if (service_name.ToUpper() == "HOTEL")
                    {
                        sub_category = "HOTELS";
                    }
                }
                string member_id = Session["member_id"].ToString();
                string company_id = Session["company_id"].ToString();
                this.serviceModel.service_group_list = await this.serviceManager.GetGroupList(serviceId, member_id, service_name, sub_category);
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
                int company_id = int.Parse(!String.IsNullOrEmpty(Session["company_id"].ToString()) ? Session["company_id"].ToString() : "0");
                string cat = ShineYatraAdmin.Common.Common.getServiceNamebyId(Convert.ToString(service_id));
                string subSection = string.Empty;
                if (!string.IsNullOrEmpty(cat))
                {
                    if (cat.ToLower().Trim() == "flight")
                    {
                        subSection = "domestic";
                    }
                    else if (cat.ToLower().Trim() == "hotel")
                    {
                        subSection = "hotels";
                    }
                    else if (cat.ToLower().Trim() == "bus")
                    {
                        subSection = "bus";
                    }
                    else if (cat.ToLower().Trim() == "recharge")
                    {
                        subSection = "prepaid";
                    }
                }
                var result = await this.companyManager.EditCompanyServiceGroup(company_id, price_group_id, service_id, member_id, cat, subSection);
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