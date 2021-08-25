using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.ViewComponents
{
    public class SlidersViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public SlidersViewComponent(DataBaseContext context)
        {
            db = context;
        }

        [ResponseCache(Duration = 3600)]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await db.Sliders.Where(s => s.IsActive && s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now).OrderByDescending(s => s.StartDate).ToListAsync());
        }

    }
}
