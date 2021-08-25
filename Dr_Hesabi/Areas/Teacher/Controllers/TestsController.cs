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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Dr_Hesabi.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize]
    [AuthorizeRole("Teacher,Admin")]
    [RequireHttps]
    public class TestsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _IConfiguration;
        public TestsController(DataBaseContext context, IWebHostEnvironment environment, IConfiguration iConfiguration)
        {
            _context = context;
            _hostEnvironment = environment;
            _IConfiguration = iConfiguration;
            try
            {
                StiLicense.LoadFromString("6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHl2AD0gPVknKsaW0un+3PuM6TTcPMUAWEURKXNso0e5OJN40hxJjK5JbrxU+NrJ3E0OUAve6MDSIxK3504G4vSTqZezogz9ehm+xS8zUyh3tFhCWSvIoPFEEuqZTyO744uk+ezyGDj7C5jJQQjndNuSYeM+UdsAZVREEuyNFHLm7gD9OuR2dWjf8ldIO6Goh3h52+uMZxbUNal/0uomgpx5NklQZwVfjTBOg0xKBLJqZTDKbdtUrnFeTZLQXPhrQA5D+hCvqsj+DE0n6uAvCB2kNOvqlDealr9mE3y978bJuoq1l4UNE3EzDk+UqlPo8KwL1XM+o1oxqZAZWsRmNv4Rr2EXqg/RNUQId47/4JO0ymIF5V4UMeQcPXs9DicCBJO2qz1Y+MIpmMDbSETtJWksDF5ns6+B0R7BsNPX+rw8nvVtKI1OTJ2GmcYBeRkIyCB7f8VefTSOkq5ZeZkI8loPcLsR4fC4TXjJu2loGgy4avJVXk32bt4FFp9ikWocI9OQ7CakMKyAF6Zx7dJF1nZw");
            }
            catch
            { }
        }

        // GET: Testing/Tests
        public async Task<IActionResult> Index()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            bool IsAdmin = false;
            var Models = new List<Tests>();
            if (await _context.RoleSelects.AnyAsync(s => s.UserID == UserID && s.Roles.Name.Contains("Admin")))
            {
                Models = await _context.Tests.Include(t => t.Users).Include(s => s.TestsUltimate)
                   .Include(s => s.Questions).Include(s => s.TestRequests).Include(s => s.TestClasses).ToListAsync();
                IsAdmin = true;
            }
            else
            {
                Models = await _context.Tests.Include(t => t.Users).Include(s => s.TestsUltimate)
                   .Include(s => s.Questions).Include(s => s.TestRequests).Include(s => s.TestClasses).Where(s => s.UserID == UserID).ToListAsync();
            }

            ViewData["IsAdmin"] = IsAdmin;
            ViewData["Domain"] = _IConfiguration["MyDomain"];
            return View(await Task.FromResult(Models.OrderByDescending(s => s.StartDateTime)));
        }

        public IActionResult Event()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        public async Task<IActionResult> PageReportTestRequests(string id)
        {
            if (!await _context.Tests.AnyAsync(s => s.TestID == id))
                return NotFound();
            return View();
        }

        public async Task<IActionResult> PrintReportTestRequests(string id)
        {
            StiReport report = new StiReport();
            report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/ReportTestRequest.mrt"));

            var test = await _context.Tests.FindAsync(id);
            List<PrintReportTestRequestViewModel> model = new List<PrintReportTestRequestViewModel>();
            if (await _context.TestRequests.AnyAsync(s => s.TestID == id && s.IsActive == true && s.UserID != test.UserID))
            {
                int num = 0;
                foreach (var item in _context.TestRequests.Include(s => s.Users).ThenInclude(s => s.ProfileStudents).Where(s => s.TestID == id && s.IsActive == true))
                {
                    num++;
                    model.Add(new PrintReportTestRequestViewModel()
                    {
                        CodeClass = item.Users.ProfileStudents.CodeClass.ToString(),
                        FullName = item.Users.ProfileStudents.FullName,
                        Code = num
                    });
                }
            }
            string TitleTable = "درخواست های تأیید شده " + test.Title;
            report.RegData("DB", model.ToList());
            report["Title"] = TitleTable;
            return StiNetCoreViewer.GetReportResult(this, report);
        }
        public async Task<IActionResult> PageReportTest(string id)
        {
            if (!await _context.Tests.AnyAsync(s => s.TestID == id))
                return NotFound();
            return View();
        }

        public async Task<IActionResult> PrintReportTest(string id)
        {
            StiReport report = new StiReport();
            report.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/ReportTest.mrt"));

            var test = await _context.Tests.FindAsync(id);
            List<PrintReportTestViewModel> List = new List<PrintReportTestViewModel>();
            if (await _context.TestsUltimate.AnyAsync(s => s.TestID == id))
            {
                int num = 0;
                foreach (var item in _context.TestsUltimate.Include(s => s.Users).ThenInclude(s => s.ProfileStudents).Where(s => s.TestID == id && s.UserID != test.UserID).OrderBy(s => s.Users.ProfileStudents.CodeClass))
                {
                    num += 1;
                    var Login = await _context.LoginTests
                        .FirstOrDefaultAsync(s => s.UserID == item.UserID && s.TestID == id);
                    List.Add(new PrintReportTestViewModel()
                    {
                        Code = num,
                        CountTrue = item.CountTrue,
                        CountFalse = item.CountFalse,
                        Score = item.Score,
                        StartDate = Login.DateTime.ToDateTime(),
                        EndDate = item.DateTime.ToDateTime(),
                        FullName = item.Users.ProfileStudents.FullName,
                        CodeClass = item.Users.ProfileStudents.CodeClass.ToString()
                    });
                }
            }

            string TitleTable = "شرکت کنندگان " + test.Title;
            report["Title"] = TitleTable;
            report.RegData("DB", List.ToList());
            return StiNetCoreViewer.GetReportResult(this, report);
        }
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tests = await _context.Tests
                .Include(t => t.Users)
                .FirstOrDefaultAsync(m => m.TestID == id);
            if (tests == null)
            {
                return NotFound();
            }
            ViewData["Classes"] = await _context.TestClasses.Where(s => s.TestID == tests.TestID).Select(s => s.Class.ToString()).ToListAsync();
            return View(tests);
        }

        // GET: Testing/Tests/Create
        public IActionResult Create()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            return View(new Tests()
            {
                UserID = UserID
            });
        }

        // POST: Testing/Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestID,UserID,Title,Description,StartDateTime,EndDateTime,IsActive,IsNegative,IsDeleted,IsRandom,IsUltimate")] Tests tests, DateTime StartTime, DateTime EndTime, List<int> Classes)
        {
            if (ModelState.IsValid)
            {
                if (!Classes.Any())
                {
                    ViewBag.ErrorClasses = true;
                    return View(tests);
                }
                tests.StartDateTime = new DateTime(tests.StartDateTime.Year, tests.StartDateTime.Month, tests.StartDateTime.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                tests.EndDateTime = new DateTime(tests.StartDateTime.Year, tests.StartDateTime.Month, tests.StartDateTime.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);

                tests.EndDateTime = tests.EndDateTime.ToDateTimeM2();
                tests.StartDateTime = tests.StartDateTime.ToDateTimeM2();
                tests.IsDeleted = false;
                tests.TestID = CodeGeneratore.ActiveCode();
                _context.Add(tests);
                await _context.SaveChangesAsync();
                foreach (var item in Classes)
                {
                    _context.Add(new TestClasses()
                    {
                        TestClassID = CodeGeneratore.ActiveCode(),
                        Class = item,
                        TestID = tests.TestID
                    });
                }

                await _context.TestRequests.AddAsync(new TestRequests()
                {
                    TestID = tests.TestID,
                    UserID = tests.UserID,
                    TestRequestID = CodeGeneratore.ActiveCode(),
                    IsActive = true
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tests);
        }

        // GET: Testing/Tests/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tests = await _context.Tests.FindAsync(id);
            if (tests == null)
            {
                return NotFound();
            }
            ViewData["Classes"] = await _context.TestClasses.Where(s => s.TestID == tests.TestID).Select(s => s.Class).ToListAsync();
            return View(tests);
        }

        // POST: Testing/Tests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TestID,UserID,Title,Description,StartDateTime,EndDateTime,IsActive,IsNegative,IsDeleted,IsRandom,IsUltimate")] Tests tests, DateTime StartTime, DateTime EndTime, List<int> Classes)
        {
            if (id != tests.TestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!Classes.Any())
                    {
                        ViewBag.ErrorClasses = true;
                        ViewData["Classes"] = await _context.TestClasses.Where(s => s.TestID == tests.TestID).Select(s => s.Class).ToListAsync();
                        return View(tests);
                    }
                    tests.StartDateTime = new DateTime(tests.StartDateTime.Year, tests.StartDateTime.Month, tests.StartDateTime.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
                    tests.EndDateTime = new DateTime(tests.StartDateTime.Year, tests.StartDateTime.Month, tests.StartDateTime.Day, EndTime.Hour, EndTime.Minute, EndTime.Second);

                    tests.IsDeleted = false;
                    tests.EndDateTime = tests.EndDateTime.ToDateTimeM2();
                    tests.StartDateTime = tests.StartDateTime.ToDateTimeM2();
                    _context.Update(tests);
                    foreach (var item in _context.TestClasses.Where(s => s.TestID == tests.TestID))
                    {
                        _context.Remove(item);
                    }
                    foreach (var item in Classes)
                    {
                        _context.Add(new TestClasses()
                        {
                            TestClassID = CodeGeneratore.ActiveCode(),
                            Class = item,
                            TestID = tests.TestID
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestsExists(tests.TestID))
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
            ViewData["Classes"] = await _context.TestClasses.Where(s => s.TestID == tests.TestID).Select(s => s.Class).ToListAsync();
            return View(tests);
        }

        // GET: Testing/Tests/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tests = await _context.Tests
                .Include(t => t.Users)
                .FirstOrDefaultAsync(m => m.TestID == id);
            if (tests == null)
            {
                return NotFound();
            }

            return View(tests);
        }

        // POST: Testing/Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tests = await _context.Tests.FindAsync(id);

            if (tests.IsDeleted)
            {
                if (await _context.TestsUltimate.AnyAsync(s => s.TestID == tests.TestID))
                {
                    foreach (var item in _context.TestsUltimate.Where(s => s.TestID == tests.TestID))
                    {
                        _context.Remove(item);
                    }
                }
                if (await _context.LoginTests.AnyAsync(s => s.TestID == tests.TestID))
                {
                    foreach (var item in _context.LoginTests.Where(s => s.TestID == tests.TestID))
                    {
                        _context.Remove(item);
                    }
                }
                _context.Remove(tests);
            }
            else
            {
                if (await _context.Choices.AnyAsync(s => s.Questions.TestID == tests.TestID))
                {
                    foreach (var item in _context.Choices.Where(s => s.Questions.TestID == tests.TestID))
                    {
                        _context.Remove(item);
                    }
                }
                if (await _context.QuestionReplys.AnyAsync(s => s.Questions.TestID == tests.TestID))
                {
                    foreach (var item in _context.QuestionReplys.Include(s => s.Questions).Where(s => s.Questions.TestID == tests.TestID))
                    {
                        if (item.Questions.Method.Contains("تشریحی"))
                        {
                            var reply = await _context.ReplyDescriptives.FirstOrDefaultAsync(s =>
                                s.ReplyID == item.ReplyID);
                            _context.Remove(reply);
                            if (reply.ImageName != null)
                            {
                                FileGeneratore.DeleteFile("Replys/Thumb", reply.ImageName, _hostEnvironment.WebRootPath);
                            }
                        }
                        else
                        {
                            var reply = await _context.ReplyOptionals.FirstOrDefaultAsync(s =>
                                s.ReplyID == item.ReplyID);
                            _context.Remove(reply);
                        }
                        _context.Remove(item);
                    }
                }
                if (await _context.Questions.AnyAsync(s => s.TestID == tests.TestID))
                {
                    foreach (var item in _context.Questions.Where(s => s.TestID == tests.TestID))
                    {
                        _context.Remove(item);
                        if (item.ImageName != null)
                        {
                            FileGeneratore.DeleteFile("Questions", item.ImageName, _hostEnvironment.WebRootPath);
                        }
                    }
                }

                foreach (var item in _context.TestClasses.Where(s => s.TestID == tests.TestID))
                {
                    _context.Remove(item);
                }

                if (await _context.TestRequests.AnyAsync(s => s.TestID == tests.TestID))
                {
                    foreach (var item in _context.TestRequests.Where(s => s.TestID == tests.TestID))
                    {
                        _context.Remove(item);
                    }
                }
                if (await _context.TestsUltimate.AnyAsync(s => s.TestID == tests.TestID))
                {
                    tests.IsDeleted = true;
                    _context.Update(tests);
                }
                else
                {
                    _context.Remove(tests);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ListParticipant(string id)
        {
            var Test = await _context.Tests.FindAsync(id);
            var List = _context.TestsUltimate.Include(s => s.Users).Include(s => s.Users.ProfileStudents).Where(s => s.TestID == id && s.UserID != Test.UserID).OrderBy(s => s.Users.ProfileStudents.CodeClass).Select(s => new GetAllParticipantTest()
            {
                UserID = s.UserID,
                FullName = s.Users.ProfileStudents.FullName,
                Score = s.Score,
                CodeClass = s.Users.ProfileStudents.CodeClass.ToString()
            });

            ViewData["TestTitle"] = Test.Title;
            ViewData["TestID"] = Test.TestID;
            return View(await List.ToListAsync());
        }

        public IActionResult Users(string id)
        {
            return ViewComponent("User", new { id = id });
        }

        public async Task<IActionResult> Requests(string id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
                return NotFound();
            ViewData["TestID"] = id;
            ViewData["TestTitle"] = test.Title;
            ViewData["AddressRequest"] = _IConfiguration["MyDomain"] + "/Tests/Request/" + id;
            return View();
        }

        public async Task<IActionResult> ChangeRequest(string id)
        {
            var request = await _context.TestRequests.Include(s => s.Users).ThenInclude(s => s.ProfileStudents).SingleOrDefaultAsync(s => s.TestRequestID == id);
            if (request == null)
                return NotFound();
            return PartialView(request);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRequest(string id, bool? IsActive)
        {
            if (ModelState.IsValid)
            {
                var resuest = await _context.TestRequests.FindAsync(id);
                resuest.IsActive = IsActive;
                _context.Update(resuest);
                await _context.SaveChangesAsync();
                return ViewComponent("ListRequestsViewComponent", new { testID = resuest.TestID });
            }
            return null;
        }
        private bool TestsExists(string id)
        {
            return _context.Tests.Any(e => e.TestID == id);
        }
    }
}
