using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Testing.ViewComponents
{
    [ViewComponent(Name = "ListRequestsViewComponent")]
    public class ListRequestsViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public ListRequestsViewComponent(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(string testID)
        {
            var test = await db.Tests.FindAsync(testID);
            return View(await db.TestRequests.Include(s => s.Users).ThenInclude(s => s.ProfileStudents).Where(s => s.TestID == testID && s.UserID != test.UserID).ToListAsync());
        }
    }
}
