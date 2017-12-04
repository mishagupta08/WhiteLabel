using Newtonsoft.Json;
using ShineYatraAdmin.Entity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShineYatraAdmin.Repository
{
    public class BillService
    {

        private const string ApiBaseUrl = "http://mukesh.bisplindia.in/apiRouter.aspx";

        private const string AuthKey = "lPJpfNMUK6u2KAGyJXqxsw==";

        private static async Task<Response> CallFunction(string data)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var httpContent = new StringContent(data, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Add("Key", AuthKey);

                    // Do the actual request and await the response
                    var httpResponse = await httpClient.PostAsync(ApiBaseUrl, httpContent);

                    // If the response contains content we want to read it!
                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        if (responseContent.ToUpper().Contains("FAILED") || responseContent.ToUpper().Contains("FAIL"))
                        {
                            Console.WriteLine(responseContent);
                            ExceptionLogging.SendAPIErrorTomail(data, responseContent);
                            return JsonConvert.DeserializeObject<Response>(responseContent);
                        }
                        else
                        {
                            var response = JsonConvert.DeserializeObject<Response>(responseContent);
                            return response;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return null;
        }

        /// <summary>
        /// get bill service provider fields
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="subServiceProvider"></param>
        /// <returns></returns>
        public static async Task<List<BillServicesFields>> GetBillServiceFields(string serviceId, string subServiceProvider, string userData)
        {
            var data = string.Empty;
            var arruserData = userData.Split('|');
            if (!string.IsNullOrEmpty(subServiceProvider))
            {
                data = "{\"action\":\"GET_BILL_SERVICES_FIELDS_LIST\",\"SERVICE_ID\":\"" + serviceId + "\",\"sub_service_id\":\"" + subServiceProvider + "\",\"member_id\":\"" + arruserData[1] + "\",\"company_id\":\"" + arruserData[2] + "\"}";
            }
            var response = await CallFunction(data);
            if (response != null && response.GET_BILL_SERVICES_FIELDS_LIST != null)
            {
                return response.GET_BILL_SERVICES_FIELDS_LIST;
            }
            return null;
        }

        /// <summary>
        /// get zone list
        /// </summary>        
        /// <returns></returns>
        public static async Task<List<ZoneList>> GetZoneList(string userData)
        {
            var data = string.Empty;
            var arruserData = userData.Split('|');
            data = "{\"action\":\"GET_ZONE_LIST\",\"member_id\":\"" + arruserData[1] + "\",\"company_id\":\"" + arruserData[2] + "\"}";
            var response = await CallFunction(data);
            if (response != null && response.GET_ZONE_LIST != null)
            {
                return response.GET_ZONE_LIST;
            }
            return null;
        }

        /// <summary>
        /// get Sub zone list
        /// </summary>        
        /// <returns></returns>
        public static async Task<List<SubZoneList>> GetSubZoneList(string zoneId, string userData)
        {
            var data = string.Empty;
            var arruserData = userData.Split('|');
            data = "{\"action\":\"GET_SUB_ZONE_LIST\",\"ZONE_ID\":" + zoneId + ",\"member_id\":\"" + arruserData[1] + "\",\"company_id\":\"" + arruserData[2] + "\"}";
            var response = await CallFunction(data);
            if (response != null && response.GET_SUB_ZONE_LIST != null)
            {
                return response.GET_SUB_ZONE_LIST;
            }
            return null;
        }
    }
}
