using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreLinq.Extensions;
using Newtonsoft.Json;

namespace Dr_Hesabi.Controllers
{
    [AutoValidateAntiforgeryToken]
    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly IAccount _IAccount;
        private readonly RenderToString.IViewRenderService _IViewRender;
        private readonly ISetting _ISetting;
        public AccountController(IAccount iAccount, RenderToString.IViewRenderService iViewRender, ISetting iSetting)
        {
            _IAccount = iAccount;
            _IViewRender = iViewRender;
            _ISetting = iSetting;
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
        public IActionResult Register()
        {
            ViewData["Description"] = _ISetting.GetSetting().Result.ShortDescription;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register, IFormCollection form)
        {
            if (ReCapcha(form, out var view)) return view;
            if (ModelState.IsValid)
            {
                if (!await _IAccount.ExistUserUserName(register.UserName))
                {
                    if (!await _IAccount.ExistUserEmail(register.Email))
                    {
                        Users User = new Users()
                        {
                            UserID = CodeGeneratore.ActiveCode(),
                            UserName = register.UserName,
                            Password = HashGeneratore.MD5(register.Password),
                            Email = register.Email,
                            ActiveCode = CodeGeneratore.ActiveCode(),
                            Date = DateTime.Now,
                            IsActive = false,
                            RoleSelects = new List<RoleSelects>()
                            {
                                new RoleSelects()
                                {
                                    SelectID = CodeGeneratore.ActiveCode(),
                                    RoleID = await _IAccount.LastRoleID()
                                }
                            }
                        };

                        ////Send Email
                        MessageSender Ms = new MessageSender();
                        var Body = await _IViewRender.RenderToStringAsync("ManageEmail/EmailRegister", User.ActiveCode);
                        Ms.SendEmail(User.Email, "فعالسازی اکانت کاربری", Body, HttpContext);
                        await _IAccount.AddUser(User);

                        ViewBag.SuccessRegister = true;
                        return View(null);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "نام کاربری وارد شده تکراری است");
                }
            }
            return View(register);
        }
        public AuthenticationProperties Properties()
        {
            var propertise = new AuthenticationProperties()
            {
                IsPersistent = true
            };
            return propertise;
        }

        public ClaimsPrincipal Principal(string UserName, string UserID, List<string> Role)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,UserName),
                new Claim(ClaimTypes.NameIdentifier,UserID.ToString()),
                new Claim("Role",string.Join(",",Role))
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        public async Task<IActionResult> Login(string ReturnUrl = "/", string ActiveCode = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (ActiveCode != "")
            {
                var active = await _IAccount.ActiveCode(ActiveCode);
                if (active == true)
                {
                    ViewBag.IsActiveCode = true;
                }
                else
                {
                    ViewBag.IsActiveCode = false;
                }
            }
            ViewData["Description"] = _ISetting.GetSetting().Result.ShortDescription;
            return View(new LoginViewModel()
            {
                ReturnUrl = ReturnUrl
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login, IFormCollection form)
        {
            if (ReCapcha(form, out var view)) return view;

            if (ModelState.IsValid)
            {
                Users user = await _IAccount.GetUser(login.EmailandUserName, HashGeneratore.MD5(login.Password));
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        await HttpContext.SignInAsync(Principal(user.UserName, user.UserID, _IAccount.GetRoleUser(user.UserID)), Properties());

                        if (Url.IsLocalUrl(login.ReturnUrl))
                        {
                            if (login.ReturnUrl.Contains("Surveys/VoteQuestion"))
                            {
                                return Redirect("/Surveys");
                            }
                            if (login.ReturnUrl.Contains("Panel/CommandComment"))
                            {
                                return Redirect("/");
                            }
                            if (!await _IAccount.ExistProfile(user.UserID))
                            {
                                return RedirectToAction("Index", "Dashboard");
                            }
                            return Redirect(login.ReturnUrl);
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                    else
                    {
                        ViewBag.Error = "اکانت کاربری شما فعال نشده است";
                    }
                }
                else
                {
                    ViewBag.Error = "کاربری با اطلاعات وارد شده یافت نشد";
                }

            }
            return View(login);
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [Authorize]
        [Route("Dashboard/ChangePassword")]
        public IActionResult ChangePassword()
        {
            ViewData["Description"] = _ISetting.GetSetting().Result.ShortDescription;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dashboard/ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel change, IFormCollection form)
        {
            if (ReCapcha(form, out var view)) return view;
            if (ModelState.IsValid)
            {
                Users user = await _IAccount.GetUser(User.Identity.Name, HashGeneratore.MD5(change.OldPassword));
                if (user != null)
                {
                    user.Password = HashGeneratore.MD5(change.NewPassword);
                    await _IAccount.UpdateUser(user);

                    ViewBag.SuccessChange = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "رمز عبور فعلی وارد شده نادرست است");
                }
            }
            return View(change);
        }

        [Route("Dashboard/ChangeUserName")]
        [Authorize]
        public IActionResult ChangeUserName()
        {
            //var val = TempData["SuccessChangeUserName"] ?? "";
            //if (val.ToString() == "true")
            //{
            //    ViewBag.ChangeUserName = true;
            //}
            ViewData["Description"] = _ISetting.GetSetting().Result.ShortDescription;
            return View();
        }

        [HttpPost]
        [Authorize]
        [Route("Dashboard/ChangeUserName")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserName(ChangeUserNameViewModel change, IFormCollection form)
        {
            if (ReCapcha(form, out var view)) return view;
            if (ModelState.IsValid)
            {
                if (!await _IAccount.ExistUserUserName(change.UserName))
                {
                    Users user = await _IAccount.GetUser2(User.Identity.Name);
                    user.UserName = change.UserName;
                    await _IAccount.UpdateUser(user);

                    //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    //await HttpContext.SignInAsync(Principal(user.UserName, user.UserID, _IAccount.GetRoleUser(user.UserID)), Properties());

                    //TempData["SuccessChangeUserName"] = "true";
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError("UserName", "نام کاربری وارد شده تکراری است");
                }
            }
            return View(change);
        }

        public IActionResult ForgetPassword()
        {
            ViewData["Description"] = _ISetting.GetSetting().Result.ShortDescription;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forget, IFormCollection form)
        {
            if (ReCapcha(form, out var view)) return view;
            if (ModelState.IsValid)
            {
                var User = await _IAccount.GetUserInForget(forget.Email);
                if (User != null)
                {
                    if (User.IsActive)
                    {
                        ////Send Email
                        MessageSender Ms = new MessageSender();
                        var Body = await _IViewRender.RenderToStringAsync("ManageEmail/EmailForget", User.ActiveCode);
                        Ms.SendEmail(User.Email, "بازیابی رمز عبور", Body, HttpContext);

                        ViewBag.SuccessForget = true;
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "اکانت کاربری شما فعال نشده است");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده نادرست است");
                }
            }
            return View(forget);
        }

        [Route("Account/RecoveryPassword/{ActiveCode}")]
        public IActionResult RecoveryPassword(string ActiveCode)
        {
            ViewData["Description"] = _ISetting.GetSetting().Result.ShortDescription;
            return View();
        }

        [Route("Account/RecoveryPassword/{ActiveCode}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoveryPassword(string ActiveCode, RecoveryPasswordViewModel recovery, IFormCollection form)
        {
            if (ReCapcha(form, out var view)) return view;
            if (ModelState.IsValid)
            {
                var User = await _IAccount.GetUserInRecover(ActiveCode);
                if (User != null)
                {
                    User.Password = HashGeneratore.MD5(recovery.NewPassword);
                    User.ActiveCode = CodeGeneratore.ActiveCode();
                    await _IAccount.UpdateUser(User);

                    ViewBag.SuccessRecovery = true;
                    return View();
                }
                else
                {
                    ViewBag.ErrorRecovery = true;
                }
            }
            return View(recovery);
        }

        #region Remote

        public async Task<IActionResult> VerifyUserName(string UserName)
        {
            if (!await _IAccount.ExistUserUserName(UserName))
            {
                return Json(true);
            }
            return Json("نام کاربری وارد شده تکراری است");
        }

        #endregion
    }
}
