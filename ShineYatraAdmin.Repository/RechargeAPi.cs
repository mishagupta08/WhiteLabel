namespace ShineYatraAdmin.Repository
{
    #region namespace

    using Newtonsoft.Json;
    using Entity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System;
    using System.Xml;
    using System.Web;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml.Linq;
    using System.Text.RegularExpressions;

    #endregion namespace

    /// <summary>
    /// Recharge APi functionality
    /// </summary>
    public class RechargeAPi
    {
        /// <summary>
        /// Authentication key
        /// </summary>
        const string AuthKey = "e969da44-91f8-4d51-b138-0ace0980d519";
                                
        ///all method to call api

        public static async Task<List<ServiceDetail>> GetServiceProviderList(ServicesRequest serviceRequest)
        {
            var serviceDetails= new List<ServiceDetail>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(ServicesRequest));

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, serviceRequest);
                    httpClient.DefaultRequestHeaders.Add("authKey", AuthKey);
                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    try
                    {
                        var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Recharge/Services", httpContent);
                        if (httpResponse.Content != null)
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();
                            responseContent = Regex.Replace(responseContent, "<.*?>", String.Empty);
                            serviceDetails = JsonConvert.DeserializeObject<List<ServiceDetail>>(responseContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                    }
                       
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return serviceDetails;
        }

        public static async Task<ValidateTransaction> ValidateTransaction(ServicesRequest serviceRequest)
        {
            var validateTransaction = new ValidateTransaction();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(ServicesRequest));

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, serviceRequest);
                    httpClient.DefaultRequestHeaders.Add("authKey", AuthKey);
                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    try
                    {
                        var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Recharge/ValidateTransaction", httpContent);
                        if (httpResponse.Content != null)
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();
                            responseContent = Regex.Replace(responseContent, "<.*?>", String.Empty);
                            validateTransaction = JsonConvert.DeserializeObject<ValidateTransaction>(responseContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                    }
                  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return validateTransaction;
        }

        public static async Task<List<TransactionStatus>> TransactionStatus(ServicesRequest serviceRequest)
        {
            var transactionStatus = new List<TransactionStatus>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(ServicesRequest));

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, serviceRequest);
                    httpClient.DefaultRequestHeaders.Add("authKey", AuthKey);
                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    try
                    {
                        var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Recharge/TransactionStatus", httpContent);
                        if (httpResponse.Content != null)
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();
                            responseContent = Regex.Replace(responseContent, "<.*?>", String.Empty);
                            transactionStatus = JsonConvert.DeserializeObject<List<TransactionStatus>>(responseContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return transactionStatus;
        }

        public static async Task<TransactionStatus> Transaction(ServicesRequest serviceRequest)
        {
            var transactionStatus = new TransactionStatus();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string xmlstring = string.Empty;

                    var serializer = new XmlSerializer(typeof(ServicesRequest));

                    var stringwriter = new System.IO.StringWriter();
                    serializer.Serialize(stringwriter, serviceRequest);
                    httpClient.DefaultRequestHeaders.Add("authKey", AuthKey);
                    var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                    try
                    {
                        var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Recharge/Transaction", httpContent);
                        if (httpResponse.Content != null)
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();
                            responseContent = Regex.Replace(responseContent, "<.*?>", String.Empty);
                            transactionStatus = JsonConvert.DeserializeObject<TransactionStatus>(responseContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                    }
                 }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return transactionStatus;
        }       
    }
}