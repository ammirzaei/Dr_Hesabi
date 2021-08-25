using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.ViewComponents
{
    public class NewsViewComponent:ViewComponent
    {
        private readonly DataBaseContext db;

        public NewsViewComponent(DataBaseContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await db.Newses.Where(s => s.IsActive).OrderByDescending(s=>s.DateTime).Take(12).ToListAsync());
        }
    }
}
