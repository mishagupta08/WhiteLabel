using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShineYatraAdmin.Controllers
{
    public class OTPController : Controller
    {
        /// <summary>
        /// generate otp
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateOTP()
        {
            string otp = string.Empty;
            try {
                Random random = new Random();
                otp = random.Next(1000, 9999).ToString();
                string[] userData = User.Identity.Name.Split('|');
                string Number = "8107737208";//userData[4];
                string Message = "Your OTP code is - " + otp;                
                string URL = "http://bulksms.biztadka.com/submitsms.jsp?user=MUKESH1&key=e718f5c1ffXX&mobile=" + Number + "&message=" + Message + "&senderid=INFOSM&accusage=1";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string results = sr.ReadToEnd();
                sr.Close();
                Session["OTP"] = otp;
            }
            catch (Exception ex) {
                throw ex;
            }
            return Json(otp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerifyOTP(FormCollection frm)
        {
            string response = string.Empty;            
            try
            {
                string otp = Convert.ToString(Session["OTP"]);
                string userotp = Convert.ToString(frm.GetValue("otp").AttemptedValue);
                if (otp.Equals(userotp))
                {
                    response = "success";
                }
                else
                {
                    response = "error";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}