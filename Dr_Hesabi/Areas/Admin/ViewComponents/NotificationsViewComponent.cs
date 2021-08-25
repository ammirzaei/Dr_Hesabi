using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.ViewComponents
{
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public NotificationsViewComponent(DataBaseContext context)
        {
            db = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new Tuple<IEnumerable<Connections>, int>(await db.Connections.Include(s => s.Users).Where(s => s.IsSeen == false).ToListAsync(), await db.Connections.CountAsync()));
        }
    }
}
