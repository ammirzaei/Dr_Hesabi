using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dr_Hesabi.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize]
    [AuthorizeRole("Teacher,Admin")]
    [RequireHttps]
    public class QuestionsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly ITests _Tests;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public QuestionsController(DataBaseContext context, ITests tests, IWebHostEnvironment environment)
        {
            _context = context;
            _Tests = tests;
            _hostingEnvironment = environment;
        }

        // GET: Testing/Questions
        public async Task<IActionResult> Index(string id)
        {
            var Test = await _context.Tests.FindAsync(id);
            if (Test == null)
            {
                return NotFound();
            }

            ViewData["TestTitle"] = Test.Title;
            ViewData["TestID"] = Test.TestID;
            return View();
        }


        // GET: Testing/Questions/Create
        public IActionResult Create(string id)
        {
            return PartialView(new Questions()
            {
                TestID = id
            });
        }

        // POST: Testing/Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionID,ImageName,TestID,Method,Title,Score,MethodInput")] Questions questions, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    questions.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                    await FileGeneratore.SaveFile("Questions", questions.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                }
                if (questions.Method.Contains("گزینه ای"))
                {
                    questions.MethodInput = null;
                }

                string score = questions.Score.ToString();
                score = score.PersianToEnglish();
                questions.Score = double.Parse(score);
                questions.QuestionID = CodeGeneratore.ActiveCode();
                _context.Add(questions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = questions.TestID });
            }
            return RedirectToAction(nameof(Index), new { id = questions.TestID });
        }

        // GET: Testing/Questions/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }
            return PartialView(questions);
        }

        // POST: Testing/Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("QuestionID,ImageName,TestID,Method,Title,Score,MethodInput")] Questions questions, IFormFile ImgUp)
        {
            if (id != questions.QuestionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgUp != null)
                    {
                        if (questions.ImageName != null)
                        {
                            FileGeneratore.DeleteFile("Questions", questions.ImageName, _hostingEnvironment.WebRootPath);
                        }
                        questions.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                        await FileGeneratore.SaveFile("Questions", questions.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                    }
                    if (questions.Method.Contains("گزینه ای"))
                    {
                        questions.MethodInput = null;
                    }
                    else
                    {
                        if (await _context.Choices.AnyAsync(s => s.QuestionID == questions.QuestionID))
                        {
                            foreach (var item in _context.Choices.Where(s => s.QuestionID == questions.QuestionID))
                            {
                                _context.Remove(item);
                            }
                        }
                    }
                    string score = questions.Score.ToString();
                    score = score.PersianToEnglish();
                    questions.Score = double.Parse(score);
                    _context.Update(questions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsExists(questions.QuestionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = questions.TestID });
            }
            return RedirectToAction(nameof(Index), new { id = questions.TestID });
        }

        // GET: Testing/Questions/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions
                .Include(q => q.Tests)
                .FirstOrDefaultAsync(m => m.QuestionID == id);
            if (questions == null)
            {
                return NotFound();
            }

            return PartialView(questions);
        }

        // POST: Testing/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var questions = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(questions);
            if (questions.ImageName != null)
            {
                FileGeneratore.DeleteFile("Questions", questions.ImageName, _hostingEnvironment.WebRootPath);
            }

            if (await _context.Choices.AnyAsync(s => s.QuestionID == questions.QuestionID))
            {
                foreach (var item in _context.Choices.Where(s => s.QuestionID == questions.QuestionID))
                {
                    _context.Remove(item);
                }
            }

            if (await _context.QuestionReplys.AnyAsync(s => s.QuestionID == questions.QuestionID))
            {
                foreach (var item in _context.QuestionReplys.Include(s => s.Questions).Where(s => s.QuestionID == questions.QuestionID))
                {
                    if (item.Questions.Method.Contains("تشریحی"))
                    {
                        var reply = await _context.ReplyDescriptives.FirstOrDefaultAsync(s =>
                            s.ReplyID == item.ReplyID);
                        _context.Remove(reply);
                        if (reply.ImageName != null)
                        {
                            FileGeneratore.DeleteFile("Replys/Thumb", reply.ImageName, _hostingEnvironment.WebRootPath);
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
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = questions.TestID });
        }

        public IActionResult ListReplyDescriptive(string id)
        {
            ViewData["ID"] = id;
            return View();
        }

        public async Task<IActionResult> ListReplyOptional(string id)
        {
            var Question = await _Tests.GetQuestion(id);
            if (Question == null)
            {
                return NotFound();
            }

            ViewData["QuestionTitle"] = Question.Title;
            ViewData["TestID"] = Question.TestID;
            var List = _context.ReplyOptionals.Include(s => s.Choices).Include(s => s.QuestionReply).Include(s => s.QuestionReply.Users.ProfileStudents).Where(s => s.QuestionReply.QuestionID == id);
            return View(await List.ToListAsync());
        }
        public async Task<IActionResult> ChangeCondition(string id)
        {
            var Reply = await _context.ReplyDescriptives.Include(s => s.QuestionReplys).FirstOrDefaultAsync(s => s.DescriptiveID == id);
            return PartialView(Reply);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeCondition(string id, bool? IsCondition)
        {
            var Reply = await _context.ReplyDescriptives.Include(s => s.QuestionReplys).Include(s => s.QuestionReplys.Questions)
                .FirstOrDefaultAsync(s => s.DescriptiveID == id);
            if (ModelState.IsValid)
            {
                if (IsCondition != null)
                {
                    Reply.IsCondition = IsCondition;
                    _context.Update(Reply);
                    await _context.SaveChangesAsync();
                    var TestID = Reply.QuestionReplys.Questions.TestID;
                    var UserID = Reply.QuestionReplys.UserID;
                    var report = await _Tests.UltimateReport(TestID, UserID);
                    await _Tests.UpdateReportTest(TestID, UserID, report);

                    return ViewComponent("ListReplyDescriptive", new { id = Reply.QuestionReplys.QuestionID });
                }
                else
                {
                    return null;
                }
            }
            return ViewComponent("ListReplyDescriptive", new { id = Reply.QuestionReplys.QuestionID });
        }

        public IActionResult Users(string id)
        {
            return ViewComponent("User", new { id = id });
        }
        private bool QuestionsExists(string id)
        {
            return _context.Questions.Any(e => e.QuestionID == id);
        }
    }
}
