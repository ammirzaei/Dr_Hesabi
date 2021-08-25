using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dr_Hesabi.Controllers
{
    [RequireHttps]
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISetting _ISetting;

        public HomeController(ILogger<HomeController> logger, ISetting iSetting)
        {
            _logger = logger;
            _ISetting = iSetting;
        }

        public async Task<IActionResult> Index()
        {
            var Setting = await _ISetting.GetSetting();
            string description = "";
            if (Setting.ShortDescription != null)
            {
                description = Setting.ShortDescription;
            }
            ViewData["Description"] = description;
            return View();
        }

        [ResponseCache(Duration = 3600)]
        public IActionResult Map()
        {
            return PartialView();
        }

        public IActionResult HttpNotFound()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }

        public string GetCode()
        {
            return CodeGeneratore.ActiveCode();
        }
    }
}
