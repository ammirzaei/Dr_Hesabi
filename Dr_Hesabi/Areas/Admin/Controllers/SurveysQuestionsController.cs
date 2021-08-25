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
    public class SurveysQuestionsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SurveysQuestionsController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Admin/SurveysQuestions
        public async Task<IActionResult> Index(string id)
        {
            var Surveys = await _context.Surveys.FindAsync(id);

            ViewData["SurveysID"] = Surveys.SurveyID;
            ViewData["SurveysTitle"] = Surveys.Title;
            return View();
        }


        // GET: Admin/SurveysQuestions/Create
        public IActionResult Create(string id)
        {
            return PartialView(new SurveysQuestions()
            {
                SurveyID = id
            });
        }

        // POST: Admin/SurveysQuestions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionID,SurveyID,Title,ImageName,SumVote")] SurveysQuestions surveysQuestions, IFormFile ImageName)
        {
            if (ModelState.IsValid)
            {
                if (ImageName != null)
                {
                    surveysQuestions.ImageName = FileGeneratore.NameFile(ImageName.FileName);
                    await FileGeneratore.SaveFile("Surveys/Surveys_Items", surveysQuestions.ImageName, ImageName, _hostingEnvironment.WebRootPath);
                    surveysQuestions.QuestionID = CodeGeneratore.ActiveCode();
                    _context.Add(surveysQuestions);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = surveysQuestions.SurveyID });
                }
            }
            return RedirectToAction(nameof(Index), new { id = surveysQuestions.SurveyID });
        }

        // GET: Admin/SurveysQuestions/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surveysQuestions = await _context.SurveysQuestions.FindAsync(id);
            if (surveysQuestions == null)
            {
                return NotFound();
            }

            return PartialView(surveysQuestions);
        }

        // POST: Admin/SurveysQuestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("QuestionID,SurveyID,Title,ImageName,SumVote")] SurveysQuestions surveysQuestions, IFormFile ImgUp)
        {
            if (id != surveysQuestions.QuestionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgUp != null)
                    {
                        FileGeneratore.DeleteFile("Surveys/Surveys_Items", surveysQuestions.ImageName, _hostingEnvironment.WebRootPath);
                        surveysQuestions.ImageName = FileGeneratore.NameFile(ImgUp.FileName);
                        await FileGeneratore.SaveFile("Surveys/Surveys_Items", surveysQuestions.ImageName, ImgUp, _hostingEnvironment.WebRootPath);
                    }
                    _context.Update(surveysQuestions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SurveysQuestionsExists(surveysQuestions.QuestionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = surveysQuestions.SurveyID });
            }
            return RedirectToAction(nameof(Index), new { id = surveysQuestions.SurveyID });
        }

        // GET: Admin/SurveysQuestions/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var surveysQuestions = await _context.SurveysQuestions
                .Include(s => s.Surveys)
                .FirstOrDefaultAsync(m => m.QuestionID == id);
            if (surveysQuestions == null)
            {
                return NotFound();
            }

            return PartialView(surveysQuestions);
        }

        // POST: Admin/SurveysQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var surveysQuestions = await _context.SurveysQuestions.FindAsync(id);
            _context.SurveysQuestions.Remove(surveysQuestions);
            FileGeneratore.DeleteFile("Surveys/Surveys_Items", surveysQuestions.ImageName, _hostingEnvironment.WebRootPath);

            if (await _context.SurveysVotes.AnyAsync(s => s.QuestionID == surveysQuestions.QuestionID))
            {
                foreach (var vote in _context.SurveysVotes.Where(s => s.QuestionID == surveysQuestions.QuestionID))
                {
                    _context.Remove(vote);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = surveysQuestions.SurveyID });
        }

        private bool SurveysQuestionsExists(string id)
        {
            return _context.SurveysQuestions.Any(e => e.QuestionID == id);
        }
    }
}
