using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Dr_Hesabi.Areas.Admin.ViewComponents.ListModel
{
    [ViewComponent(Name = "ListUserViewComponent")]
    public class ListUserViewComponent : ViewComponent
    {
        private readonly DataBaseContext _context;

        public ListUserViewComponent(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int Take = 12, string q = "")
        {
            ViewData["CountUser"] = _context.Users.Count() / Take;
            ViewData["Take"] = Take;
            ViewData["Profile"] = await _context.ProfileStudents.AnyAsync(s => s.IsCondition == null);
            ViewData["ProfileRequest"] = await _context.ProfileRequests.AnyAsync(s => s.IsCondition == null);
            var dataBaseContext = await _context.Users.Include(s=>s.ProfileRequests).Include(s => s.ProfileStudents).Where(s => s.Email.Contains(q) || s.UserName.Contains(q) || s.ProfileStudents.FullName.Contains(q) || s.ProfileStudents.CodeMeli.Contains(q)).ToListAsync();
            return View("ListUser", dataBaseContext.DistinctBy(s => s.UserID).Take(Take).OrderByDescending(s=>s.Date));
        }
    }
}
