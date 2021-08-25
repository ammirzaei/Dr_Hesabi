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
    public class PanelController : Controller
    {
        private readonly IPanel _IPanel;
        private readonly IVisitDocument _IVisitDocument;
        private readonly ISetting _ISetting;
        public PanelController(IPanel iPanel, IVisitDocument iVisitDocument, ISetting setting)
        {
            _IPanel = iPanel;
            _IVisitDocument = iVisitDocument;
            _ISetting = setting;
        }

        [Route("Articles")]
        public async Task<IActionResult> Articles()
        {
            return View(await _IPanel.GetAllBlogs());
        }

        [Route("Article/{id}/{title}")]
        public async Task<IActionResult> ShowArticle(string id, string title)
        {
            Blogs blog = await _IPanel.GetBlog(id, title);
            if (blog == null)
            {
                return NotFound();
            }
            string IP = HttpContext.Connection.RemoteIpAddress.ToString();
            if (!await _IVisitDocument.ExistIP(IP, blog.BlogID))
            {
                await _IVisitDocument.AddVisit(IP, blog.BlogID);
                blog.Visit += 1;
                await _IPanel.UpdateBlog(blog);
            }
            return View(blog);
        }

        [Route("Bests")]
        public async Task<IActionResult> Bests()
        {
            return View(await _IPanel.GetAllBests());
        }

        [Route("Bests/{id}/{title}")]
        public async Task<IActionResult> ShowBest(string id, string title)
        {
            Bests best = await _IPanel.GetBest(id, title);
            if (best == null)
            {
                return NotFound();
            }
            ViewBag.Best = best;
            return View(await _IPanel.GetAllBestItems(id, title));
        }

        [Route("News")]
        public async Task<IActionResult> News()
        {
            return View(await _IPanel.GetAllNews());
        }

        [Route("News/{id}/{title}")]
        public async Task<IActionResult> ShowNews(string id, string title)
        {
            News news = await _IPanel.GetNews(id, title);
            if (news == null)
            {
                return NotFound();
            }
            string IP = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            if (!await _IVisitDocument.ExistIP(IP, news.NewsID))
            {
                await _IVisitDocument.AddVisit(IP, news.NewsID);
                news.Visit += 1;
                await _IPanel.UpdateNews(news);
            }
            return View(news);
        }
        [Route("Majors")]
        public IActionResult Major()
        {
            return ViewComponent("Majors", new { IsView = true });
        }

        [Route("Major/{title}")]
        public async Task<IActionResult> ShowMajor(string title)
        {
            Majors major = await _IPanel.GetMajor(title);
            if (major == null)
            {
                return NotFound();
            }
            return View(major);
        }

        [Route("Staffs")]
        public async Task<IActionResult> Staffs()
        {
            return View(await _IPanel.GetAllStaffs());
        }

        [Route("Staff/{id}/{title}")]
        public async Task<IActionResult> ShowStaff(string id, string title)
        {
            var staff = await _IPanel.GetStaff(id, title);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        [Route("Galleries")]
        public async Task<IActionResult> Gallerys()
        {
            return View(await _IPanel.GetAllGallerys());
        }

        [Route("Gallery/{id}/{title}")]
        public async Task<IActionResult> ShowGallery(string id, string title)
        {
            Gallerys gallerys = await _IPanel.GetGallery(id, title);
            if (gallerys == null)
            {
                return NotFound();
            }
            ViewData["GalleryTitle"] = gallerys.Title;
            return View(await _IPanel.GetImageGallery(id, title));
        }

        [Route("History")]
        public async Task<IActionResult> History()
        {
            Setting setting = await _ISetting.GetSetting();
            ViewData["History"] = setting.History;
            ViewData["ImgHistory"] = setting.ImgHistory;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CommandComment(Comments comments, IFormCollection form)
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
                    return null;
                }
            }

            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (comments.CommentID == null)
            {
                comments.UserID = UserID;
                comments.DateTime = DateTime.Now;
                comments.CommentID = CodeGeneratore.ActiveCode();   
                await _IPanel.AddComment(comments);
            }
            else
            {
                var Comment = await _IPanel.GetComment(comments.CommentID);
                Comment.Text = comments.Text;
                if (Comment.UserID == UserID)
                {
                    await _IPanel.UpdateComment(Comment);
                }
            }


            return ViewComponent("ListCommentViewComponent", new { id = comments.PanelID, Method = comments.Method });
        }

        [Authorize]
        public IActionResult DeleteComment(int id)
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteComment")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentConfirm(string id)
        {
            var Comment = await _IPanel.GetComment(id);
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (Comment.UserID == UserID)
            {
                await _IPanel.DeleteComment(Comment);
            }
            return ViewComponent("ListCommentViewComponent", new { id = Comment.PanelID, Method = Comment.Method });
        }

        public IActionResult ListComments(string id, string Method)
        {
            return ViewComponent("ListCommentViewComponent", new { id = id, Method = Method });
        }
    }
}
