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

    #endregion namespace

    /// <summary>
    /// Hold api functionality
    /// </summary>
    public class Program
    {
        static void Main()
        {
        }

        /// <summary>
        /// Constant for success status
        /// </summary>
        private const string SUCCESS = "SUCCESS";

        /// <summary>
        /// Base url of API
        /// </summary>
        const string ApiBaseURL = "http://mukesh.bisplindia.in/apiRouter.aspx";

        /// <summary>
        /// Authentication key
        /// </summary>
        const string AuthKey = "lPJpfNMUK6u2KAGyJXqxsw==";

        /// <summary>
        /// Authentication key
        /// </summary>
        const string FlightAuthKey = "e969da44-91f8-4d51-b138-0ace0980d519";

        /// <summary>
        /// Method to invoke api function
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
                    var httpResponse = await httpClient.PostAsync(ApiBaseURL, httpContent);

                    // If the response contains content we want to read it!

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        if (responseContent.Contains("FAILED") || responseContent.Contains("FAIL"))
                        {
                            Console.WriteLine(responseContent);
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

        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Implemented based on interface, not part of algorithm
        public static string RemoveAllNamespacesPost(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.Value.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        //replace <br> tag with new line character
        public static string ReplaceBRwithNewline(string txtVal)
        {
            string newText = "";
            // Create regular expressions    
            System.Text.RegularExpressions.Regex regex =
                new System.Text.RegularExpressions.Regex(@"(<br />|<br/>|</ br>|</br>|<br>)");
            // Replace new line with <br/> tag    
            newText = regex.Replace(txtVal, "\r\n");
            // Result    
            return newText;
        }

        #region Login API

        /// <summary>
        /// Method to validate user
        /// </summary>
        /// <param name="loginDetail"></param>
        /// <returns></returns>
        public static async Task<UserDetail> ValidateUser(LoginModel loginDetail)
        {
            string data = JsonConvert.SerializeObject(loginDetail);
            var response = await CallFunction(data);

            if (response != null && response.VALIDATELOGIN != null && response.VALIDATELOGIN.Count > 0)
            {
                return response.VALIDATELOGIN.FirstOrDefault();
            }

            return null;
        }

        #endregion Login API

        #region Primary Setting API
        /// <summary>
        /// method to get Primary Settings
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<IList<CompanyCommissionGroup>> GetPrimarySetting(string serviceId, string sub_service)
        {
            var data = string.Empty;
            //Code for get services           
            data = "{\"action\":\"GET_SERVICE_DETAILS\",\"SERVICE_ID\":\"" + serviceId + "\"}";
            if (!string.IsNullOrEmpty(sub_service))
            {
                data = "{\"action\":\"GET_SERVICE_DETAILS\",\"SERVICE_ID\":\"" + serviceId + "\",\"sub_category\":\"" + sub_service + "\"}";
            }
            var response = await CallFunction(data);
            if (response != null && response.GET_SERVICE_DETAILS != null)
            {
                return response.GET_SERVICE_DETAILS;
            }
            return null;
        }

        /// <summary>
        /// method to update primay setting margin for airline bus and hotel
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<string> UpdatePrimaryMargin(PrimarySettingMargin Row)
        {
            Row.action = "UPDATE_PRIMARY_SERVICES_COMMISSION";
            string data = JsonConvert.SerializeObject(Row);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == SUCCESS)
            {
                return response.APISTATUS;
            }
            else if (response != null)
            {
                return response.MSG;
            }
            return null;
        }

        /// <summary>
        /// method to update primay setting margin for airline bus and hotel
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<List<CompanyTheme>> GetWhitLabelTheme(string domain)
        {            
            string data = "{\"action\":\"GET_DOMAININFO\",\"domain_name\":\""+domain+"\"}";
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == SUCCESS && response.GET_DOMAININFO != null)
            {
                return response.GET_DOMAININFO;
            }           
            return null;
        }
        

        /// <summary>
        /// get list of subservices for a service
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<IList<SubCategory>> GetSubServiceList(string serviceId)
        {
            string data = string.Empty;
            if (!string.IsNullOrEmpty(serviceId))
            {
                data = "{\"action\":\"GET_SERVICE_SUB_CATEGORY_LIST\",\"service_id\":\"" + serviceId + "\"}";
                var response = await CallFunction(data);
                if (response != null && response.GET_SERVICE_SUB_CATEGORY_LIST != null && response.GET_SERVICE_SUB_CATEGORY_LIST.Count > 0)
                {
                    return response.GET_SERVICE_SUB_CATEGORY_LIST;
                }
            }
            return null;
        }


        /// <summary> 
        /// Get primary default recharge setting
        /// </summary>
        /// <returns></returns>
        public static async Task<IList<DefaultRechargeSetting>> GetRechargeDefaultSetting()
        {
            var data = "{\"action\":\"get_default_settings\"}";
            var response = await CallFunction(data);
            if (response != null && response.GET_DEFAULT_SETTINGS != null)
            {
                return response.GET_DEFAULT_SETTINGS;
            }
            return null;
        }

        /// <summary>
        /// Save primary recharge default setting
        /// </summary>
        /// <param name="addSetting"></param>
        /// <returns></returns>
        public static async Task<string> SaveRechargeDefaultSetting(DefaultRechargeSetting addSetting)
        {
            addSetting.action = "UPDATE_PRIMARY_DEFAULT_SETTINGS";
            string data = JsonConvert.SerializeObject(addSetting);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == SUCCESS)
            {
                return response.APISTATUS;
            }
            return null;
        }

        #endregion Primary Setting API

        #region Company API
        public static async Task<CompanySetting> GetCompanySetting(string companySettingId)
        {
            var data = "{\"action\":\"GET_COMPANY_EXTRA_SETTINGS\",\"cmp_setting_id\":\"xyz\"}";
            data = data.Replace("xyz", companySettingId);
            var response = await CallFunction(data);
            if (response != null && response.GET_COMPANY_EXTRA_SETTINGS != null && response.GET_COMPANY_EXTRA_SETTINGS.Count > 0)
            {
                return response.GET_COMPANY_EXTRA_SETTINGS.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// method to get copany list 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static async Task<IList<Company>> GetCompany(string memberId, string companyId)
        {
            var data = string.Empty;
            //Code for add company
            if (string.IsNullOrEmpty(companyId))
            {
                data = "{\"action\":\"GETCOMPANY\",\"member_id\":\"xyz\"}";
            }
            else
            {
                //Code for Edit company
                data = "{\"action\":\"GETCOMPANY\",\"member_id\":\"xyz\",\"company_id\":\"abc\"}";
                data = data.Replace("abc", companyId);
            }

            data = data.Replace("xyz", memberId);
            var response = await CallFunction(data);
            if (response != null && response.GETCOMPANY != null)
            {
                return response.GETCOMPANY;
            }

            return null;
        }

        /// <summary>
        /// Method to add edit company detail
        /// </summary>
        /// <param name="companyDetail"></param>
        /// <returns></returns>
        public static async Task<string> AddEditCompany(Company companyDetail)
        {
            string data = JsonConvert.SerializeObject(companyDetail);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == SUCCESS)
            {
                return response.APISTATUS;
            }

            return null;
        }

        /// <summary>
        /// Method to save company setting
        /// </summary>
        /// <param name="companySettingModel"></param>
        /// <returns></returns>
        public static async Task<string> SaveCompanySetting(CompanyViewModel companySettingModel)
        {
            string data = string.Empty;

            if (companySettingModel.ApiSetting != null)
            {
                companySettingModel.ApiSetting.action = "UPDATE_COMPANY_EXTRASETTINGS_API";
                data = JsonConvert.SerializeObject(companySettingModel.ApiSetting);
            }
            else if (companySettingModel.SmsSetting != null)
            {
                companySettingModel.SmsSetting.action = "UPDATE_COMPANY_EXTRASETTINGS_SMS";
                data = JsonConvert.SerializeObject(companySettingModel.SmsSetting);
            }
            else if (companySettingModel.EmailSetting != null)
            {
                companySettingModel.EmailSetting.action = "UPDATE_COMPANY_EXTRASETTINGS_EMAIL";
                data = JsonConvert.SerializeObject(companySettingModel.EmailSetting);
            }
            else if (companySettingModel.OtpSetting != null)
            {
                companySettingModel.OtpSetting.action = "UPDATE_COMPANY_EXTRASETTINGS_OTP";
                data = JsonConvert.SerializeObject(companySettingModel.OtpSetting);
            }
            else if (companySettingModel.CommissionSetting != null)
            {
                companySettingModel.CommissionSetting.action = "UPDATE_COMPANY_EXTRASETTINGS_COMMISSION";
                data = JsonConvert.SerializeObject(companySettingModel.CommissionSetting);
            }

            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace("null", "\"\"");
                var response = await CallFunction(data);
                if (response != null && response.APISTATUS == SUCCESS)
                {
                    return response.APISTATUS;
                }
            }

            return string.Empty;
        }

        #endregion Company API

        #region Fund API
        /// <summary>
        /// method to save fund detail
        /// </summary>
        /// <param name="fundDetail"></param>
        /// <returns></returns>
        public static async Task<string> SaveFundDetail(CompanyFund fundDetail)
        {
            string data = JsonConvert.SerializeObject(fundDetail);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == SUCCESS)
            {
                return response.APISTATUS;
            }
            else if (response != null)
            {
                return response.MSG;
            }
            return null;
        }

        #endregion fund API

        #region Service Group API
        /// <summary>
        /// method to get all the price groups (commission structures) for a service
        /// </summary>        
        /// <returns></returns>

        public static async Task<IList<CompanyCommissionGroup>> GetServiceGroupList(string serviceId, string memberId, string companyId, string service_type, string sub_category)
        {
            var data = string.Empty;
            if (!string.IsNullOrEmpty(serviceId) && !string.IsNullOrEmpty(memberId) && !string.IsNullOrEmpty(companyId))
            {
                data = "{\"action\":\"GET_COMMISSION_GROUPS\",\"service_id\":\"" + serviceId + "\",\"category\":\"" + service_type + "\",\"member_id\":\"" + memberId + "\",\"company_id\":\"" + companyId + "\",\"sub_category\":\"" + sub_category + "\"}";
            }
            var response = await CallFunction(data);
            if (response != null && response.GET_COMMISSION_GROUPS != null)
            {
                return response.GET_COMMISSION_GROUPS;
            }
            return null;
        }

        /// <summary>
        /// method to get details of all price groups according to commission id
        /// </summary>        
        /// <returns></returns>
        public static async Task<IList<CompanyCommissionGroup>> GetCompanyCommissionStructure(string commissionId, string serviceid, string subservice)
        {
            var data = string.Empty;
            if (!string.IsNullOrEmpty(commissionId))
            {
                data = "{\"action\":\"GET_COMMISSION_GROUP_STRUCTURE\",\"comp_group_id\":\"" + commissionId + "\",\"service_id\":\"" + serviceid + "\",\"sub_category\":\"" + subservice + "\"}";
            }
            var response = await CallFunction(data);
            if (response != null && response.GET_COMMISSION_GROUP_STRUCTURE != null)
            {
                return response.GET_COMMISSION_GROUP_STRUCTURE;
            }
            return null;
        }

        /// <summary>
        /// method to update commission structure group
        /// </summary>        
        /// <returns></returns>
        public static async Task<string> EditCompanyServiceGroup(int company_id, int price_group_id, int service_id, string member_id)
        {
            string data = "{\"action\":\"UPDATE_ALLOTMENT_COMMISSION_GROUP\",\"company_id\": " + company_id + ",\"price_group_id\":" + price_group_id + ",\"service_id\": " + service_id + ",\"member_id\": " + member_id + "}";
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == SUCCESS)
            {
                return response.APISTATUS;
            }

            return null;
        }

        /// <summary>
        /// method to get company service list 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static async Task<IList<Service>> GetCompanyServices(string companyId)
        {
            var data = string.Empty;
            //Code for get services           
            data = "{\"action\":\"get_services\"}";
            var response = await CallFunction(data);
            if (response != null && response.GET_SERVICES != null)
            {
                return response.GET_SERVICES;
            }
            return null;
        }




        /// <summary>
        /// method to get Recharge Commission Structure
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<IList<CompanyCommissionGroup>> GetRechargeCommissionStructure(string companyId)
        {
            var data = string.Empty;
            //Code for get services           
            data = "{\"action\":\"GET_COMPANY_RECHARGE_COMMISSION\",\"COMPANY_ID\":\"" + companyId + "\"}";
            var response = await CallFunction(data);
            if (response != null && response.GET_COMPANY_RECHARGE_COMMISSION != null)
            {
                return response.GET_COMPANY_RECHARGE_COMMISSION;
            }
            return null;
        }

        /// <summary>
        /// method to Create new flight,bus, hotel group 
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<string> AddNewGroup(NewGroupModel newGroup)
        {
            newGroup.action = "INSERT_COMMISSION_GROUP";
            string data = JsonConvert.SerializeObject(newGroup);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == SUCCESS)
            {
                return response.APISTATUS;
            }
            else if (response != null)
            {
                return response.MSG;
            }
            return null;
        }

        /// <summary>
        /// method to Create new flight,bus, hotel group 
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<string> UpdateGroupRow(CompanyCommissionGroupRow groupRow)
        {
            groupRow.action = "UPDATE_COMMISSION_GROUP_STRUCTURE";
            string data = JsonConvert.SerializeObject(groupRow);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == SUCCESS)
            {
                return response.APISTATUS;
            }
            return null;
        }
        #endregion Service Group API

        #region White Label USer API


        /// <summary>
        /// method to get Recharge Commission Structure
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<IList<UserDetail>> GetCompanyUserList(string memberId)
        {
            var data = string.Empty;
            if (!string.IsNullOrEmpty(memberId))
            {
                data = "{\"action\":\"GET_USERS_LIST\",\"member_id\":" + memberId + "}";
            }
            else
            {
                data = "{\"action\":\"GET_USERS_LIST\",\"role_id\":3}";
            }

            var response = await CallFunction(data);
            if (response != null && response.GET_USERS_LIST != null)
            {
                return response.GET_USERS_LIST;
            }
            return null;
        }

        /// <summary>
        /// Get Flight, bus and hotel commission groups alloted to member
        /// </summary>
        /// <returns></returns>
        public static async Task<IList<Member_Allotted_group>> GetMemberAllottedGroup(string memberId, string companyId)
        {
            var data = "{\"action\":\"GET_MEMBER_ALLOTED_GROUPS\",\"member_id\":" + memberId + ",\"company_id\":" + companyId + "}";
            var response = await CallFunction(data);
            if (response != null && response.GET_MEMBER_ALLOTED_GROUPS != null)
            {
                return response.GET_MEMBER_ALLOTED_GROUPS;
            }
            return null;
        }
        #endregion White Label USer API

        #region Flight API
        public static async Task<ArrayOfFlightsDetail> FlightPricing(Request searchflight)
        {
            var flightprice = new ArrayOfFlightsDetail();
            using (var httpClient = new HttpClient())
            {
                string xmlstring = string.Empty;

                var serializer = new XmlSerializer(typeof(Request));

                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, searchflight);
                httpClient.DefaultRequestHeaders.Add("authKey", FlightAuthKey);
                var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Flight/FlightPricing", httpContent);

                // If the response contains content we want to read it!

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (responseContent.Contains("FAILED") || responseContent.Contains("FAIL"))
                    {
                        var response = JsonConvert.DeserializeObject<Response>(responseContent);
                    }
                    else
                    {
                        var serializer1 = new XmlSerializer(typeof(ArrayOfFlightsDetail));
                        string searchResponse = RemoveAllNamespacesPost(responseContent);
                        searchResponse = HttpUtility.HtmlDecode(searchResponse);
                        searchResponse = ReplaceBRwithNewline(searchResponse);
                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            flightprice = (ArrayOfFlightsDetail)serializer1.Deserialize(reader);
                        }

                        //return response;
                    }
                }
            }

            return flightprice;
        }

        public static async Task<ArrayOfFlightsDetail> SearchFlight(Request searchflight)
        {
            var flightList = new ArrayOfFlightsDetail();
            using (var httpClient = new HttpClient())
            {
                string xmlstring = string.Empty;

                var serializer = new XmlSerializer(typeof(Request));
                httpClient.DefaultRequestHeaders.Add("authKey", FlightAuthKey);

                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, searchflight);

                var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Flight/SearchFlight", httpContent);

                // If the response contains content we want to read it!

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (responseContent.Contains("FAILED") || responseContent.Contains("FAIL"))
                    {
                        var response = JsonConvert.DeserializeObject<Response>(responseContent);
                    }
                    else
                    {

                        var serializer1 = new XmlSerializer(typeof(ArrayOfFlightsDetail));
                        string searchResponse = RemoveAllNamespacesPost(responseContent);
                        searchResponse = HttpUtility.HtmlDecode(searchResponse);
                        searchResponse = ReplaceBRwithNewline(searchResponse);

                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            flightList = (ArrayOfFlightsDetail)serializer1.Deserialize(reader);
                        }
                    }
                }
            }

            return flightList;
        }

        /// <summary>
        /// method to get flight city list
        /// </summary>
        /// <returns></returns>
        public static async Task<List<KeyValuePair>> GetFlightCityList(bool isFlight)
        {
            var cityList = new List<KeyValuePair>();
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse;
                if (isFlight)
                {
                    // Do the actual request and await the response
                    httpResponse = await httpClient.GetAsync("http://wlapi.bisplindia.in/api/Flight/GetCityList");
                }
                else
                {
                    httpResponse = await httpClient.GetAsync("http://wlapi.bisplindia.in/api/Hotel/GetHotelCityList");
                }

                // If the response contains content we want to read it!

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (responseContent.Contains("FAILED") || responseContent.Contains("FAIL"))
                    {
                        var response = JsonConvert.DeserializeObject<Response>(responseContent);
                    }
                    else
                    {
                        Console.WriteLine(responseContent);
                        string searchResponse = RemoveAllNamespaces(responseContent);
                        var serializer = new XmlSerializer(typeof(List<KeyValuePair>));
                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            cityList = (List<KeyValuePair>)serializer.Deserialize(reader);
                        }
                    }
                }
            }
            return cityList;
        }

        public static async Task<Bookingresponse> BookTicket(Request BookTicket)
        {
            var bookResponse = new Bookingresponse();
            using (var httpClient = new HttpClient())
            {
                string xmlstring = string.Empty;

                var serializer = new XmlSerializer(typeof(Request));
                httpClient.DefaultRequestHeaders.Add("authKey", FlightAuthKey);
                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, BookTicket);
                
                var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Flight/BookTicket", httpContent);

                // If the response contains content we want to read it!

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (responseContent.Contains("FAILED") || responseContent.Contains("FAIL"))
                    {
                        var response = JsonConvert.DeserializeObject<Response>(responseContent);
                    }
                    else
                    {
                        var serializer1 = new XmlSerializer(typeof(Bookingresponse));
                        string searchResponse = RemoveAllNamespacesPost(responseContent);
                        searchResponse = HttpUtility.HtmlDecode(searchResponse);
                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            bookResponse = (Bookingresponse)serializer1.Deserialize(reader);
                        }
                    }
                }
            }

            return bookResponse;
        }

        public static async Task<CancelationDetails> CancelFlightTicket(EticketRequest BookTicket)
        {
            var cancelResponse = new CancelationDetails();
            using (var httpClient = new HttpClient())
            {
                string xmlstring = string.Empty;

                var serializer = new XmlSerializer(typeof(EticketRequest));
                httpClient.DefaultRequestHeaders.Add("authKey", FlightAuthKey);
                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, BookTicket);

                var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Flight/CancelTicket", httpContent);

                // If the response contains content we want to read it!

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (responseContent.Contains("FAILED") || responseContent.Contains("FAIL"))
                    {
                        var response = JsonConvert.DeserializeObject<Response>(responseContent);
                    }
                    else
                    {
                        var serializer1 = new XmlSerializer(typeof(CancelationDetails));
                        string searchResponse = RemoveAllNamespacesPost(responseContent);
                        searchResponse = HttpUtility.HtmlDecode(searchResponse);
                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            cancelResponse = (CancelationDetails)serializer1.Deserialize(reader);
                        }
                    }
                }
            }
            return cancelResponse;
        }


        public static async Task<EticketDetails> FlightCancelStatus(EticketRequest BookTicket)
        {
            var cancelResponse = new EticketDetails();
            using (var httpClient = new HttpClient())
            {
                string xmlstring = string.Empty;

                var serializer = new XmlSerializer(typeof(EticketRequest));
                httpClient.DefaultRequestHeaders.Add("authKey", FlightAuthKey);
                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, BookTicket);

                var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Flight/CancelRequestStatus", httpContent);

                // If the response contains content we want to read it!

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (responseContent.Contains("FAILED") || responseContent.Contains("FAIL"))
                    {
                        var response = JsonConvert.DeserializeObject<Response>(responseContent);
                    }
                    else
                    {
                        var serializer1 = new XmlSerializer(typeof(EticketDetails));
                        string searchResponse = RemoveAllNamespacesPost(responseContent);
                        searchResponse = HttpUtility.HtmlDecode(searchResponse);
                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            cancelResponse = (EticketDetails)serializer1.Deserialize(reader);
                        }
                    }
                }
            }
            return cancelResponse;
        }

        public static async Task<EticketDetails> FlightTicketStatus(EticketRequest BookTicket)
        {
            var cancelResponse = new EticketDetails();
            using (var httpClient = new HttpClient())
            {
                string xmlstring = string.Empty;

                var serializer = new XmlSerializer(typeof(EticketRequest));
                httpClient.DefaultRequestHeaders.Add("authKey", FlightAuthKey);
                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, BookTicket);

                var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Flight/BookingStatus", httpContent);

                // If the response contains content we want to read it!

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (responseContent.Contains("FAILED") || responseContent.Contains("FAIL"))
                    {
                        var response = JsonConvert.DeserializeObject<Response>(responseContent);
                    }
                    else
                    {
                        var serializer1 = new XmlSerializer(typeof(EticketDetails));
                        string searchResponse = RemoveAllNamespacesPost(responseContent);
                        searchResponse = HttpUtility.HtmlDecode(searchResponse);
                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            cancelResponse = (EticketDetails)serializer1.Deserialize(reader);
                        }
                    }
                }
            }
            return cancelResponse;
        }


        #endregion  Flight API

    }
}
