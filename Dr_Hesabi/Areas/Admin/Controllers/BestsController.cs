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
    [RequireHttps]
    public class BestsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BestsController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Admin/Bests
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.Bests.Include(b => b.Bests2);
            return View(await dataBaseContext.ToListAsync());
        }


        // GET: Admin/Bests/Create
        public IActionResult Create(string? id)
        {
            return PartialView(new Bests()
            {
                ParentID = id
            });
        }

        // POST: Admin/Bests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BestID,ParentID,Title,Description,ImageName,IsActive")] Bests bests, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    bests.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                    if (bests.ParentID == null)
                    {
                        await FileGeneratore.SaveFile("Bests/Best_Lists", bests.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                    }
                    else
                    {
                        await FileGeneratore.SaveFile("Bests/Best_Items", bests.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                        bests.IsActive = true;
                    }

                    bests.BestID = CodeGeneratore.ActiveCode();
                    _context.Add(bests);
                    await _context.SaveChangesAsync();
                }
            }

            if (bests.ParentID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(IndexItem), new { id = bests.ParentID });
            }
        }

        // GET: Admin/Bests/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bests = await _context.Bests.FindAsync(id);
            if (bests == null)
            {
                return NotFound();
            }
            return PartialView(bests);
        }

        // POST: Admin/Bests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BestID,ParentID,Title,Description,ImageName,IsActive")] Bests bests, IFormFile ImgUp)
        {
            if (id != bests.BestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgUp != null)
                    {
                        if (bests.ParentID == null)
                        {
                            FileGeneratore.DeleteFile("Bests/Best_Lists", bests.ImageName, _hostingEnvironment.WebRootPath);
                            bests.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                            await FileGeneratore.SaveFile("Bests/Best_Lists", bests.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                        }
                        else
                        {
                            FileGeneratore.DeleteFile("Bests/Best_Items", bests.ImageName, _hostingEnvironment.WebRootPath);
                            bests.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                            await FileGeneratore.SaveFile("Bests/Best_Items", bests.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                        }

                    }
                    _context.Update(bests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BestsExists(bests.BestID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            if (bests.ParentID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(IndexItem), new { id = bests.ParentID });
            }
        }

        // GET: Admin/Bests/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bests = await _context.Bests
                .Include(b => b.Bests2)
                .FirstOrDefaultAsync(m => m.BestID == id);
            if (bests == null)
            {
                return NotFound();
            }

            return PartialView(bests);
        }

        // POST: Admin/Bests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var bests = await _context.Bests.FindAsync(id);
            _context.Bests.Remove(bests);

            if (bests.ParentID == null)
            {
                FileGeneratore.DeleteFile("Bests/Best_Lists", bests.ImageName, _hostingEnvironment.WebRootPath);

                if (_context.Bests.Any(s => s.ParentID == bests.BestID))
                {
                    foreach (var item in _context.Bests.Where(s => s.ParentID == bests.BestID))
                    {
                        _context.Remove(item);
                        FileGeneratore.DeleteFile("Bests/Best_Items", item.ImageName, _hostingEnvironment.WebRootPath);
                    }
                }
            }
            else
            {
                FileGeneratore.DeleteFile("Bests/Best_Items", bests.ImageName, _hostingEnvironment.WebRootPath);
            }

            await _context.SaveChangesAsync();
            if (bests.ParentID == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(IndexItem), new { id = bests.ParentID });
            }
        }

        #region Best_Items

        public async Task<IActionResult> IndexItem(string id)
        {
            var best = await _context.Bests.FindAsync(id);
            if (best == null)
            {
                return NotFound();
            }

            ViewData["BestTitle"] = best.Title;
            ViewData["BestID"] = best.BestID;
            var Items = _context.Bests.Where(s => s.ParentID == id);
            return View(await Items.ToListAsync());
        }


        #endregion

        private bool BestsExists(string id)
        {
            return _context.Bests.Any(e => e.BestID == id);
        }
    }
}
