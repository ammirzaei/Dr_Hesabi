using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AuthorizeRole("Admin")]
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly DataBaseContext db;

        public HomeController(DataBaseContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Comments(string id, string method)
        {
            ViewData["ID"] = id;
            ViewData["Method"] = method;
            return View();
        }

        public async Task<IActionResult> EditComment(int id)
        {
            var comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return PartialView(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(Comments comment)
        {
            if (ModelState.IsValid)
            {
                db.Update(comment);
                await db.SaveChangesAsync();
            }
            return ViewComponent("ListCommentsViewComponent", new { id = comment.PanelID, method = comment.Method });
        }

        public async Task<IActionResult> DeleteComment(string id)
        {
            var comment = await db.Comments.Include(s => s.Users).FirstOrDefaultAsync(s => s.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }
            return PartialView(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteComment")]
        public async Task<IActionResult> DeleteCommentConfirm(int id)
        {
            var comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return ViewComponent("ListCommentsViewComponent", new { id = comment.PanelID, method = comment.Method });
        }
    }
}
