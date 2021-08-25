using System;
using System.Collections.Generic;
using System.Linq;
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
    [AutoValidateAntiforgeryToken]
    [RequireHttps]
    public class MajorsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public MajorsController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Admin/Majors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Majors.ToListAsync());
        }

        // GET: Admin/Majors/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var majors = await _context.Majors
                .FirstOrDefaultAsync(m => m.MajorID == id);
            if (majors == null)
            {
                return NotFound();
            }

            return View(majors);
        }

        // GET: Admin/Majors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Majors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MajorID,Title,Text,ImageName,Count,ShortDescription")] Majors majors, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    if (await _context.Majors.AnyAsync(s => s.Title == majors.Title))
                    {
                        ModelState.AddModelError("Title", "نام رشته وارد شده تکراری می باشد");
                        return View(majors);
                    }
                    majors.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                    await FileGeneratore.SaveFile("Majors", majors.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                    majors.MajorID = CodeGeneratore.ActiveCode();
                    _context.Add(majors);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("ImageName", "لطفا تصویر رشته را انتخاب کنید");
                }
            }
            return View(majors);
        }

        // GET: Admin/Majors/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var majors = await _context.Majors.FindAsync(id);
            if (majors == null)
            {
                return NotFound();
            }
            return View(majors);
        }

        // POST: Admin/Majors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MajorID,Title,Text,ImageName,Count,ShortDescription")] Majors majors, IFormFile ImgUp)
        {
            if (id != majors.MajorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.Majors.AnyAsync(s => s.Title == majors.Title && s.MajorID != majors.MajorID))
                    {
                        ModelState.AddModelError("Title", "نام رشته وارد شده تکراری می باشد");
                        return View(majors);
                    }
                    if (ImgUp != null)
                    {
                        FileGeneratore.DeleteFile("Majors", majors.ImageName, _hostingEnvironment.WebRootPath);

                        majors.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                        await FileGeneratore.SaveFile("Majors", majors.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                    }
                    _context.Update(majors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MajorsExists(majors.MajorID))
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
            return View(majors);
        }

        // GET: Admin/Majors/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var majors = await _context.Majors
                .FirstOrDefaultAsync(m => m.MajorID == id);
            if (majors == null)
            {
                return NotFound();
            }

            return View(majors);
        }

        // POST: Admin/Majors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var majors = await _context.Majors.FindAsync(id);
            _context.Majors.Remove(majors);
            FileGeneratore.DeleteFile("Majors", majors.ImageName, _hostingEnvironment.WebRootPath);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MajorsExists(string id)
        {
            return _context.Majors.Any(e => e.MajorID == id);
        }
    }
}
