using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.Controllers
{
    [RequireHttps]
    public class ManageEmailController : Controller
    {
        public IActionResult EmailRegister()
        {
            return PartialView();
        }

        public IActionResult EmailForget()
        {
            return PartialView();
        }

        public IActionResult EmailSupportConnection()
        {
            return PartialView();
        }
    }
}
