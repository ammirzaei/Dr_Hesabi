using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.ViewModel;
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
    public class StaffsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public StaffsController(DataBaseContext context, IWebHostEnvironment evEnvironment)
        {
            _context = context;
            _hostingEnvironment = evEnvironment;
        }

        // GET: Admin/Staffs
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListStaffsViewComponent()
        {
            return ViewComponent("ListStaffsViewComponent");
        }

        // GET: Admin/Staffs/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var staffs = await _context.Staffs
                .Include(s => s.Staffs2)
                .FirstOrDefaultAsync(m => m.StaffID == id);
            if (staffs == null)
            {
                return NotFound();
            }
            return View(staffs);
        }

        // GET: Admin/Staffs/Create
        public IActionResult Create(string? id)
        {
            return PartialView(new Staffs()
            {
                ParentID = id
            });
        }

        // POST: Admin/Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffID,ParentID,Title,Text,ImageName")] Staffs staffs)
        {
            if (ModelState.IsValid)
            {
                staffs.StaffID = CodeGeneratore.ActiveCode();
                _context.Add(staffs);
                await _context.SaveChangesAsync();
                return ViewComponent("ListStaffsViewComponent");
            }
            return ViewComponent("ListStaffsViewComponent");
        }

        // GET: Admin/Staffs/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffs = await _context.Staffs.FindAsync(id);
            if (staffs == null)
            {
                return NotFound();
            }
            return PartialView(staffs);
        }

        // POST: Admin/Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StaffID,ParentID,Title,Text,ImageName")] Staffs staffs)
        {
            if (id != staffs.StaffID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staffs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffsExists(staffs.StaffID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return ViewComponent("ListStaffsViewComponent");
            }

            return ViewComponent("ListStaffsViewComponent");
        }

        // GET: Admin/Staffs/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staffs = await _context.Staffs
                .Include(s => s.Staffs2)
                .FirstOrDefaultAsync(m => m.StaffID == id);
            if (staffs == null)
            {
                return NotFound();
            }

            return PartialView(staffs);
        }

        // POST: Admin/Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var staffs = await _context.Staffs.FindAsync(id);
            _context.Staffs.Remove(staffs);


            if (await _context.Staffs.AnyAsync(s => s.ParentID == staffs.StaffID))
            {
                foreach (var item in _context.Staffs.Where(s => s.ParentID == staffs.StaffID))
                {
                    _context.Staffs.Remove(item);
                    FileGeneratore.DeleteFile("Staffs", item.ImageName, _hostingEnvironment.WebRootPath);
                }
            }

            if (staffs.ImageName != null)
            {
                FileGeneratore.DeleteFile("Staffs", staffs.ImageName, _hostingEnvironment.WebRootPath);
            }

            await _context.SaveChangesAsync();
            return ViewComponent("ListStaffsViewComponent");
        }

        public IActionResult CreateParent(string id)
        {
            return View(new Staffs()
            {
                ParentID = id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateParent(Staffs staffs, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (staffs.Text != null)
                {
                    if (ImageName != null)
                    {
                        if (!await _context.Staffs.AnyAsync(s => s.Title == staffs.Title) &&
                           !await _context.ProfileStaffs.AnyAsync(s => s.StaffID != null && s.Title == staffs.Title))
                        {
                            staffs.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                            await FileGeneratore.SaveFile("Staffs", staffs.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                            staffs.StaffID = CodeGeneratore.ActiveCode();
                            await _context.Staffs.AddAsync(staffs);
                            await _context.SaveChangesAsync();

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("Title", "نام و نام خانوادگی وارد شده تکراری است.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ImageName", "لطفا تصویر کادر را انتخاب کنید");
                    }
                }
                else
                {
                    ModelState.AddModelError("Text", "لطفا متن توضیحات را وارد نمایید");
                }
            }

            return View(staffs);
        }

        public async Task<IActionResult> EditParent(string id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditParent(Staffs staffs, IFormFile ImgUp)
        {
            if (ModelState.IsValid)
            {
                if (staffs.Text != null)
                {
                    if (!await _context.Staffs.AnyAsync(s => s.Title == staffs.Title) &&
                        !await _context.ProfileStaffs.AnyAsync(s => s.StaffID != null && s.Title == staffs.Title))
                    {
                        if (ImgUp != null)
                        {
                            FileGeneratore.DeleteFile("Staffs", staffs.ImageName, _hostingEnvironment.WebRootPath);

                            staffs.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                            await FileGeneratore.SaveFile("Staffs", staffs.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                        }

                        _context.Update(staffs);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Title", "نام و نام خانوادگی وارد شده تکراری است.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Text", "لطفا متن توضیحات را وارد نمایید");
                }
            }
            return View(staffs);
        }

        public async Task<IActionResult> DetailProfileStaff(string id)
        {
            var profile = await _context.ProfileStaffs.SingleOrDefaultAsync(s => s.ProfileStaffID == id);
            if (profile == null)
                return NotFound();
            return View(profile);
        }

        public async Task<IActionResult> SetStaffToParent(string id, bool isNative)
        {
            string fullName = isNative
                ? _context.Staffs.FirstOrDefaultAsync(s => s.ParentID != null && s.StaffID == id).Result.Title
                : _context.ProfileStaffs.SingleOrDefaultAsync(s => s.ProfileStaffID == id).Result.Title;

            ViewData["fullName"] = fullName;
            ViewData["IsNative"] = isNative.ToString();
            return PartialView(await _context.Staffs.Where(s => s.ParentID == null).Select(s => new AdminViewModel.SetStaffToParentViewModel()
            {
                Title = s.Title,
                StaffID = s.StaffID
            }).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SetStaffToParent(string id, string? staff, string native)
        {
            if (ModelState.IsValid)
            {
                if (native.Contains("False"))
                {
                    var profile = await _context.ProfileStaffs.FirstOrDefaultAsync(s => s.ProfileStaffID == id);
                    if (profile.StaffID != staff)
                    {
                        profile.StaffID = staff;
                        _context.Update(profile);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    var Staff = await _context.Staffs.FirstOrDefaultAsync(s => s.StaffID == id && s.ParentID != null);
                    if (Staff.ParentID != staff)
                    {
                        Staff.ParentID = staff;
                        _context.Update(Staff);
                        await _context.SaveChangesAsync();
                    }
                }
                return ViewComponent("ListStaffsViewComponent");
            }
            return ViewComponent("ListStaffsViewComponent");
        }

        public async Task<IActionResult> SetUserToStaff(string id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
                return NotFound();

            ViewData["fullName"] = staff.Title;
            string roleIDTeacher = _context.Roles.FirstOrDefaultAsync(s => s.Name == "Teacher").Result.RoleID;
            return PartialView(await _context.Users.Where(s => s.ProfileStaffs == null && s.RoleSelects.Any(r => r.RoleID == roleIDTeacher)).Select(s => new AdminViewModel.SetUserToStaffViewModel()
            {
                UserID = s.UserID,
                UserName = s.UserName
            }).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetUserToStaff(string id, string UserID)
        {
            var staff = await _context.Staffs.FindAsync(id);
            var user = await _context.Users.FindAsync(UserID);
            if (staff == null || user == null)
                return NotFound();

            await _context.AddAsync(new ProfileStaffs()
            {
                UserID = UserID,
                ImageName = staff.ImageName,
                Title = staff.Title,
                StaffID = staff.ParentID,
                Description = staff.Text,
                ProfileStaffID = CodeGeneratore.ActiveCode()
            });
            _context.Remove(staff);
            await _context.SaveChangesAsync();
            return ViewComponent("ListStaffsViewComponent");
        }

        private bool StaffsExists(string id)
        {
            return _context.Staffs.Any(e => e.StaffID == id);
        }
    }
}
