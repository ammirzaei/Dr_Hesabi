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
using Microsoft.AspNetCore.Http;

namespace Dr_Hesabi.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize]
    [AuthorizeRole("Teacher,Admin")]
    [RequireHttps]
    public class ChoicesController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly DataBaseContext db;
        private readonly ITests _ITests;

        public ChoicesController(DataBaseContext context, DataBaseContext _db, ITests iTests)
        {
            _context = context;
            db = _db;
            _ITests = iTests;
        }

        // GET: Testing/Choices
        public async Task<IActionResult> Index(string id)
        {
            var Question = await _context.Questions.Include(s => s.Tests).FirstOrDefaultAsync(s => s.QuestionID == id);
            if (Question == null)
            {
                return NotFound();
            }
            ViewData["QuestionID"] = Question.QuestionID;
            ViewData["QuesrionTitle"] = Question.Title;
            ViewData["TestID"] = Question.Tests.TestID;
            return View();
        }


        // GET: Testing/Choices/Create
        public IActionResult Create(string id)
        {
            return PartialView(new Choices()
            {
                QuestionID = id
            });
        }

        // POST: Testing/Choices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChoiceID,QuestionID,Title,IsSuccess,Order")] Choices choices)
        {
            if (ModelState.IsValid)
            {
                if (choices.Order <= 0)
                {
                    int order = 0;
                    try
                    {
                        order = await _context.Choices.Where(s => s.QuestionID == choices.QuestionID)
                            .MaxAsync(s => s.Order);
                    }
                    catch
                    { }
                    choices.Order = order + 1;
                }

                await IsSuccess(choices);

                if (await _context.Choices.AnyAsync(s => s.QuestionID == choices.QuestionID && s.Order == choices.Order))
                {
                    var ChoiceOrder =
                        await _context.Choices.FirstOrDefaultAsync(s =>
                            s.QuestionID == choices.QuestionID && s.Order == choices.Order);
                    var OrderMax = await _context.Choices.MaxAsync(s => s.Order);
                    ChoiceOrder.Order = OrderMax + 1;
                    _context.Update(ChoiceOrder);
                }

                choices.ChoiceID = CodeGeneratore.ActiveCode();
                _context.Add(choices);
                await _context.SaveChangesAsync();
                if (choices.IsSuccess == false)
                {
                    RandomIsSuccess(choices.QuestionID);
                }
                return ViewComponent("ListChoices", new { id = choices.QuestionID });
            }
            return ViewComponent("ListChoices", new { id = choices.QuestionID });
        }

        private async Task IsSuccess(Choices choices)
        {
            if (choices.IsSuccess)
            {
                if (await _context.Choices.AnyAsync(s => s.QuestionID == choices.QuestionID && s.IsSuccess))
                {
                    var ChoiceIsSuccess =
                        await _context.Choices.FirstOrDefaultAsync(s =>
                            s.QuestionID == choices.QuestionID && s.IsSuccess);
                    ChoiceIsSuccess.IsSuccess = false;
                    _context.Update(ChoiceIsSuccess);
                }
            }
        }

        // GET: Testing/Choices/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var choices = await _context.Choices.FindAsync(id);
            if (choices == null)
            {
                return NotFound();
            }
            return PartialView(choices);
        }

        // POST: Testing/Choices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ChoiceID,QuestionID,Title,IsSuccess,Order")] Choices choices, string IsHidden)
        {
            if (id != choices.ChoiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (choices.Order <= 0)
                    {
                        int order = 0;
                        try
                        {
                            order = await _context.Choices.Where(s => s.QuestionID == choices.QuestionID)
                                .MaxAsync(s => s.Order);
                        }
                        catch
                        { }
                        choices.Order = order + 1;
                    }
                    await IsSuccess(choices);
                    if (await _context.Choices.AnyAsync(s => s.QuestionID == choices.QuestionID && s.Order == choices.Order))
                    {
                        using (db)
                        {
                            var ChoiceOrder =
                                await db.Choices.FirstOrDefaultAsync(s =>
                                    s.QuestionID == choices.QuestionID && s.Order == choices.Order);
                            var ChoiceThis = await db.Choices.FindAsync(choices.ChoiceID);
                            ChoiceOrder.Order = ChoiceThis.Order;
                            db.Update(ChoiceOrder);
                            await db.SaveChangesAsync();
                        }
                    }
                    if (IsHidden == "True")
                    {
                        choices.IsSuccess = true;
                    }
                    _context.Update(choices);
                    await _context.SaveChangesAsync();
                    if (choices.IsSuccess == false)
                    {
                        RandomIsSuccess(choices.QuestionID);
                    }

                    if (await _context.ReplyOptionals.AnyAsync(s => s.QuestionReply.QuestionID == choices.QuestionID))
                    {
                        foreach (var item in _context.ReplyOptionals.AsNoTracking().Where(s => s.QuestionReply.QuestionID == choices.QuestionID))
                        {
                            item.IsCondition =
                                await _ITests.GetChoiceIsSuccess(choices.QuestionID, item.ChoiceID);
                            await _ITests.UpdateReplyOptional(item);
                        }
                    }

                    choices = await _context.Choices.Include(s => s.Questions).FirstOrDefaultAsync(s => s.ChoiceID == choices.ChoiceID);
                    if (await _context.TestsUltimate.AnyAsync(s => s.TestID == choices.Questions.TestID))
                    {
                        foreach (var item in _context.TestsUltimate.AsNoTracking().Where(s => s.TestID == choices.Questions.TestID))
                        {
                            var report = await _ITests.UltimateReport(item.TestID, item.UserID);
                            await _ITests.UpdateReportTest(item.TestID, item.UserID, report);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoicesExists(choices.ChoiceID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return ViewComponent("ListChoices", new { id = choices.QuestionID });
            }
            return ViewComponent("ListChoices", new { id = choices.QuestionID });
        }

        // GET: Testing/Choices/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var choices = await _context.Choices
                .Include(c => c.Questions)
                .FirstOrDefaultAsync(m => m.ChoiceID == id);
            if (choices == null)
            {
                return NotFound();
            }

            return PartialView(choices);
        }

        // POST: Testing/Choices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var choices = await _context.Choices.FindAsync(id);
            _context.Choices.Remove(choices);
            await _context.SaveChangesAsync();
            if (choices.IsSuccess)
            {
                RandomIsSuccess(choices.QuestionID);
            }
            return ViewComponent("ListChoices", new { id = choices.QuestionID });
        }

        private void RandomIsSuccess(string QuestionID)
        {
            List<Choices> List = _context.Choices.Where(s => s.QuestionID == QuestionID).ToList();
            if (List.Any())
            {
                if (!List.Any(s => s.IsSuccess))
                {
                    Random Rn = new Random();
                    var Index = Rn.Next(0, List.Count());
                    var choice = _context.Choices.Find(List[Index].ChoiceID);
                    choice.IsSuccess = true;
                    _context.SaveChanges();
                }
            }
        }
        private bool ChoicesExists(string id)
        {
            return _context.Choices.Any(e => e.ChoiceID == id);
        }
    }
}
