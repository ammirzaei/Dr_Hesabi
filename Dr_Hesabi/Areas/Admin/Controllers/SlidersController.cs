using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
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

namespace Dr_Hesabi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AuthorizeRole("Admin")]
    [RequireHttps]
    public class SlidersController : Controller
    {
        private readonly DataBaseContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public SlidersController(DataBaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Sliders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }


        // GET: Admin/Sliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SlideID,Title,Caption,ImageName,StartDate,EndDate,IsActive")] Sliders sliders, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    sliders.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                    await FileGeneratore.SaveFile("Sliders", sliders.ImageName, ImageName, _webHostEnvironment.WebRootPath);
                    sliders.EndDate = sliders.EndDate.ToDateTimeM();
                    sliders.StartDate = sliders.StartDate.ToDateTimeM();
                    sliders.SlideID = CodeGeneratore.ActiveCode();
                    _context.Add(sliders);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("ImageName", "لطفا تصویر اسلاید را انتخاب کنید");
                }
            }
            return View(sliders);
        }

        // GET: Admin/Sliders/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliders = await _context.Sliders.FindAsync(id);
            if (sliders == null)
            {
                return NotFound();
            }
            return View(sliders);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SlideID,Title,Caption,ImageName,StartDate,EndDate,IsActive")] Sliders sliders, IFormFile ImgUp)
        {
            if (id != sliders.SlideID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgUp != null)
                    {
                        FileGeneratore.DeleteFile("Sliders", sliders.ImageName, _webHostEnvironment.WebRootPath);
                        sliders.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                        await FileGeneratore.SaveFile("Sliders", sliders.ImageName, ImgUp, _webHostEnvironment.WebRootPath);
                    }
                    sliders.EndDate = sliders.EndDate.ToDateTimeM();
                    sliders.StartDate = sliders.StartDate.ToDateTimeM();
                    _context.Update(sliders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlidersExists(sliders.SlideID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sliders);
        }

        // GET: Admin/Sliders/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliders = await _context.Sliders
                .FirstOrDefaultAsync(m => m.SlideID == id);
            if (sliders == null)
            {
                return NotFound();
            }

            return View(sliders);
        }

        // POST: Admin/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var sliders = await _context.Sliders.FindAsync(id);
            FileGeneratore.DeleteFile("Sliders", sliders.ImageName, _webHostEnvironment.WebRootPath);
            _context.Sliders.Remove(sliders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlidersExists(string id)
        {
            return _context.Sliders.Any(e => e.SlideID == id);
        }
    }
}
