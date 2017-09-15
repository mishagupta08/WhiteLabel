using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;

namespace ShineYatraAdmin.Business
{

    public class FundManager
    {
        /// <summary>
        /// method to add fund details company list
        /// </summary>
        /// <param name="CompanyFund"></param>
        /// <returns></returns>
        public async Task<string> SaveFundDetail(CompanyFund companyFund)
        {
            return await Program.SaveFundDetail(companyFund);
        }

        /// <summary>
        /// method to add fund details company list
        /// </summary>
        /// <param name="CompanyFund"></param>
        /// <returns></returns>
        public async Task<DTUserDetails> WalletFunction(TransactionDetail fundDetail)
        {
            return await Program.WalletFunction(fundDetail);
        }

        /// <summary>
        /// method to add fund details company list
        /// </summary>
        /// <param name="CompanyFund"></param>
        /// <returns></returns>
        public async Task<string> WalletCreditRequest(FundRequestContainer fundDetail)
        {
            return await Program.WalletCreditRequest(fundDetail);
        }

        /// <summary>
        /// method to add fund details company list
        /// </summary>
        /// <param name="CompanyFund"></param>
        /// <returns></returns>
        public async Task<string> UpdateWalletCreditRequest(FundRequestContainer fundDetail)
        {
            return await Program.UpdateWalletCreditRequest(fundDetail);
        }

        /// <summary>
        /// method to get fund request list
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<List<CompanyFund>> getFundRequestList (string memberId)
        {
            return await Program.getFundRequestList(memberId);
        }

        /// <summary>
        /// update fund request
        /// </summary>
        /// <param name="txnid"></param>
        /// <param name="memberid"></param>
        /// <param name="status"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        public async Task<string> UpdateFundRequest(int txnid, string memberid, string status, string remarks) {
            return await Program.UpdateFundRequest(txnid, memberid, status, remarks);
        }
    }
}
