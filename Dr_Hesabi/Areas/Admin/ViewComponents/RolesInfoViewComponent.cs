using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.ViewComponents
{
    public class RolesInfoViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public RolesInfoViewComponent(DataBaseContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<string> Labels = await db.Roles.Select(s => s.Title).ToListAsync();
            ViewData["Labels"] = Labels;
            List<double> Values = new List<double>();
            foreach (var item in db.Roles)
            {
                Values.Add(await db.RoleSelects.Where(s => s.RoleID == item.RoleID).CountAsync());
            }

            ViewData["Values"] = Values;
            return View();
        }
    }
}
