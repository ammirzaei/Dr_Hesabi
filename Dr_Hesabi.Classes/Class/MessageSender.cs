using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.Classes.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dr_Hesabi.Classes.Class
{
    public class MessageSender
    {
        public void SendEmail(string To, string Subject, string Body, HttpContext context)
        {
            try
            {
                ISetting ISetting = (ISetting)context.RequestServices.GetService(typeof(ISetting));
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();

                var Setting = ISetting.GetSetting();
                message.From = new MailAddress(Setting.Result.Email, Setting.Result.NameSite);
                message.To.Add(To);
                message.Body = Body;
                message.Subject = Subject;
                message.IsBodyHtml = true;

                client.Host = "mail.dr-hesabit.ir";
                client.Port = 25;
                client.EnableSsl = false;
                client.Credentials = new NetworkCredential(Setting.Result.Email, Setting.Result.PasswordEmail);

                client.Send(message);
            }
            catch
            {
                
            }
        }
    }
}
