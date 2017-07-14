using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Business
{
    public class ThemeManager
    {
        /// <summary>
        /// Function to get user list 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<List<CompanyTheme>> GetCompanyTheme(string domain)
        {
            return await Program.GetWhitLabelTheme(domain);
        }
    }
}
