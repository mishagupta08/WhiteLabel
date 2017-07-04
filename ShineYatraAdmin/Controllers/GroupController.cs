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
    public class GroupController : Controller
    {
        GroupManager groupManager = new GroupManager();
        ServiceManager serviceManager = new ServiceManager();
        CompanyManager companyManager = new CompanyManager();
        GroupModel group;
        // GET: Group
        public async Task<ActionResult> Index(string category)
        {
            group = new GroupModel();
            string sub_category = string.Empty;
            try
            {
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
                group.service_group_list = await this.serviceManager.GetServiceGroupList(group.service_id, ShineYatraSession.LoginUser.member_id, ShineYatraSession.LoginUser.company_id, category, group.sub_service_name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }            
            return View(group);
        }

        public async Task<ActionResult> Getgrouplist(string category,string subcategory)
        {
            string service_id = Common.Common.getIdbyServiceName(category);
            group = new GroupModel();             
            try
            {               
                group.service_group_list = await this.serviceManager.GetServiceGroupList(service_id, ShineYatraSession.LoginUser.member_id, ShineYatraSession.LoginUser.company_id, category, subcategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
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

                newGroup.company_id = ShineYatraSession.LoginUser.company_id;
                newGroup.member_id = ShineYatraSession.LoginUser.member_id;
                //if (newGroup.service_id == "1")
                //{
                //    newGroup.sub_category = "DOMESTIC";
                //}
                //else if (newGroup.service_id == "2")
                //{
                //    newGroup.sub_category = "Hotels";
                //}
                //else if (newGroup.service_id == "4")
                //{
                //    newGroup.sub_category = "Prepaid";
                //}
                //else
                //{
                //    newGroup.sub_category = newGroup.category;
                //}
                string result = await this.groupManager.AddNewGroup(newGroup);
                return Json(result);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return null;
        }

        /// <summary>
        /// method to get structure
        /// </summary>      
        /// <returns></returns>
        public async Task<ActionResult> getCommissionGroupDetails(string service_id,string currentGroupId,string sub_service)
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
            }
            return PartialView("groupstructure", group);
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
                UpdatedRow.member_id = ShineYatraSession.LoginUser.member_id;
                //if (UpdatedRow.service_id == 1)
                //{
                //    UpdatedRow.sub_category = "DOMESTIC";
                //}
                //else if (UpdatedRow.service_id == 2)
                //{
                //    UpdatedRow.sub_category = "Hotels";
                //}
                //else
                //{
                //    UpdatedRow.sub_category = UpdatedRow.category;
                //}
                var result = await groupManager.UpdateGroupRow(UpdatedRow);
                return Json(result);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.InnerException);
            }
            return Json(null);
        }

    }
}