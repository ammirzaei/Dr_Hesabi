using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.ViewComponents
{
    public class UsersInfoViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public UsersInfoViewComponent(DataBaseContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["Labels"] = new List<string>()
            {
                {"کاربران فعال"},
                {"کاربران غیرفعال"},
                {"همه کاربران"}
            };
            ViewData["Values"] = new List<double>()
            {
                {await db.Users.Where(s=>s.IsActive).CountAsync()},
                {await db.Users.Where(s=>s.IsActive==false).CountAsync()},
                {await db.Users.CountAsync()}
            };
            return View();
        }
    }
}
