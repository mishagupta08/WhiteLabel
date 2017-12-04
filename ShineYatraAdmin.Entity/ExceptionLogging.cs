using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Net.Http;

/// <summary>
/// Summary description for ExceptionLogging
/// </summary>
public static class ExceptionLogging
{
    public static void SendErrorTomail(Exception exmail, string userData, string domain)
    {
        var fromAddress = new MailAddress("guptamisha88@gmail.com");
        var toAddress = new MailAddress("guptamisha88@gmail.com");
        const string fromPassword = "octoberssmisha";
        try
        {
            string[] userDataArray = null;
            string userDetail = string.Empty;
            if (!string.IsNullOrEmpty(userData))
            {
                 userDataArray = userData.Split('|');
            }
            string msg = string.Empty;
            var newline = "<br/>";            
            var extype = exmail.GetType().Name.ToString();            
            var EmailHead = "An exception occurred in a Application " + newline + newline;
            if (userDataArray != null)
            {
                userDetail = "Member ID: " + userDataArray[1] + newline + "Member Name: " + userDataArray[3] + newline;
            }
            var companyDetail = "Company Domain: " + domain;
            var EmailSing = newline + "Thanks and Regards" + newline + "    " + "     " + "<b>Application Admin </b>" + "</br>";
            var Sub = "Exception occurred in Application";            
            string errortomail = EmailHead + "<b>Log Written Date: </b>" + " " + DateTime.Now.ToString() + newline+ userDetail + "<b>Exception Type:</b>" + " " + extype + newline + "<b>Error Message:</b>" + " " + exmail.Message + newline + "<b> Error Details :</b>" + " " + exmail.StackTrace  + newline + newline + newline + newline + EmailSing;
            
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = Sub,
                Body = errortomail,
                IsBodyHtml = true,
            })
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                
                //message.CC.Add("bharatlakhani@yahoo.com");                
                smtp.Send(message);
            }
        }
        catch (Exception ex)
        {
            Console.Write("Fail Has error" + ex.Message);
        }

    }
   
        public static void SendAPIErrorTomail(string request, string response)
        {
            var fromAddress = new MailAddress("guptamisha88@gmail.com");
            var toAddress = new MailAddress("guptamisha88@gmail.com");
            const string fromPassword = "octoberssmisha";
            try
            {                
                string msg = string.Empty;
                var newline = "<br/>";                
                var EmailHead = "An exception occurred in a API Application " + newline + newline;                
                var EmailSing = newline + "Thanks and Regards" + newline + "    " + "     " + "<b>Application Admin </b>" + "</br>";
                var Sub = "Exception occurred in Database API";
                string errortomail = EmailHead + "<b>Log Written Date: </b>" + " " + DateTime.Now.ToString() + newline + "<b>Request :</b> "+ request + newline + "<b>Response :</b> " + response  + newline + newline + EmailSing;

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = Sub,
                    Body = errortomail,
                    IsBodyHtml = true,
                })
                {
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = true,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };

                    //message.CC.Add("bharatlakhani@yahoo.com");                
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.Write("Fail Has error" + ex.Message);
            }

        }

    }