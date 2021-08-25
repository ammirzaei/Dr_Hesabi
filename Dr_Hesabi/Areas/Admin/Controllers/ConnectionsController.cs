using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AuthorizeRole("Admin")]
    [RequireHttps]
    public class ConnectionsController : Controller
    {
        private readonly DataBaseContext db;

        public ConnectionsController(DataBaseContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Connections.Include(s => s.Users).OrderByDescending(s => s.CreateDate).ToListAsync());
        }

        public async Task<IActionResult> ShowComment(string id,bool? isDelete)
        {
            Connections comment = await db.Connections.Include(s => s.Users).FirstOrDefaultAsync(s => s.ConnectionID == id);
            if (comment == null)
            {
                return NotFound();
            }
            if (comment.IsSeen == false)
            {
                comment.IsSeen = true;
                db.Update(comment);
                await db.SaveChangesAsync();
            }

            ViewBag.IsDelete = isDelete;
            return View(comment);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Connections comment = await db.Connections.FirstOrDefaultAsync(s => s.ConnectionID == id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Remove(comment);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
