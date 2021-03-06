﻿using System.Data;

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
    using System.Web;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml.Linq;
    using System.Configuration;
    using Entity.HotelDetail;

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
        private const string Success = "SUCCESS";

        /// <summary>
        /// Base url of API
        /// </summary>
        private const string LoginApiUrl = "http://wlapi.bisplindia.in/api/Login/";

        /// <summary>
        /// Base url of API
        /// </summary>
        private const string ApiBaseUrl = "http://mukesh.bisplindia.in/apiRouter.aspx";

        /// <summary>
        /// Authentication key
        /// </summary>
        private const string AuthKey = "lPJpfNMUK6u2KAGyJXqxsw==";

        /// <summary>
        /// Authentication key
        /// </summary>
        private const string FlightAuthKey = "e969da44-91f8-4d51-b138-0ace0980d519";

        private const string ValidateUserAction = "ValidateUser";

        private const string ValidateLoginAction = "ValidateLogin";

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

            return xmlDocumentWithoutNs.Value;
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName) { Value = xmlDocument.Value };

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
            bool ApiIntegrated = Convert.ToBoolean(ConfigurationManager.AppSettings["ApiIntegrated"]);
            bool isLocalApiRun = false;
            UserDetail userDetail = null;

            if (ApiIntegrated)
            {
                loginDetail.action = ValidateUserAction;
                string data = JsonConvert.SerializeObject(loginDetail);
                var response = await CallLoginFunction(data, ValidateUserAction);
                if (response != null && response.Status == true)
                {
                    var dtUser = JsonConvert.DeserializeObject<DTUserDetails>(response.ResponseValue);
                    if (dtUser != null)
                    {
                        userDetail = new UserDetail();
                        userDetail.user_name = dtUser.loginid;
                        userDetail.pwd = loginDetail.password;
                        userDetail.member_id = dtUser.loginid;
                        userDetail.first_name = dtUser.name;
                        userDetail.emailId = dtUser.email;
                        userDetail.mobileNo = dtUser.mobileno;
                        userDetail.kitid = dtUser.kitid;
                        userDetail.doj = dtUser.doj;
                    }
                    if (userDetail != null)
                    {
                        userDetail.doj = DateTime.Now.ToString("dd-MMM-yyyy");
                        if (string.IsNullOrEmpty(userDetail.kitid))
                        {
                            userDetail.kitid = "0";
                        }

                        if (string.IsNullOrEmpty(userDetail.ref_id))
                        {
                            userDetail.ref_id = "0";
                        }

                        userDetail.user_type = "DISTB";
                        var loginDetailJson = "{'action':'USER_LOGIN_UPDATE','domain_name':'" + ConfigurationManager.AppSettings["DomainName"] + "','user_type':'" + userDetail.user_type + "','ref_id':'" + userDetail.ref_id + "','user_name':'" + userDetail.user_name + "','password':'" + userDetail.pwd + "','first_name':'" + userDetail.first_name + "','last_name':'','mobileno':'" + userDetail.mobileNo + "','email':'" + userDetail.emailId + "','doj':'" + userDetail.doj + "','kitid':" + userDetail.kitid + "}";
                        var updateresponse = await CallFunction(loginDetailJson);
                        if (updateresponse == null)
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    isLocalApiRun = true;
                }
            }
            else
            {
                isLocalApiRun = true;
            }

            if (isLocalApiRun)
            {
                loginDetail.action = ValidateLoginAction;
                string data = JsonConvert.SerializeObject(loginDetail);
                var response = await CallFunction(data);
                if (response != null && response.VALIDATELOGIN != null && response.VALIDATELOGIN.Count > 0)
                {
                    userDetail = response.VALIDATELOGIN.FirstOrDefault();
                }
            }
            return userDetail;
        }

        #endregion Login API

        /// <summary>
        /// method to save booking details to database
        /// </summary>        
        /// <param name="bookticket"></param>
        /// <returns></returns>
        public static async Task<List<INSERT_SERVICE_HOTEL_REQUEST>> InsertHotelServiceBookingRequest(HotelBoookingDetail bookticket)
        {
            string data = JsonConvert.SerializeObject(bookticket);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == Success && response.INSERT_SERVICE_HOTEL_REQUEST != null)
            {
                return response.INSERT_SERVICE_HOTEL_REQUEST;
            }
            else if (response?.INSERT_SERVICE_HOTEL_REQUEST != null)
            {
                return response.INSERT_SERVICE_HOTEL_REQUEST;
            }
            return null;
        }

        /// <summary>
        /// Method to invoke api function
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static async Task<Response> CallLoginFunction(string data, string action)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                    var httpContent = new StringContent(data, Encoding.UTF8, "application/json");
                    //httpClient.DefaultRequestHeaders.Add("Key", AuthKey);

                    // Do the actual request and await the response
                    var httpResponse = await httpClient.PostAsync(LoginApiUrl + action, httpContent);

                    // If the response contains content we want to read it!

                    if (httpResponse.Content != null)
                    {
                        var responseContent = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        var response = JsonConvert.DeserializeObject<Response>(responseContent);
                        return response;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return null;
        }

        #region Primary Setting API
        /// <summary>
        /// method to get Primary Settings
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<IList<CompanyCommissionGroup>> GetPrimarySetting(string serviceId, string sub_service,string userDetail)
        {
            var data = string.Empty;
            var arrUserData = userDetail.Split('|');
            //Code for get services           
            data = "{\"action\":\"GET_SERVICE_DETAILS\",\"SERVICE_ID\":\"" + serviceId + "\",\"member_id\":\"" + arrUserData[1] + "\",\"company_id\":\"" + arrUserData[2] + "\"}";
            if (!string.IsNullOrEmpty(sub_service))
            {
                data = "{\"action\":\"GET_SERVICE_DETAILS\",\"SERVICE_ID\":\"" + serviceId + "\",\"sub_category\":\"" + sub_service + "\",\"member_id\":\"" + arrUserData[1] + "\",\"company_id\":\"" + arrUserData[2] + "\"}";
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
            if (response != null && response.APISTATUS == Success)
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
            if (response != null && response.APISTATUS == Success)
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
            if (response != null && response.APISTATUS == Success)
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
                if (response != null && response.APISTATUS == Success)
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
            if (response != null && response.APISTATUS == Success)
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
                data = "{\"action\":\"GET_MEMBER_SELF_AND_ALLOTED_GROUPS\",\"service_id\":\"" + serviceId + "\",\"category\":\"" + service_type + "\",\"member_id\":\"" + memberId + "\",\"sub_category\":\"" + sub_category + "\"}";
            }
            var response = await CallFunction(data);
            if (response != null && response.GET_MEMBER_SELF_AND_ALLOTED_GROUPS != null)
            {
                return response.GET_MEMBER_SELF_AND_ALLOTED_GROUPS;
            }
            return null;
        }

        /// <summary>
        /// method to get all the price groups (commission structures group) for a service
        /// </summary>        
        /// <returns></returns>

        public static async Task<IList<CompanyCommissionGroup>> GetGroupList(string serviceId, string memberId, string service_type, string sub_category)
        {
            var data = string.Empty;
            if (!string.IsNullOrEmpty(serviceId) && !string.IsNullOrEmpty(memberId))
            {
                data = "{\"action\":\"GET_COMMISSION_GROUPS_ALLOTEMENT_CHOICES\",\"service_id\":\"" + serviceId + "\",\"category\":\"" + service_type + "\",\"member_id\":\"" + memberId + "\",\"sub_category\":\"" + sub_category + "\"}";
            }
            var response = await CallFunction(data);
            if (response != null && response.GET_COMMISSION_GROUPS_ALLOTEMENT_CHOICES != null)
            {
                return response.GET_COMMISSION_GROUPS_ALLOTEMENT_CHOICES;
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
        public static async Task<string> EditCompanyServiceGroup(int company_id, int price_group_id, int service_id, string member_id, string category, string sub_category)
        {
            string data = "{\"action\":\"UPDATE_ALLOTMENT_COMMISSION_GROUP\",\"company_id\": " + company_id + ",\"price_group_id\":" + price_group_id + ",\"service_id\": " + service_id + ",\"member_id\": " + member_id + ",\"category\": \"" + category + "\",\"sub_category\": \"" + sub_category + "\"}";
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == Success)
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
            if (response != null && response.APISTATUS == Success)
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
            if (response != null && response.APISTATUS == Success)
            {
                return response.APISTATUS;
            }
            else if (response != null)
            {
                return response.MSG;
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
        /// method to get Recharge Commission Structure
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<IList<UserDetail>> GetMemberUsersList(string memberId, string roleId)
        {
            var data = string.Empty;
            if (!string.IsNullOrEmpty(memberId))
            {
                data = "{\"action\":\"GET_USERS_LIST\",\"ref_id\":" + memberId + ",\"role_id\":" + roleId + "}";
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
        public static async Task<IList<Member_Allotted_group>> GetMemberAllottedGroup(string memberId, string companyId, string serviceid, string category, string sub_category)
        {
            var data = "{\"action\":\"GET_MEMBER_ALLOTED_GROUPS\",\"member_id\":" + memberId + ",\"company_id\":" + companyId;
            if (!string.IsNullOrEmpty(serviceid))
            {
                data = data + ",\"service_id\":" + serviceid;
            }
            if (!string.IsNullOrEmpty(category))
            {
                data = data + ",\"category\":\"" + category + "\"";
            }
            if (!string.IsNullOrEmpty(sub_category))
            {
                data = data + ",\"sub_category\":\"" + sub_category + "\"";
            }

            data = data + "}";

            var response = await CallFunction(data);
            if (response != null && response.GET_MEMBER_ALLOTED_GROUPS != null)
            {
                return response.GET_MEMBER_ALLOTED_GROUPS;
            }
            return null;
        }

        /// <summary>
        /// Get Flight, bus and hotel commission groups alloted to member
        /// </summary>
        /// <returns></returns>
        public static async Task<IList<CompanyCommissionGroup>> GetServiceAllottedGroupDetails(AllotedServiceCGsDetailsRequest allotedServiceCGsDetailsRequest)
        {
            var data = JsonConvert.SerializeObject(allotedServiceCGsDetailsRequest);

            var response = await CallFunction(data);
            if (response != null && response.GET_ALLOTED_SERVICE_COMMISSION_GROUPS_DETAILS != null)
            {
                return response.GET_ALLOTED_SERVICE_COMMISSION_GROUPS_DETAILS;
            }
            return null;
        }

        #endregion White Label USer API

        #region Flight API
        /// <summary>
        /// get details of Fare for a particular flight
        /// </summary>
        /// <param name="searchflight"></param>
        /// <returns></returns>
        public static async Task<OriginDestinationOption> FlightPricing(Request searchflight)
        {
            var flightprice = new OriginDestinationOption();
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
                        var serializer1 = new XmlSerializer(typeof(OriginDestinationOption));
                        string searchResponse = RemoveAllNamespacesPost(responseContent);
                        searchResponse = HttpUtility.HtmlDecode(searchResponse);
                        searchResponse = ReplaceBRwithNewline(searchResponse);
                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            try
                            {
                                flightprice = (OriginDestinationOption)serializer1.Deserialize(reader);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.InnerException);
                            }
                        }

                        //return response;
                    }
                }
            }

            return flightprice;
        }

        /// <summary>
        /// search flight btn source and destination
        /// </summary>
        /// <param name="searchflight"></param>
        /// <returns></returns>
        public static async Task<ArrayOfOrigindestinationoption> SearchFlight(Request searchflight)
        {
            var flightList = new ArrayOfOrigindestinationoption();
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

                        var serializer1 = new XmlSerializer(typeof(ArrayOfOrigindestinationoption));
                        string searchResponse = RemoveAllNamespacesPost(responseContent);
                        searchResponse = HttpUtility.HtmlDecode(searchResponse);
                        searchResponse = ReplaceBRwithNewline(searchResponse);

                        using (TextReader reader = new StringReader(searchResponse))
                        {
                            flightList = (ArrayOfOrigindestinationoption)serializer1.Deserialize(reader);
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
        /// <summary>
        /// flight ticket booking
        /// </summary>
        /// <param name="bookTicket"></param>
        /// <returns></returns>
        public static async Task<Bookingresponse> BookTicket(Request bookTicket)
        {
            var bookResponse = new Bookingresponse();
            using (var httpClient = new HttpClient())
            {
                string xmlstring = string.Empty;

                var serializer = new XmlSerializer(typeof(Request));
                httpClient.DefaultRequestHeaders.Add("authKey", FlightAuthKey);
                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, bookTicket);

                var httpContent = new StringContent(stringwriter.ToString(), Encoding.UTF8, "application/xml");
                var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Flight/BookTicket", httpContent);

                // If the response contains content we want to read it!

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    if (!responseContent.ToUpper().Contains("SUCCESS"))
                    {
                        bookResponse.Status = HttpUtility.HtmlDecode(responseContent);
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
                var httpResponse = await httpClient.PostAsync("http://wlapi.bisplindia.in/api/Flight/CancelRequest", httpContent);

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

        /// <summary>
        /// Get flight ticket status
        /// </summary>
        /// <param name="bookTicket"></param>
        /// <returns></returns>
        public static async Task<EticketDetails> FlightTicketStatus(EticketRequest bookTicket)
        {
            var cancelResponse = new EticketDetails();
            using (var httpClient = new HttpClient())
            {
                string xmlstring = string.Empty;

                var serializer = new XmlSerializer(typeof(EticketRequest));
                httpClient.DefaultRequestHeaders.Add("authKey", FlightAuthKey);
                var stringwriter = new System.IO.StringWriter();
                serializer.Serialize(stringwriter, bookTicket);

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

        /// <summary>
        /// method to save booking details to database
        /// </summary>        
        /// <param name="bookticket"></param>
        /// <returns></returns>
        public static async Task<List<INSERT_SERVICE_BOOKING_REQUEST>> InsertServiceBookingRequest(BookingDetail bookticket)
        {
            string data = JsonConvert.SerializeObject(bookticket);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == Success && response.INSERT_SERVICE_BOOKING_REQUEST != null)
            {
                return response.INSERT_SERVICE_BOOKING_REQUEST;
            }
            else if (response?.INSERT_SERVICE_BOOKING_REQUEST != null)
            {
                return response.INSERT_SERVICE_BOOKING_REQUEST;
            }
            return null;
        }

        /// <summary>
        /// Get details of flight from database
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public static async Task<List<HotelBookingContainer>> GetServiceBookingRequest(string transactionId, string memberId)
        {
            string data = "{\"action\":\"GET_FLIGHT_TRANSACTIONS\",\"member_id\":" + memberId + ",\"txn_id\":" + transactionId + "}";
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == Success && response.GET_FLIGHT_TRANSACTIONS != null)
            {
                return response.GET_FLIGHT_TRANSACTIONS;
            }
            return null;
        }

        /// <summary>
        /// method to get booking details to database
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<UPDATE_TRANSACTION_STATUS> UpdateServiceBookingRequest(string TransactionId, string memberId, string api_txn_id, string status)
        {
            string data = "{\"action\":\"UPDATE_TRANSACTION_STATUS\",\"update_member_id\":\"" + memberId + "\",\"txn_id\":" + TransactionId + ",\"status\":\"" + status + "\",\"api_txn_id\":\"" + api_txn_id + "\",\"deposit_mode\":\"\",\"bank_name\":\"\",\"remarks\":\"\"}";
            var response = await CallFunction(data);
            UPDATE_TRANSACTION_STATUS updatestatus = new UPDATE_TRANSACTION_STATUS();

            if (response != null && response.APISTATUS == Success && response.UPDATE_TRANSACTION_STATUS != null)
            {
                return response.UPDATE_TRANSACTION_STATUS;
            }
            else if (response != null && response.MSG != null)
            {
                updatestatus.MSG = response.MSG;
                return updatestatus;
            }
            return null;
        }

        /// <summary>
        /// method to get company wallet balance
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<List<WalletResponse>> GET_WALLET_BALANCE(WalletRequest request)
        {
            string data = JsonConvert.SerializeObject(request);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == Success && response.GET_WALLET_BALANCE != null)
            {
                return response.GET_WALLET_BALANCE;
            }
            else if (response != null)
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// method to save rehargee details to database
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<List<InsertServiceRechargeResponse>> SaveRechargeRequest(InsertServiceRechargeRequest request)
        {
            string data = JsonConvert.SerializeObject(request);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == Success && response.INSERT_SERVICE_RECHARGE_REQUEST != null)
            {
                return response.INSERT_SERVICE_RECHARGE_REQUEST;
            }
            else if (response != null)
            {
                return null;
            }
            return null;
        }



        #endregion  Flight API

        /************/

        /// <summary>
        /// Method to validate user
        /// </summary>
        /// <param name="loginDetail"></param>
        /// <returns></returns>
        public static async Task<DTUserDetails> WalletFunction(TransactionDetail detail)
        {
            string data = JsonConvert.SerializeObject(detail);
            DTUserDetails result = null;
            var response = await CallLoginFunction(data, detail.action);
            if (response != null && response.Status == true)
            {
                result = JsonConvert.DeserializeObject<DTUserDetails>(response.ResponseValue);
            }

            return result;
        }

        /// <summary>
        /// get list of subservices for a service
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<string> WalletCreditRequest(FundRequestContainer fundDetail)
        {
            string data = JsonConvert.SerializeObject(fundDetail);

            var response = await CallFunction(data);
            if (response != null && response.APISTATUS == Success && response.WALLET_CREDIT_3RD_PARTY_REQUEST != null && response.WALLET_CREDIT_3RD_PARTY_REQUEST.Count > 0)
            {
                var txnId = response.WALLET_CREDIT_3RD_PARTY_REQUEST.FirstOrDefault().txn_id;
                return txnId;
            }

            return null;
        }

        /// <summary>
        /// get list of subservices for a service
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static async Task<string> UpdateWalletCreditRequest(FundRequestContainer fundDetail)
        {
            string data = JsonConvert.SerializeObject(fundDetail);

            var response = await CallFunction(data);
            if (response != null)
            {
                if (response.APISTATUS == Success)
                {
                    return "Fund Transfer successfully.";
                }

                return response.MSG;
            }

            return "No Result Found.";
        }

        /// <summary>
        /// get list of fund request from members
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="txn_type"></param>
        /// <param name="member"></param>
        /// <param name="status"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<List<CompanyFund>> getFundRequestList(string member, string memberId, string txn_type, string status, string id)
        {
            string data = "{ \"action\":\"GET_FUND_REQUEST\",\"service_id\":\"" + id + "\",\"" + member + "\":" + memberId + ",\"txn_type\":\"" + txn_type + "\"";
            if (!string.IsNullOrEmpty(status))
            {
                data += ",\"status\":\"" + status + "\"}";
            }
            else
            {
                data += "}";
            }

            var response = await CallFunction(data);
            if (response != null)
            {
                if (response.APISTATUS == Success && response.GET_FUND_REQUEST != null)
                {
                    return response.GET_FUND_REQUEST;
                }
                return null;
            }
            return null;
        }

        /// <summary>
        ///  update fund request status
        /// </summary>
        /// <param name="txnid"></param>
        /// <param name="memberid"></param>
        /// <param name="status"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        public static async Task<string> UpdateFundRequest(int txnid, string memberid, string status, string remarks)
        {
            string data = "{\"action\":\"UPDATE_FUND_REQUEST\",\"txn_id\":" + txnid + ",\"member_id\":" + memberid + ",\"status\":\"" + status + "\",\"remarks\":\"" + remarks + "\"}";

            var response = await CallFunction(data);
            if (response != null)
            {
                if (response.APISTATUS == Success)
                {
                    return "SUCCESS";
                }

                return response.MSG;
            }

            return "No Result Found.";
        }

        /// <summary>
        ///  update fund request status
        /// </summary>
        /// <param name="txnid"></param>
        /// <param name="memberid"></param>
        /// <param name="status"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        public static async Task<string> UpdatePgFundRequest(int txnid, string memberid, string status, string remarks)
        {
            string data = "{\"action\":\"UPDATE_PG_REQUEST\",\"txn_id\":" + txnid + ",\"member_id\":" + memberid + ",\"status\":\"" + status + "\",\"remarks\":\"" + remarks + "\"}";

            var response = await CallFunction(data);
            if (response != null)
            {
                if (response.APISTATUS == Success)
                {
                    return "SUCCESS";
                }

                return response.MSG;
            }

            return "No Result Found.";
        }

        /// <summary>
        /// method to save fund detail while payment from payment gateway
        /// </summary>
        /// <param name="fundDetail"></param>
        /// <returns></returns>
        public static async Task<string> SavePaymntGatewayTransactions(CompanyFund fundDetail)
        {
            string data = JsonConvert.SerializeObject(fundDetail);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS.ToUpper().Trim() == Success)
            {
                var transaction = response.INSERT_PG_REQUEST_FOR_SERVICE.FirstOrDefault();
                if (transaction != null) return Convert.ToString(transaction.payment_txn_id);
            }
            else if (response != null)
            {
                return "Failed-" + response.MSG;
            }
            return null;
        }

        /// <summary>
        /// Get member flight detail list
        /// </summary>        
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<List<BookingDetail>> GetMemberFlightList(FlightBookingListRequest request)
        {
            List<BookingDetail> flightSummary = new List<BookingDetail>();
            try
            {
                string data = JsonConvert.SerializeObject(request);
                var response = await CallFunction(data);
                if (response != null && response.APISTATUS.ToUpper().Trim() == Success &&
                    response.GET_FLIGHT_TRANSACTIONS_SUMMARY != null)
                {
                    return response.GET_FLIGHT_TRANSACTIONS_SUMMARY;
                }
                else
                {
                    return flightSummary;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return flightSummary;
        }

        /// <summary>
        /// method to save fund detail while payment from payment gateway
        /// </summary>
        /// <returns></returns>
        public static async Task<string> AddUser(UserDetail userDetail)
        {
            var data = JsonConvert.SerializeObject(userDetail);
            data = data.Replace("null", "\"\"");
            var response = await CallFunction(data);
            if (response != null && response.APISTATUS.ToUpper().Trim() == Success)
                return response.APISTATUS;

            return response.MSG;
        }

        public static async Task<CompanySetting> GetCompanyExtraSetting(string domain)
        {
            var data = "{\"action\":\"GET_COMPANY_EXTRA_SETTINGS\",\"domain_name\":\"" + domain + "\"}";
            var response = await CallFunction(data);
            if (response != null && response.GET_COMPANY_EXTRA_SETTINGS != null && response.GET_COMPANY_EXTRA_SETTINGS.Count > 0)
            {
                return response.GET_COMPANY_EXTRA_SETTINGS.FirstOrDefault();
            }
            return null;
        }        
    }
}
