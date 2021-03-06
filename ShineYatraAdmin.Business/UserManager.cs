﻿using ShineYatraAdmin.Entity;
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
        /// Function to get user list 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<IList<UserDetail>> GetMemberUsersList(string memberId,string roleId)
        {
            return await Program.GetMemberUsersList(memberId,roleId);
        }

        /// <summary>
        /// method to get flight bus and hotel allotted groups
        /// </summary>
        /// <param name="memberId,companyId"></param>
        /// <returns></returns>
        public async Task<IList<Member_Allotted_group>> GetUserAllottedGroups(string memberId, string companyId,string serviceid,string category,string sub_category)
        {
            IList<Member_Allotted_group> List = await Program.GetMemberAllottedGroup(memberId, companyId,serviceid,category,sub_category);
            return List;
        }

        /// <summary>
        /// function to get usr wallet balance
        /// </summary>
        /// <param name="bookticket"></param>
        /// <returns></returns>
        public async Task<WalletResponse> GET_WALLET_BALANCE(WalletRequest request)
        {
            List<WalletResponse> response = null;
            EticketDetails eticketDetail = new EticketDetails();
            try
            {
                response = await Program.GET_WALLET_BALANCE(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return response.FirstOrDefault();
        }

        /// <summary>
        /// function to get usr wallet balance
        /// </summary>
        /// <param name="bookticket"></param>
        /// <returns></returns>
        public async Task<string> AddUser(UserDetail userDetail)
        {            
            try
            {
                return await Program.AddUser(userDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return null;
        }
    }
}
