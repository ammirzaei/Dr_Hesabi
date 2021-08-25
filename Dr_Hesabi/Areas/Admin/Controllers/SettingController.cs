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
    public class SettingController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SettingController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Admin/Setting
        public async Task<IActionResult> Index()
        {
            var settings = await _context.Setting.FirstOrDefaultAsync();
            var SettingCount = await _context.Setting.CountAsync();
            if (SettingCount > 1)
            {
                var settingmax = _context.Setting.MaxAsync(s => s.SettingID);
                var setting = _context.Setting.FindAsync(await settingmax);
                _context.Setting.Remove(await setting);
                await _context.SaveChangesAsync();
            }
            return View(settings);
        }



        // GET: Admin/Setting/Create
        public IActionResult Create()
        {
            if (_context.Setting.Any())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Admin/Setting/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SettingID,History,ImgCodeQR,ImgLogo,ImgHistory,ShortDescription,NameSite,NameSite2,Telephone,Address,Telegram,Targets,Email,PasswordEmail,GuideTeacher,GuideStudent,EmailSupport")] Setting setting, IFormFile ImgCodeQR, IFormFile ImgLogo, IFormFile ImgHistory)
        {
            if (ModelState.IsValid)
            {
                if (ImgLogo != null)
                {
                    setting.ImgLogo = FileGeneratore.NameFile(ImgLogo.FileName);
                    await FileGeneratore.SaveFile("Setting", setting.ImgLogo, ImgLogo, _hostingEnvironment.WebRootPath);
                }
                if (ImgCodeQR != null)
                {
                    setting.ImgCodeQR = FileGeneratore.NameFile(ImgCodeQR.FileName);
                    await FileGeneratore.SaveFile("Setting", setting.ImgCodeQR, ImgCodeQR, _hostingEnvironment.WebRootPath);
                }
                if (ImgHistory != null)
                {
                    setting.ImgHistory = FileGeneratore.NameFile(ImgHistory.FileName);
                    await FileGeneratore.SaveFile("Setting", setting.ImgHistory, ImgHistory,
                        _hostingEnvironment.WebRootPath);
                }
                setting.SettingID = CodeGeneratore.ActiveCode();
                _context.Add(setting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(setting);
        }

        // GET: Admin/Setting/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setting = await _context.Setting.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        // POST: Admin/Setting/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SettingID,History,ImgCodeQR,ImgLogo,ImgHistory,ShortDescription,NameSite,NameSite2,Telephone,Address,Telegram,Targets,Email,PasswordEmail,GuideTeacher,GuideStudent,EmailSupport")] Setting setting, IFormFile ImgCodeQRUp, IFormFile ImgLogoUp, IFormFile ImgHistoryUp)
        {
            if (id != setting.SettingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgCodeQRUp != null)
                    {
                        if (setting.ImgCodeQR != null)
                        {
                            FileGeneratore.DeleteFile("Setting", setting.ImgCodeQR, _hostingEnvironment.WebRootPath);
                        }
                        setting.ImgCodeQR = FileGeneratore.NameFile(ImgCodeQRUp.FileName);
                        await FileGeneratore.SaveFile("Setting", setting.ImgCodeQR, ImgCodeQRUp, _hostingEnvironment.WebRootPath);
                    }
                    if (ImgLogoUp != null)
                    {
                        if (setting.ImgLogo != null)
                        {
                            FileGeneratore.DeleteFile("Setting", setting.ImgLogo, _hostingEnvironment.WebRootPath);
                        }
                        setting.ImgLogo = FileGeneratore.NameFile(ImgLogoUp.FileName);
                        await FileGeneratore.SaveFile("Setting", setting.ImgLogo, ImgLogoUp, _hostingEnvironment.WebRootPath);
                    }

                    if (ImgHistoryUp != null)
                    {
                        if (setting.ImgHistory != null)
                        {
                            FileGeneratore.DeleteFile("Setting",setting.ImgHistory,_hostingEnvironment.WebRootPath);
                        }
                        setting.ImgHistory = FileGeneratore.NameFile(ImgHistoryUp.FileName);
                        await FileGeneratore.SaveFile("Setting", setting.ImgHistory, ImgHistoryUp,
                            _hostingEnvironment.WebRootPath);
                    }
                    _context.Update(setting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.SettingID))
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
            return View(setting);
        }

        private bool SettingExists(string id)
        {
            return _context.Setting.Any(e => e.SettingID == id);
        }
    }
}
