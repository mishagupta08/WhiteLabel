using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShineYatraAdmin.Common
{
    public static class Common
    {
        public static string getServiceNamebyId(string id)
        {
            string service_Name = string.Empty;
            switch (id)
            {
                case "1": service_Name = "FLIGHT"; break;
                case "2": service_Name = "HOTEL"; break;
                case "3": service_Name = "BUS"; break;
                case "4": service_Name = "RECHARGE"; break;
                case "5": service_Name = "CAB"; break;
                case "6": service_Name = "BILL SERVICE"; break;
            }
            return service_Name;
        }

        public static string getIdbyServiceName(string service_Name)
        {
            string id = string.Empty;
            switch (service_Name.ToUpper().Trim())
            {
                case "FLIGHT": id = "1"; break;
                case "HOTEL": id = "2"; break;
                case "BUS": id = "3"; break;
                case "RECHARGE": id = "4"; break;
                case "CAB": id = "5"; break;
                case "BILL SERVICE": id = "6"; break;
            }
            return id;
        }
    }
}