using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Teacher.ViewComponents
{
    public class ManageSiteTeacherViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public ManageSiteTeacherViewComponent(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)
                .Value.ToString();
            ManageSiteTeacherViewModel model = new ManageSiteTeacherViewModel()
            {
                CountAttachments = await db.Attachments.Where(s => s.PanelName == "Teacher" && s.UserID == UserID).CountAsync(),
                CountTests = await db.Tests.Where(s => s.UserID == UserID).CountAsync(),
                CountGuides = db.Setting.FirstOrDefaultAsync().Result.GuideTeacher.Split(",").Count(),
                IsMajor = await db.MajorTeachers.AnyAsync(s => s.UserID == UserID)
            };
            return View(model);
        }
    }
}
