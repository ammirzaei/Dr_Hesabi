using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize]
    [AuthorizeRole("Teacher,Admin")]
    [RequireHttps]

    public class HomeController : Controller
    {
        private readonly DataBaseContext db;
        private readonly IWebHostEnvironment _IWebHostEnvironment;

        public HomeController(DataBaseContext db, IWebHostEnvironment iWebHostEnvironment)
        {
            this.db = db;
            _IWebHostEnvironment = iWebHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (!await db.ProfileStaffs.AnyAsync(s => s.UserID == UserID))
                ViewData["ShowAddProfile"] = true;
            else
                ViewData["ShowAddProfile"] = false;
            return View();
        }

        public async Task<IActionResult> GetGuids()
        {
            var GuideTeacher = db.Setting.FirstOrDefaultAsync().Result.GuideTeacher;
            List<string> ListGuideTeacher = new List<string>();
            if (GuideTeacher != null)
            {
                ListGuideTeacher = GuideTeacher.Split(',').ToList();
            }
            return View(await Task.FromResult(ListGuideTeacher.ToList()));
        }
        public async Task<IActionResult> AddProfile()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            if (await db.ProfileStaffs.AnyAsync(s => s.UserID == UserID))
            {
                return RedirectToAction(nameof(EditProfile));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProfile(ProfileStaffs profileStaff, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
                profileStaff.UserID = UserID;
                profileStaff.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                await FileGeneratore.SaveFile("Staffs", profileStaff.ImageName, ImageName, _IWebHostEnvironment.WebRootPath);
                profileStaff.ProfileStaffID = CodeGeneratore.ActiveCode();
                await db.ProfileStaffs.AddAsync(profileStaff);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profileStaff);
        }

        public async Task<IActionResult> EditProfile()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            var profile = await db.ProfileStaffs.SingleOrDefaultAsync(s => s.UserID == UserID);
            if (profile != null)
                return View(profile);
            else
                return RedirectToAction(nameof(AddProfile));
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileStaffs profileStaff, IFormFile ImgUp)
        {
            if (ModelState.IsValid)
            {
                string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
                if (ImgUp != null)
                {
                    FileGeneratore.DeleteFile("Staffs", profileStaff.ImageName, _IWebHostEnvironment.WebRootPath);

                    profileStaff.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                    await FileGeneratore.SaveFile("Staffs", profileStaff.ImageName, ImgUp,
                        _IWebHostEnvironment.WebRootPath);
                }

                db.Update(profileStaff);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profileStaff);
        }
    }
}
