using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Dr_Hesabi.Controllers
{
    [AutoValidateAntiforgeryToken]
    [RequireHttps]
    public class ConnectionsController : Controller
    {
        private readonly IConnections _IConnenctions;
        private readonly RenderToString.IViewRenderService _IViewRender;

        public ConnectionsController(IConnections iConnenctions, RenderToString.IViewRenderService iViewRender)
        {
            _IConnenctions = iConnenctions;
            _IViewRender = iViewRender;
        }
        public IActionResult Index(bool? Status)
        {
            ViewBag.Status = Status;
            string val = User.Identity.IsAuthenticated ? "a" : "";
            return View(new Connections()
            {
                IsSeen = false,
                EmailorPhone = val,
                FullName = val
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(Connections connection, IFormCollection form)
        {
            if (ReCapcha(form, out var view)) return view;
            if (ModelState.IsValid)
            {
                connection.CreateDate = DateTime.Now;
                connection.ConnectionID = CodeGeneratore.ActiveCode();
                if (User.Identity.IsAuthenticated)
                    connection.UserID = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
                await _IConnenctions.AddConnection(connection);

                ////Send Email Support
                await SendEmailSupport();

                return RedirectToAction(nameof(Index), new { Status = true });
            }
            return View(connection);
        }

        private bool ReCapcha(IFormCollection form, out IActionResult view)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = "6LcUrMoZAAAAANjiiP2rKZkv8cf9tqKH-5iDNjUJ"; // change this
            string gRecaptchaResponse = form["g-recaptcha-response"];
            var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;
            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                ViewBag.MessageError = "لطفا reCAPTCHA  را تأیید کنید";
                {
                    view = View();
                    return true;
                }
            }

            view = null;
            return false;
        }

        private async Task SendEmailSupport()
        {
            try
            {
                var emailSupport = await _IConnenctions.GetEmailSupport();
                if (emailSupport != null)
                {
                    string body = await _IViewRender.RenderToStringAsync("ManageEmail/EmailSupportConnection", null);
                    MessageSender ms = new MessageSender();
                    ms.SendEmail(emailSupport, "پیام جدید", body, HttpContext);
                }
            }
            catch
            {

            }
        }
    }
}
