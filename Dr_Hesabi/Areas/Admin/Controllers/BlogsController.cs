using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
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
    public class BlogsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BlogsController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.OrderByDescending(s => s.DateTime).ToListAsync());
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogID == id);
            if (blogs == null)
            {
                return NotFound();
            }

            return View(blogs);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogID,NameWriter,Title,ShortDescription,Description,ImageName,Visit,IsActive,DateTime")] Blogs blogs, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    blogs.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                    await FileGeneratore.SaveFile("Blogs", blogs.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                    blogs.DateTime = DateTime.Now;
                    blogs.Visit = 0;
                    blogs.BlogID = CodeGeneratore.ActiveCode();
                    _context.Add(blogs);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("ImageName", "لطفا تصویر مقاله را انتخاب کنید");
                }
            }
            return View(blogs);
        }


        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs.FindAsync(id);
            if (blogs == null)
            {
                return NotFound();
            }
            return View(blogs);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BlogID,NameWriter,Title,ShortDescription,Description,ImageName,Visit,IsActive,DateTime")] Blogs blogs, IFormFile ImgUp)
        {
            if (id != blogs.BlogID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgUp != null)
                    {
                        FileGeneratore.DeleteFile("Blogs", blogs.ImageName, _hostingEnvironment.WebRootPath);

                        blogs.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                        await FileGeneratore.SaveFile("Blogs", blogs.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                    }
                    _context.Update(blogs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogsExists(blogs.BlogID))
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
            return View(blogs);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogs = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogID == id);
            if (blogs == null)
            {
                return NotFound();
            }

            return View(blogs);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var blogs = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blogs);
            FileGeneratore.DeleteFile("Blogs", blogs.ImageName, _hostingEnvironment.WebRootPath);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogsExists(string id)
        {
            return _context.Blogs.Any(e => e.BlogID == id);
        }
    }
}
