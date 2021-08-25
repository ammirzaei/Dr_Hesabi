using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Testing.ViewComponents
{
    [ViewComponent(Name = "User")]
    public class UserViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public UserViewComponent(DataBaseContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var User = await db.Users.Include(s => s.ProfileStudents).FirstOrDefaultAsync(s => s.UserID == id);
            return View(User);
        }
    }
}
