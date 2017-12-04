using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Business;
using System.Threading.Tasks;
using System.Configuration;

namespace ShineYatraAdmin.Controllers

{
    [Authorize]
    public class GroupController : Controller
    {
        GroupManager groupManager = new GroupManager();
        ServiceManager serviceManager = new ServiceManager();
        CompanyManager companyManager = new CompanyManager();
        UserManager userManager = new UserManager();
        GroupModel group;
        // GET: Group
        public async Task<ActionResult> Index(string category)
        {
            group = new GroupModel();
            string sub_category = string.Empty;
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                group.service_name = category;
                group.service_id = Common.Common.getIdbyServiceName(category);
                if (group.service_id == "1")
                {
                    group.sub_service_name = "DOMESTIC";
                }
                else if (group.service_id == "2")
                {
                    group.sub_service_name = "Hotels";
                }
                else if (group.service_id == "4")
                {
                    group.sub_service_name = "Prepaid";
                }
                else
                {
                    group.sub_service_name = group.service_name;
                }
                group.service_group_list = await this.serviceManager.GetServiceGroupList(group.service_id, userData[1], userData[2], category, group.sub_service_name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }            
            return View(group);
        }
        
        public async Task<ActionResult> GetAllottedGroup(string category,string sub_category)
        {
            group = new GroupModel();
            Member_Allotted_group Servicegroup = new Member_Allotted_group();
            try
            {
                group.service_name = category;
                group.service_id = Common.Common.getIdbyServiceName(category);

                if (string.IsNullOrEmpty(sub_category))
                {

                    if (group.service_id == "1")
                    {
                        group.sub_service_name = "DOMESTIC";
                    }
                    else if (group.service_id == "2")
                    {
                        group.sub_service_name = "Hotels";
                    }
                    else if (group.service_id == "4")
                    {
                        group.sub_service_name = "Prepaid";
                    }
                    else
                    {
                        group.sub_service_name = group.service_name;
                    }
                }
                else
                {
                    group.sub_service_name = sub_category;
                        }
                string[] userData = User.Identity.Name.Split('|');
                var groups = await this.userManager.GetUserAllottedGroups(userData[1], userData[2], group.service_id,category, group.sub_service_name);
                Servicegroup = groups.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(Servicegroup, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Getgrouplist(string category,string subcategory)
        {
            string service_id = Common.Common.getIdbyServiceName(category);
            group = new GroupModel();             
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                group.service_group_list = await this.serviceManager.GetServiceGroupList(service_id, userData[1], userData[2], category, subcategory);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(group.service_group_list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// method to change commission or price group of a service for particular company
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult> AddNewGroup(NewGroupModel newGroup)
        {
            try {

                string[] userData = User.Identity.Name.Split('|');
                newGroup.company_id = userData[2];
                newGroup.member_id = userData[1];
                string result = await this.groupManager.AddNewGroup(newGroup);
                return Json(result);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return null;
        }

        

        /// <summary>
        /// method to get structure
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult> getCommissionGroupDetails(string service_id,string currentGroupId,string sub_service,string type)
        {
            group = new GroupModel();
            try
            {
                group.service_id = service_id;
                group.service_group_list = await companyManager.GetCompanyCommissionStructure(currentGroupId, service_id, sub_service);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            if (type.ToLower() == "custom")
            {
                return PartialView("groupstructure", group);
            }
            else
            {
                return PartialView("allottedgroupstructure", group);
            }
        }

        /// <summary>
        /// method to update commission row
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult> UpdateCommissionGroupDetails(CompanyCommissionGroupRow UpdatedRow)
        {

            group = new GroupModel();
            try
            {
                string[] userData = User.Identity.Name.Split('|');
                UpdatedRow.member_id = userData[1];
                var result = await groupManager.UpdateGroupRow(UpdatedRow);
                return Json(result);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
                ExceptionLogging.SendErrorTomail(ex, User.Identity.Name, ConfigurationManager.AppSettings["DomainName"]);
            }
            return Json(null);
        }

    }
}