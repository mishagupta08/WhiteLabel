using ShineYatraAdmin.Entity;
using ShineYatraAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Business
{
    public class RechargeManager
    {
        /// <summary>
        /// Get details of service providers
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<ServiceDetail>> getserviceProviderlist(ServicesRequest search)
        {
            List<ServiceDetail> serviceDetails = new List<ServiceDetail>();
            try
            {
                serviceDetails = await RechargeAPi.GetServiceProviderList(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return serviceDetails;
        }


        public async Task<ValidateTransaction> ValidateTransaction(ServicesRequest search)
        {
            ValidateTransaction validateTransaction = new ValidateTransaction();
            try
            {
                validateTransaction = await RechargeAPi.ValidateTransaction(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return validateTransaction;
        }

        public async Task<TransactionStatus> Transaction(ServicesRequest search)
        {
            TransactionStatus transactionStatus = new TransactionStatus();
            try
            {
                transactionStatus = await RechargeAPi.Transaction(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return transactionStatus;
        }

        public async Task<InsertServiceRechargeResponse> SaveRechargeRequest(InsertServiceRechargeRequest search)
        {
            List<InsertServiceRechargeResponse> transactionStatus = new List<InsertServiceRechargeResponse>();
            try
            {
                transactionStatus = await Program.SaveRechargeRequest(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return transactionStatus.FirstOrDefault();
        }
    }
}
