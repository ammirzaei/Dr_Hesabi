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
    public class ShowProfileStaffViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public ShowProfileStaffViewComponent(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)
                .Value.ToString();
            var profile = await db.ProfileStaffs.SingleOrDefaultAsync(s => s.UserID == UserID);
            string staff = profile.StaffID == null
                ? "در حال بررسی"
                : db.Staffs.FirstOrDefaultAsync(s => s.StaffID == profile.StaffID && s.ParentID == null).Result.Title;
            ViewData["Staff"] = staff;

            return View(await Task.FromResult(profile));
        }
    }
}
