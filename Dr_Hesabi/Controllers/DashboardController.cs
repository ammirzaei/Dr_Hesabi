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
    [Authorize]
    [AutoValidateAntiforgeryToken]
    [RequireHttps]
    public class DashboardController : Controller
    {
        private readonly IProfile _IProfile;
        private readonly ISetting _ISetting;
        public DashboardController(IProfile iProfile, ISetting iSetting)
        {
            _IProfile = iProfile;
            _ISetting = iSetting;
        }
        public enum ConditionProfileRequest
        {
            Null,
            True,
            False,
            Awit
        }
        public async Task<IActionResult> Index()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();

            if (await _IProfile.IsUserStaff(UserID))
                ViewData["IsUserStaff"] = true;
            else
            {
                if (!await _IProfile.ExistProfileStudent(UserID))
                {
                    return RedirectToAction(nameof(AddInfo));
                }
                ViewData["IsUserStaff"] = false;
            }
            return View();
        }

        private async Task<ConditionProfileRequest> GetConditionProfileRequest(string UserID)
        {
            ConditionProfileRequest condition = ConditionProfileRequest.Null;
            if (await _IProfile.ExistProfileRequest(UserID))
            {
                bool? IsCondition = await _IProfile.IsConditionProfileRequest(UserID);
                if (IsCondition == true)
                    condition = ConditionProfileRequest.True;
                else if (IsCondition == false)
                    condition = ConditionProfileRequest.False;
                else
                    condition = ConditionProfileRequest.Awit;
            }
            return condition;
        }

        public IActionResult AddInfo()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value
                .ToString();
            return View(new ProfileStudents()
            {
                UserID = UserID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInfo(ProfileStudents profile)
        {
            if (ModelState.IsValid)
            {
                if (!CheckKodeMeli.Check(profile.CodeMeli))
                {
                    ModelState.AddModelError("CodeMeli", "کد ملی وارد شده معتبر نیست");
                    return View(profile);
                }

                profile.IsCondition = null;
                profile.ProfileID = CodeGeneratore.ActiveCode();
                await _IProfile.AddProfile(profile);
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }

        public async Task<IActionResult> EditInfo()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value
                .ToString();
            var profile = await _IProfile.GetProfile(UserID);
            if (profile != null)
            {
                if (profile.IsCondition == true && await _IProfile.IsConditionProfileRequest(UserID) != true)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(profile);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfo(ProfileStudents profile)
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value
                .ToString();
            if (profile.IsCondition == true && await _IProfile.IsConditionProfileRequest(UserID) != true)
            {
                return RedirectToAction(nameof(RequestProfile));
            }
            if (ModelState.IsValid)
            {
                if (!CheckKodeMeli.Check(profile.CodeMeli))
                {
                    ModelState.AddModelError("CodeMeli", "کد ملی وارد شده معتبر نیست");
                    return View(profile);
                }

                if (profile.IsCondition == false)
                    profile.IsCondition = null;
                await _IProfile.UpdateProfile(profile);
                if (profile.IsCondition == true)
                    await _IProfile.DeleteProfileRequest(UserID);

                return RedirectToAction(nameof(Index));
            }
            return View(await _IProfile.GetProfile(UserID));
        }

        public async Task<IActionResult> RequestProfile()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value
                .ToString();
            var profile = await _IProfile.GetProfile(UserID);
            if (profile != null)
            {
                if (profile.IsCondition == true)
                {
                    var condition = await GetConditionProfileRequest(UserID);
                    if (condition == ConditionProfileRequest.Awit || condition == ConditionProfileRequest.True)
                        return RedirectToAction(nameof(Index));
                }
                return View(new ProfileRequests()
                {
                    UserID = UserID
                });
            }
            return RedirectToAction(nameof(Index));
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

        [HttpPost]
        public async Task<IActionResult> RequestProfile(ProfileRequests profileRequest, IFormCollection form)
        {
            if (ReCapcha(form, out var view)) return view;
            if (ModelState.IsValid)
            {
                profileRequest.IsCondition = null;
                profileRequest.CreateDate = DateTime.Now;
                profileRequest.ProfileRequestID = CodeGeneratore.ActiveCode();
                await _IProfile.AddProfileRequest(profileRequest);
                return RedirectToAction(nameof(Index));
            }
            return View(profileRequest);
        }

        [Route("Dashboard/TestsUltimate")]
        public async Task<IActionResult> ShowTestUltimate()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value
                .ToString();
            await _IProfile.CheckUltimate(UserID);
            return View(_IProfile.GetAllUltimate(UserID));
        }

        public async Task<IActionResult> Guide()
        {
            var setting = await _ISetting.GetSetting();
            List<string> ListGuide = new List<string>();
            if (HttpContext.User.Claims.FirstOrDefault(s => s.Type.Contains("Role")).Value.Contains("Teacher"))
            {
                if (setting.GuideTeacher != null)
                {
                    ListGuide = setting.GuideTeacher.Split(',').ToList();
                }
            }
            else
            {
                if (setting.GuideStudent != null)
                {
                    ListGuide = setting.GuideStudent.Split(',').ToList();
                }
            }

            return View(ListGuide.ToList());
        }
    }
}
