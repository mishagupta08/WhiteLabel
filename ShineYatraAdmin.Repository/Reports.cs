using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShineYatraAdmin.Entity;

namespace ShineYatraAdmin.Repository
{

    public class Reports
    {       

        /// <summary>
        /// Base url of API
        /// </summary>
        private const string ApiBaseUrl = "http://mukesh.bisplindia.in/apiRouter.aspx";

        /// <summary>
        /// Authentication key
        /// </summary>
        private const string AuthKey = "lPJpfNMUK6u2KAGyJXqxsw==";

        

        /// <summary>
        /// Method to invoke api function
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static async Task<string> CallFunction(string data)
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
                        return responseContent;
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
        /// method to save fund detail while payment from payment gateway
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetLedgerList(DistributorLedgerRequest request)
        {
            var data = JsonConvert.SerializeObject(request);
            data = data.Replace("null", "\"\"");
            return await CallFunction(data);            
        }
    }
}
