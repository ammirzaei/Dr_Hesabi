using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public NavbarViewComponent(DataBaseContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["IsBests"] = await db.Bests.AnyAsync(s => s.IsActive && s.ParentID == null && s.Bests1.Any(a => a.IsActive));
            var setting = await db.Setting.FirstOrDefaultAsync();
            if (setting != null)
            {
                ViewBag.NameSite = setting.NameSite;
                ViewBag.LogoSite = setting.ImgLogo;
            }

            return View();
        }
    }
}
