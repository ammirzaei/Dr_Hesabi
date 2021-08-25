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

namespace Dr_Hesabi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    [AuthorizeRole("Admin")]
    [RequireHttps]
    public class SurveysController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ISurveys _ISurveys;

        public SurveysController(DataBaseContext context, IWebHostEnvironment hostingEnvironment, ISurveys iSurveys)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _ISurveys = iSurveys;
        }

        // GET: Admin/Surveys
        public async Task<IActionResult> Index()
        {
            return View(await _context.Surveys.Include(s => s.SurveysQuestions).ToListAsync());
        }

        // GET: Admin/Surveys/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surveys = await _context.Surveys
                .FirstOrDefaultAsync(m => m.SurveyID == id);
            if (surveys == null)
            {
                return NotFound();
            }

            return View(surveys);
        }

        // GET: Admin/Surveys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Surveys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SurveyID,CountStar,Title,Description,ImageName,StartDate,EndDate,IsActive,IsPermission")] Surveys surveys, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    if (surveys.CountStar > 0)
                    {
                        surveys.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                        await FileGeneratore.SaveFile("Surveys/Surveys_Lists", surveys.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                        surveys.EndDate = surveys.EndDate.ToDateTimeM();
                        surveys.StartDate = surveys.StartDate.ToDateTimeM();
                        surveys.SurveyID = CodeGeneratore.ActiveCode();
                        _context.Add(surveys);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("CountStar", "لطفا تعداد ستاره های نظرسنجی را بیشتر از 0 وارد نمایید");
                    }
                }
                else
                {
                    ModelState.AddModelError("ImageName", "لطفا تصویر نظرسنجی را انتخاب نمایید");
                }
            }
            return View(surveys);
        }

        // GET: Admin/Surveys/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surveys = await _context.Surveys.FindAsync(id);
            if (surveys == null)
            {
                return NotFound();
            }
            return View(surveys);
        }

        // POST: Admin/Surveys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SurveyID,CountStar,Title,Description,ImageName,StartDate,EndDate,IsActive,IsPermission")] Surveys surveys, IFormFile ImgUp)
        {
            if (id != surveys.SurveyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (surveys.CountStar > 0)
                    {
                        if (ImgUp != null)
                        {
                            FileGeneratore.DeleteFile("Surveys/Surveys_Lists", surveys.ImageName, _hostingEnvironment.WebRootPath);

                            surveys.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                            await FileGeneratore.SaveFile("Surveys/Surveys_Lists", surveys.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                        }
                        surveys.EndDate = surveys.EndDate.ToDateTimeM();
                        surveys.StartDate = surveys.StartDate.ToDateTimeM();
                        _context.Update(surveys);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("CountStar", "لطفا تعداد ستاره های نظرسنجی را بیشتر از 0 وارد نمایید");
                        return View(surveys);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveysExists(surveys.SurveyID))
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
            return View(surveys);
        }

        // GET: Admin/Surveys/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surveys = await _context.Surveys
                .FirstOrDefaultAsync(m => m.SurveyID == id);
            if (surveys == null)
            {
                return NotFound();
            }

            return View(surveys);
        }

        // POST: Admin/Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var surveys = await _context.Surveys.FindAsync(id);
            _context.Surveys.Remove(surveys);
            FileGeneratore.DeleteFile("Surveys/Surveys_Lists", surveys.ImageName, _hostingEnvironment.WebRootPath);
            if (await _context.SurveysQuestions.AnyAsync(s => s.SurveyID == surveys.SurveyID))
            {
                foreach (var item in _context.SurveysQuestions.Where(s => s.SurveyID == surveys.SurveyID))
                {
                    if (await _context.SurveysVotes.AnyAsync(s => s.QuestionID == item.QuestionID))
                    {
                        foreach (var vote in _context.SurveysVotes.Where(s => s.QuestionID == item.QuestionID))
                        {
                            _context.Remove(vote);
                        }
                    }
                    _context.Remove(item);
                    FileGeneratore.DeleteFile("Surveys/Surveys_Items", item.ImageName, _hostingEnvironment.WebRootPath);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Participants(string id)
        {
            var survey = await _context.Surveys.Include(s => s.SurveysQuestions).FirstOrDefaultAsync(s => s.SurveyID == id);
            if (survey == null)
            {
                return NotFound();
            }

            ViewData["SurveyTitle"] = survey.Title;
            var model = _context.SurveysVotes.Include(s => s.Users).Include(s=>s.SurveysQuestions).Where(s => s.SurveysQuestions.SurveyID == survey.SurveyID);
            return View(await model.ToListAsync());
        }

        public async Task<IActionResult> DeleteParticipant(string id)
        {
            var surveyVote = await _context.SurveysVotes.Include(s => s.Users).FirstOrDefaultAsync(s => s.VoteID == id);
            if (surveyVote == null)
                return NotFound();
            return PartialView(surveyVote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteParticipant")]
        public async Task<IActionResult> DeleteParticipantConfirm(string id)
        {
            var surveyVote = await _context.SurveysVotes.Include(s => s.SurveysQuestions).FirstOrDefaultAsync(s => s.VoteID == id);
            if (surveyVote == null)
                return NotFound();
            _context.Remove(surveyVote);
            await _context.SaveChangesAsync();
            var question = await _context.SurveysQuestions.FindAsync(surveyVote.QuestionID);
            question.SumVote = await _ISurveys.SumVotes(question.QuestionID);
            await _ISurveys.EditQuestion(question);

            return RedirectToAction(nameof(Participants), new { id = surveyVote.SurveysQuestions.SurveyID });
        }
        private bool SurveysExists(string id)
        {
            return _context.Surveys.Any(e => e.SurveyID == id);
        }
    }
}
