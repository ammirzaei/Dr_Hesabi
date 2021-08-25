using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stimulsoft.System.Windows.Forms;

namespace Dr_Hesabi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AuthorizeRole("Admin")]
    [RequireHttps]
    public class AttachmentsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public AttachmentsController(DataBaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _WebHostEnvironment = webHostEnvironment;
        }
        // GET: Admin/Attachments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Attachments.Where(s => s.PanelName == "Admin").ToListAsync());
        }

        // GET: Admin/Attachments/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/Attachments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttachmentID,FileName")] Attachments attachments, IFormFile File)
        {
            if (ModelState.IsValid)
            {
                if (File != null)
                {
                    attachments.FileName = FileGeneratore.NameFile(File.FileName);
                    await FileGeneratore.SaveFile("Attachments", attachments.FileName, File, _WebHostEnvironment.WebRootPath);
                    attachments.AttachmentID = CodeGeneratore.ActiveCode();
                    attachments.PanelName = "Admin";
                    
                    _context.Add(attachments);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //public bool CopyLink(string link, bool Iscommand)
        //{
        //    try
        //    {
        //        string result = string.Empty;
        //        if (Iscommand)
        //        {
        //            result = "/Images/Attachments/" + link;
        //        }
        //        else
        //        {
        //            result = $"{_Configuration["MyDomain"]}/Images/Attachments/" + link;
        //        }
        //        CopyText.SetText(result);

        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        // GET: Admin/Attachments/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attachments = await _context.Attachments
                .FirstOrDefaultAsync(m => m.AttachmentID == id);
            if (attachments == null)
            {
                return NotFound();
            }

            return PartialView(attachments);
        }

        // POST: Admin/Attachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var attachments = await _context.Attachments.FindAsync(id);
            FileGeneratore.DeleteFile("Attachments", attachments.FileName, _WebHostEnvironment.WebRootPath);
            _context.Attachments.Remove(attachments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowFile(string id)
        {
            var attachment = await _context.Attachments.FindAsync(id);
            if (attachment == null)
                return NotFound();

            return PartialView(attachment);
        }
        private bool AttachmentsExists(string id)
        {
            return _context.Attachments.Any(e => e.AttachmentID == id);
        }
    }
}
