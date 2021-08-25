using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.ViewComponents.ListModel
{
    [ViewComponent(Name = "ListCommentsViewComponent")]
    public class ListCommentsViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public ListCommentsViewComponent(DataBaseContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id, string method)
        {
            return View(await db.Comments.Include(s => s.Users).Where(s => s.PanelID == id && s.Method == method).ToListAsync());
        }
    }
}
