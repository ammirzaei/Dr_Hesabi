using System;
using System.Collections.Generic;
using System.IO;
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
    public class GallerysController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public GallerysController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Admin/Gallerys
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.Gallerys.Include(g => g.Gallery2).Include(s=>s.Gallery1);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Admin/Gallerys/Create
        public IActionResult Create(string? id)
        {
            return PartialView(new Gallerys()
            {
                ParentID = id
            });
        }

        // POST: Admin/Gallerys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GalleryID,ParentID,Title,ImageName,DateTime")] Gallerys gallerys, IFormFile ImageName, string AddressRoot)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    gallerys.DateTime = DateTime.Now;
                    gallerys.ImageName = FileGeneratore.NameFile(ImageName.FileName);

                    if (gallerys.ParentID == null)
                    {
                        await FileGeneratore.SaveFile("Gallerys/Gallery_Lists", gallerys.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                    }
                    else
                    {
                        await FileGeneratore.SaveFile("Gallerys/Gallery_Items", gallerys.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                    }

                    gallerys.GalleryID = CodeGeneratore.ActiveCode();
                    _context.Add(gallerys);
                    await _context.SaveChangesAsync();
                }
            }

            if (gallerys.ParentID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(IndexItem), new { id = gallerys.ParentID });
            }
        }

        // GET: Admin/Gallerys/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallerys = await _context.Gallerys.FindAsync(id);
            if (gallerys == null)
            {
                return NotFound();
            }
            return PartialView(gallerys);
        }

        // POST: Admin/Gallerys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GalleryID,ParentID,Title,ImageName,DateTime")] Gallerys gallerys, IFormFile ImgUp)
        {
            if (id != gallerys.GalleryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgUp != null)
                    {
                        if (gallerys.ParentID == null)
                        {
                            FileGeneratore.DeleteFile("Gallerys/Gallery_Lists", gallerys.ImageName, _hostingEnvironment.WebRootPath);
                            gallerys.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                            await FileGeneratore.SaveFile("Gallerys/Gallery_Lists", gallerys.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                        }
                        else
                        {
                            FileGeneratore.DeleteFile("Gallerys/Gallery_Items", gallerys.ImageName, _hostingEnvironment.WebRootPath);
                            gallerys.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                            await FileGeneratore.SaveFile("Gallerys/Gallery_Items", gallerys.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                        }

                    }
                    _context.Update(gallerys);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GallerysExists(gallerys.GalleryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            if (gallerys.ParentID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(IndexItem), new { id = gallerys.ParentID });
            }
        }

        // GET: Admin/Gallerys/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gallerys = await _context.Gallerys
                .Include(g => g.Gallery2)
                .FirstOrDefaultAsync(m => m.GalleryID == id);
            if (gallerys == null)
            {
                return NotFound();
            }

            return PartialView(gallerys);
        }

        // POST: Admin/Gallerys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gallerys = await _context.Gallerys.FindAsync(id);
            _context.Gallerys.Remove(gallerys);

            if (gallerys.ParentID == null)
            {
                FileGeneratore.DeleteFile("Gallerys/Gallery_Lists", gallerys.ImageName, _hostingEnvironment.WebRootPath);
                if (_context.Gallerys.Any(s => s.ParentID == gallerys.GalleryID))
                {
                    foreach (var item in _context.Gallerys.Where(s => s.ParentID == gallerys.GalleryID))
                    {
                        _context.Gallerys.Remove(item);
                        FileGeneratore.DeleteFile("Gallerys/Gallery_Items", item.ImageName, _hostingEnvironment.WebRootPath);
                    }
                }
            }
            else
            {
                FileGeneratore.DeleteFile("Gallerys/Gallery_Items", gallerys.ImageName, _hostingEnvironment.WebRootPath);
            }

            await _context.SaveChangesAsync();

            if (gallerys.ParentID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(IndexItem), new { id = gallerys.ParentID });
            }
        }


        public async Task<IActionResult> IndexItem(string id)
        {
            var gallery = await _context.Gallerys.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }

            ViewBag.Gallery = gallery;
            var model = _context.Gallerys.Where(s => s.ParentID == id);
            return View(await model.ToListAsync());
        }
        private bool GallerysExists(string id)
        {
            return _context.Gallerys.Any(e => e.GalleryID == id);
        }
    }
}
