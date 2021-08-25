using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using MoreLinq;

namespace Dr_Hesabi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AuthorizeRole("Admin")]
    [RequireHttps]
    public class UsersController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly DataBaseContext _db;

        public UsersController(DataBaseContext context, DataBaseContext db)
        {
            _context = context;
            _db = db;
        }

        // GET: Admin/Users
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListUserViewComponent(int Take = 12, string q = "")
        {
            return ViewComponent("ListUserViewComponent", new { Take = Take, q = q });
        }

        public async Task<IActionResult> Profile(string id)
        {
            var user = await _context.Users.Include(s => s.ProfileStudents).FirstOrDefaultAsync(s => s.UserID == id);
            return PartialView(user);
        }

        public async Task<IActionResult> ConditionProfile(string id)
        {
            var Profile = await _context.ProfileStudents.Include(s => s.Users).FirstOrDefaultAsync(s => s.UserID == id);
            return PartialView(Profile);
        }

        public async Task<IActionResult> ChangeConditionProfile(string id, bool command, int Take = 12, string q = "")
        {
            if (ModelState.IsValid)
            {
                var Profile = await _context.ProfileStudents.Include(s => s.Users).FirstOrDefaultAsync(s => s.UserID == id);
                if (command)
                    Profile.IsCondition = true;
                else
                    Profile.IsCondition = false;
                _context.Update(Profile);
                await _context.SaveChangesAsync();
                if (q == null)
                    return ViewComponent("ListUserViewComponent", new { Take = Take });
                else
                    return ViewComponent("ListUserViewComponent", new { Take = Take, q = q });
            }
            return ViewComponent("ListUserViewComponent", new { Take = Take, q = q });
        }

        public async Task<IActionResult> ConditionProfileRequest(string id)
        {
            var request = await _context.ProfileRequests.FirstOrDefaultAsync(s => s.UserID == id);
            return PartialView(request);
        }

        public async Task<IActionResult> ChangeConditionProfileRequest(string id, bool command, int Take = 12,
            string q = "")
        {
            if (ModelState.IsValid)
            {
                var request = await _context.ProfileRequests.FirstOrDefaultAsync(s => s.UserID == id);
                if (command)
                    request.IsCondition = true;
                else
                    request.IsCondition = false;
                _context.Update(request);
                await _context.SaveChangesAsync();
                if (q == null)
                    return ViewComponent("ListUserViewComponent", new { Take = Take });
                else
                    return ViewComponent("ListUserViewComponent", new { Take = Take, q = q });
            }
            return ViewComponent("ListUserViewComponent", new { Take = Take, q = q });
        }
        // GET: Admin/Users/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ListRole"] = await _context.Roles.ToListAsync();
            ViewData["ListMajors"] = await _context.Majors.Select(s => new AdminViewModel.MajorsTeachersViewModel()
            {
                Title = s.Title,
                MajorID = s.MajorID
            }).ToListAsync();
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,RoleID,UserName,Email,Password,ActiveCode,IsActive,Date")] Users users, List<string> Roles, List<string> MajorsTeacher = null)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Users.Any(s => s.UserName == users.UserName))
                {
                    if (!_context.Users.Any(s => s.Email == users.Email))
                    {
                        if (Roles == null || !Roles.Any())
                        {
                            ViewBag.Error = true;
                            ViewData["ListRole"] = await _context.Roles.ToListAsync();
                            ViewData["ListMajors"] = await _context.Majors.Select(s => new AdminViewModel.MajorsTeachersViewModel()
                            {
                                Title = s.Title,
                                MajorID = s.MajorID
                            }).ToListAsync();
                            return View(users);
                        }

                        users.Password = HashGeneratore.MD5(users.Password);
                        users.Date = DateTime.Now;
                        users.ActiveCode = CodeGeneratore.ActiveCode();
                        users.UserID = CodeGeneratore.ActiveCode();

                        ///Role Select
                        List<RoleSelects> ListRoleSelect = new List<RoleSelects>();
                        foreach (var item in Roles)
                        {
                            ListRoleSelect.Add(new RoleSelects()
                            {
                                SelectID = CodeGeneratore.ActiveCode(),
                                RoleID = item,
                                UserID = users.UserID
                            });
                        }
                        users.RoleSelects = ListRoleSelect;

                        ///Major Teacher
                        List<MajorTeachers> ListMajorTeacher = new List<MajorTeachers>();
                        string roleIdTeacher = _context.Roles.FirstOrDefaultAsync(s => s.Name == "Teacher").Result.RoleID;
                        if (Roles.Any(s => s == roleIdTeacher))
                        {
                            foreach (var item in MajorsTeacher)
                            {
                                ListMajorTeacher.Add(new MajorTeachers()
                                {
                                    MajorTeacherID = CodeGeneratore.ActiveCode(),
                                    UserID = users.UserID,
                                    MajorID = item
                                });
                            }
                        }

                        users.MajorTeachers = ListMajorTeacher;
                        _context.Add(users);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "نام کاربری وارد شده تکراری است");
                }
            }
            ViewData["ListRole"] = await _context.Roles.ToListAsync();
            ViewData["ListMajors"] = await _context.Majors.Select(s => new AdminViewModel.MajorsTeachersViewModel()
            {
                Title = s.Title,
                MajorID = s.MajorID
            }).ToListAsync();
            return View(users);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["ListRole"] = await _context.Roles.ToListAsync();
            ViewData["ListMajors"] = await _context.Majors.Select(s => new AdminViewModel.MajorsTeachersViewModel()
            {
                Title = s.Title,
                MajorID = s.MajorID
            }).ToListAsync();
            ViewData["ListRoleSelect"] = await _context.RoleSelects.Include(s=>s.Roles).Where(s => s.UserID == users.UserID).ToListAsync();
            ViewData["ListMajorSelect"] = await _context.MajorTeachers.Where(s => s.UserID == users.UserID)
                .Select(s => s.MajorID).ToListAsync();
            return View(users);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserID,RoleID,UserName,Email,Password,ActiveCode,IsActive,Date")] Users users, List<string> Roles, List<string> MajorsTeacher = null)
        {
            if (id != users.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Roles == null || !Roles.Any())
                    {
                        ViewBag.Error = true;
                        ViewData["ListRole"] = await _context.Roles.ToListAsync();
                        ViewData["ListMajors"] = await _context.Majors.Select(s => new AdminViewModel.MajorsTeachersViewModel()
                        {
                            Title = s.Title,
                            MajorID = s.MajorID
                        }).ToListAsync();
                        ViewData["ListRoleSelect"] = await _context.RoleSelects.Include(s => s.Roles).Where(s => s.UserID == users.UserID).ToListAsync();
                        ViewData["ListMajorSelect"] = await _context.MajorTeachers.Where(s => s.UserID == users.UserID)
                            .Select(s => s.MajorID).ToListAsync();
                        return View(users);
                    }
                    using (_db)
                    {
                        var User = await _db.Users.FindAsync(users.UserID);
                        if (User.Password != users.Password)
                        {
                            if (User.Password != HashGeneratore.MD5(users.Password))
                            {
                                users.Password = HashGeneratore.MD5(users.Password);
                            }
                            else
                            {
                                users.Password = User.Password;
                            }
                        }
                    }
                    _context.Update(users);

                    foreach (var item in _context.RoleSelects.Where(s => s.UserID == users.UserID))
                    {
                        _context.Remove(item);
                    }

                    foreach (var item in _context.MajorTeachers.Where(s => s.UserID == users.UserID))
                    {
                        _context.Remove(item);
                    }

                    foreach (var item in Roles)
                    {
                        _context.Add(new RoleSelects()
                        {
                            SelectID = CodeGeneratore.ActiveCode(),
                            RoleID = item,
                            UserID = users.UserID
                        });
                    }
                    string roleIdTeacher = _context.Roles.FirstOrDefaultAsync(s => s.Name == "Teacher").Result.RoleID;
                    if (Roles.Any(s => s == roleIdTeacher))
                    {
                        foreach (var item in MajorsTeacher)
                        {
                            _context.Add(new MajorTeachers()
                            { 
                                MajorTeacherID = CodeGeneratore.ActiveCode(),
                                UserID = users.UserID,
                                MajorID = item
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserID))
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
            ViewData["ListRole"] = await _context.Roles.ToListAsync();
            ViewData["ListMajors"] = await _context.Majors.Select(s => new AdminViewModel.MajorsTeachersViewModel()
            {
                Title = s.Title,
                MajorID = s.MajorID
            }).ToListAsync();
            ViewData["ListRoleSelect"] = await _context.RoleSelects.Include(s => s.Roles).Where(s => s.UserID == users.UserID).ToListAsync();
            ViewData["ListMajorSelect"] = await _context.MajorTeachers.Where(s => s.UserID == users.UserID)
                .Select(s => s.MajorID).ToListAsync();
            return View(users);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                //.Include(u => u.Roles)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (users == null)
            {
                return NotFound();
            }

            ViewData["Roles"] = string.Join(" , ",
                _context.RoleSelects.Include(s => s.Roles).Where(s => s.UserID == users.UserID)
                    .Select(s => s.Roles.Title));
            ViewData["MajorsTeacher"] = string.Join(" , ",
                _context.MajorTeachers.Include(s => s.Majors).Where(s => s.UserID == users.UserID).Select(s => s.Majors.Title));
            return View(users);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var users = await _context.Users.FindAsync(id);

            //if (await _context.Connections.AnyAsync(s => s.UserID == users.UserID || s.UserID2 == users.UserID))
            //{
            //    foreach (var item in _context.Connections.Where(s =>
            //        s.UserID == users.UserID || s.UserID2 == users.UserID))
            //    {
            //        _context.Remove(item);
            //    }
            //}

            if (await _context.ProfileStaffs.AnyAsync(s => s.UserID == users.UserID))
            {
                var profile = await _context.ProfileStaffs.SingleOrDefaultAsync(s => s.UserID == users.UserID);
                _context.Remove(profile);
            }
            if (await _context.ProfileRequests.AnyAsync(s => s.UserID == users.UserID))
            {
                foreach (var item in _context.ProfileRequests.Where(s => s.UserID == users.UserID))
                {
                    _context.Remove(item);
                }
            }
            if (await _context.RoleSelects.AnyAsync(s => s.UserID == users.UserID))
            {
                foreach (var item in _context.RoleSelects.Where(s => s.UserID == users.UserID))
                {
                    _context.Remove(item);
                }
            }

            if (await _context.ProfileStudents.AnyAsync(s => s.UserID == users.UserID))
            {
                var profile = await _context.ProfileStudents.FirstOrDefaultAsync(s => s.UserID == users.UserID);
                _context.Remove(profile);
            }

            if (await _context.LoginTests.AnyAsync(s => s.UserID == users.UserID))
            {
                foreach (var item in _context.LoginTests.Where(s => s.UserID == users.UserID))
                {
                    _context.Remove(item);
                }
            }

            if (await _context.TestsUltimate.AnyAsync(s => s.UserID == users.UserID))
            {
                foreach (var item in _context.TestsUltimate.Where(s => s.UserID == users.UserID))
                {
                    _context.TestsUltimate.Remove(item);
                }
            }

            if (await _context.MajorTeachers.AnyAsync(s => s.UserID == users.UserID))
            {
                foreach (var item in _context.MajorTeachers.Where(s => s.UserID == users.UserID))
                {
                    _context.Remove(item);
                }
            }
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(string id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
