using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Business
{
    public class UserManager
    {
        /// <summary>
        /// Function to get user list 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<IList<UserDetail>> GetCompanyUserList(string memberId)
        {
            return await Program.GetCompanyUserList(memberId);
        }

        /// <summary>
        /// method to get flight bus and hotel allotted groups
        /// </summary>
        /// <param name="memberId,companyId"></param>
        /// <returns></returns>
        public async Task<Member_Allotted_group> GetUserAllottedGroups(string memberId, string companyId)
        {
            IList<Member_Allotted_group> List = await Program.GetMemberAllottedGroup(memberId, companyId);
            return List.FirstOrDefault();
        }
    }
}
