using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Teacher.ViewComponents
{
    public class PanelProfileTeacherViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public PanelProfileTeacherViewComponent(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value
                .ToString();
            string avatar = "";
            if (await db.ProfileStaffs.AnyAsync(s => s.UserID == UserID))
            {
                avatar = db.ProfileStaffs.FirstOrDefaultAsync(s => s.UserID == UserID).Result.ImageName;
            }

            ViewData["Avatar"] = avatar;
            return View();
        }
    }
}
