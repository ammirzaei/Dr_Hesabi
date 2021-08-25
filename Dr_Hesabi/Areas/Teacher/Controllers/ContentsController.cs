using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Dr_Hesabi.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize]
    [AuthorizeRole("Admin,Teacher")]
    [RequireHttps]
    public class ContentsController : Controller
    {
        private readonly DataBaseContext db;
        private readonly IWebHostEnvironment _IHostEnvironment;

        public ContentsController(DataBaseContext db, IWebHostEnvironment iHostEnvironment)
        {
            this.db = db;
            _IHostEnvironment = iHostEnvironment;
        }

        private async Task<bool> CheckMajor(string majorID, string userID)
        {
            return await db.MajorTeachers.AnyAsync(s => s.MajorID == majorID && s.UserID == userID);
        }

        public async Task<IActionResult> Index(string id)
        {
            var major = await db.Majors.FindAsync(id);
            if (major == null)
                return NotFound();
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (await CheckMajor(id, UserID))
            {
                ViewData["MajorTitle"] = major.Title;
                ViewData["MajorID"] = major.MajorID;
                return View(await db.Contents.Include(s => s.Contents2).Where(s => s.ParentID == null && s.MajorID == id).ToListAsync());
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Majors()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (await db.MajorTeachers.AnyAsync(s => s.UserID == UserID))
            {
                return View(await db.MajorTeachers.Include(s => s.Majors).Where(s => s.UserID == UserID).Select(s => s.Majors).ToListAsync());
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateParent(string id, string? parentID)
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (await CheckMajor(id, UserID))
            {
                return PartialView(new Contents()
                {
                    MajorID = id,
                    ParentID = parentID
                });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateParent(Contents content, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    content.ParentID = content.ParentID == "0" ? null : content.ParentID;
                    content.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                    await FileGeneratore.SaveFile("Contents", content.ImageName, ImageName,
                        _IHostEnvironment.WebRootPath);
                    content.CreateDate = DateTime.Now;
                    content.ContentID = CodeGeneratore.ActiveCode();
                    await db.AddAsync(content);
                    await db.SaveChangesAsync();

                    if (content.ParentID == null)
                        return RedirectToAction(nameof(Index), new { id = content.MajorID });
                    else
                        return RedirectToAction(nameof(Lists), new { id = content.ParentID });
                }
            }
            if (content.ParentID == null)
                return RedirectToAction(nameof(Index), new { id = content.MajorID });
            else
                return RedirectToAction(nameof(Lists), new { id = content.ParentID });
        }

        public async Task<IActionResult> EditParent(string id)
        {
            var content = await db.Contents.FindAsync(id);
            if (content == null)
                return NotFound();
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (await CheckMajor(content.MajorID, UserID))
            {
                return PartialView(content);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditParent(Contents content, IFormFile ImgUp)
        {
            if (ModelState.IsValid)
            {
                if (ImgUp != null)
                {
                    FileGeneratore.DeleteFile("Contents", content.ImageName, _IHostEnvironment.WebRootPath);

                    content.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                    await FileGeneratore.SaveFile("Contents", content.ImageName, ImgUp, _IHostEnvironment.WebRootPath);
                }

                db.Update(content);
                await db.SaveChangesAsync();

                if (content.ParentID == null)
                    return RedirectToAction(nameof(Index), new { id = content.MajorID });
                else
                    return RedirectToAction(nameof(Lists), new { id = content.ParentID });
            }
            if (content.ParentID == null)
                return RedirectToAction(nameof(Index), new { id = content.MajorID });
            else
                return RedirectToAction(nameof(Lists), new { id = content.ParentID });
        }

        public async Task<IActionResult> DeleteParent(string id)
        {
            var content = await db.Contents.FindAsync(id);
            if (content == null)
                return NotFound();
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (await CheckMajor(content.MajorID, UserID))
            {
                return PartialView(content);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteParent")]
        public async Task<IActionResult> DeleteParentConfirm(string id)
        {
            var content = await db.Contents.Include(s => s.Contents2).FirstOrDefaultAsync(s => s.ContentID == id);
            if (content == null)
                return NotFound();
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (await CheckMajor(content.MajorID, UserID))
            {
                if (await db.Contents.AnyAsync(s => s.ParentID == content.ContentID))
                {
                    List<string> list = new List<string>();
                    list.AddRange(await db.Contents.Where(s => s.ParentID == content.ContentID)
                        .Select(s => s.ContentID).ToListAsync());
                    for (int i = 1; i <= list.Count(); i++)
                    {
                        try
                        {
                            if (await db.Contents.AnyAsync(s => s.ParentID == list[i - 1]))
                            {
                                list.AddRange(await db.Contents.Where(s => s.ParentID == list[i - 1]).Select(s => s.ContentID)
                                    .ToListAsync());
                            }
                        }
                        catch
                        {

                        }
                    }

                    foreach (var item in list)
                    {
                        var contentItem = await db.Contents.FindAsync(item);
                        FileGeneratore.DeleteFile("Contents", contentItem.ImageName, _IHostEnvironment.WebRootPath);
                        db.Remove(contentItem);
                    }
                }

                FileGeneratore.DeleteFile("Contents", content.ImageName, _IHostEnvironment.WebRootPath);
                db.Remove(content);

                await db.SaveChangesAsync();
                if (content.ParentID == null)
                    return RedirectToAction(nameof(Index), new { id = content.MajorID });
                else
                    return RedirectToAction(nameof(Lists), new { id = content.ParentID });
            }
            if (content.ParentID == null)
                return RedirectToAction(nameof(Index), new { id = content.MajorID });
            else
                return RedirectToAction(nameof(Lists), new { id = content.ParentID });
        }

        public async Task<IActionResult> Lists(string id)
        {
            var content = await db.Contents.FindAsync(id);
            if (content == null)
                return NotFound();
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (!await CheckMajor(content.MajorID, UserID))
            {
                return RedirectToAction("Index", "Home");
            }

            ListsContentsViewModel model = new ListsContentsViewModel()
            {
                ContentID = content.ContentID,
                ContentTitle = content.Title,
                ContentParentID = content.ParentID,
                MajorID = content.MajorID
            };

            ViewData["ListModel"] = model;
            return View(await db.Contents.Include(s => s.Contents2).Where(s => s.ParentID == content.ContentID).ToListAsync());
        }

        public IActionResult CreateChild(string id, string parentID)
        {
            return View(new Contents()
            {
                ParentID = parentID,
                MajorID = id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChild(Contents content, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    if (content.Description != null)
                    {
                        content.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                        await FileGeneratore.SaveFile("Contents", content.ImageName, ImageName, _IHostEnvironment.WebRootPath);

                        content.CreateDate = DateTime.Now;
                        content.ContentID = CodeGeneratore.ActiveCode();
                        await db.AddAsync(content);
                        await db.SaveChangesAsync();

                        return RedirectToAction(nameof(Lists), new { id = content.ParentID });
                    }
                    else
                    {
                        ModelState.AddModelError("Description", "لطفا توضیحات عضو را وارد نمایید");
                    }
                }
                else
                {
                    ModelState.AddModelError("ImageName", "لطفا تصویر عضو را وارد نمایید");
                }
            }
            return View(content);
        }

        public async Task<IActionResult> EditChild(string id)
        {
            var content = await db.Contents.FindAsync(id);
            if (content == null)
                return NotFound();
            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChild(Contents content, IFormFile ImgUp)
        {
            if (ModelState.IsValid)
            {
                if (content.Description != null)
                {
                    if (ImgUp != null)
                    {
                        FileGeneratore.DeleteFile("Contents", content.ImageName, _IHostEnvironment.WebRootPath);

                        content.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                        await FileGeneratore.SaveFile("Contents", content.ImageName, ImgUp,
                            _IHostEnvironment.WebRootPath);
                    }

                    db.Update(content);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Lists), new { id = content.ParentID });
                }
                else
                {
                    ModelState.AddModelError("Description", "لطفا توضیحات عضو را وارد نمایید");
                }
            }
            return View(content);
        }

        public async Task<IActionResult> DetailsChild(string id)
        {
            var content = await db.Contents.FindAsync(id);
            if (content == null)
                return NotFound();
            return View(content);
        }

        public async Task<IActionResult> DeleteChild(string id)
        {
            var content = await db.Contents.FindAsync(id);
            if (content == null)
                return NotFound();
            return View(content);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteChild")]
        public async Task<IActionResult> DeleteChildConfirm(string id)
        {
            var content = await db.Contents.FindAsync(id);
            if (content == null)
                return NotFound();
            FileGeneratore.DeleteFile("Contents", content.ImageName, _IHostEnvironment.WebRootPath);
            db.Remove(content);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Lists), new { id = content.ParentID });
        }
    }
}
