
using System.Threading.Tasks;
using ShineYatraAdmin.Repository;
using ShineYatraAdmin.Entity;

namespace ShineYatraAdmin.Business
{
    public class GroupManager
    {
        /// <summary>
        /// method to add new group
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<string> AddNewGroup(NewGroupModel newGroup)
        {
            return await Program.AddNewGroup(newGroup);
        }

        /// <summary>
        /// method update row
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<string> UpdateGroupRow(CompanyCommissionGroupRow groupRow)
        {
            return await Program.UpdateGroupRow(groupRow);
        }
    }
}
