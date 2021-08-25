using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using Dr_Hesabi.Classes.Class;

namespace Dr_Hesabi.Controllers
{
    [AutoValidateAntiforgeryToken]
    [RequireHttps]
    public class SurveysController : Controller
    {
        private readonly ISurveys _ISurveys;

        public SurveysController(ISurveys iSurveys)
        {
            _ISurveys = iSurveys;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _ISurveys.GetAllSurveys());
        }

        [Route("Surveys/{id}/{title}")]
        public async Task<IActionResult> ShowSurveys(string id, string title)
        {
            var Surveys = await _ISurveys.GetSurveys(id, title);
            if (Surveys == null)
            {
                return NotFound();
            }

            return View(Surveys);
        }

        public async Task<IActionResult> VoteQuestion(string id)
        {
            var question = await _ISurveys.GetQuestion(id);

            ViewData["CountStar"] = question.Surveys.CountStar;
            ViewData["QuestionTitle"] = question.Title;
            return PartialView(new SurveysVotes()
            {
                QuestionID = id
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VoteQuestion(SurveysVotes surveysVotes)
        {
            var question = await _ISurveys.GetQuestion(surveysVotes.QuestionID);
            if (ModelState.IsValid)
            {
                if (question.Surveys.IsActive && question.Surveys.StartDate <= DateTime.Now &&
                    question.Surveys.EndDate >= DateTime.Now)
                {
                    if (surveysVotes.Vote > 0 && surveysVotes.Vote <= question.Surveys.CountStar)
                    {
                        string UserID = HttpContext.User.Claims
                            .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
                        if (question.Surveys.IsPermission)
                        {
                            if (!await _ISurveys.ExistUserQuestionPermission(UserID, question.QuestionID))
                            {
                                return await CommandVote(surveysVotes, UserID, question, false);
                            }
                            else
                            {
                                return await CommandVote(surveysVotes, UserID, question, true);
                            }
                        }
                        else
                        {
                            if (!await _ISurveys.ExistUserQuestion(UserID, question.SurveyID))
                            {
                                return await CommandVote(surveysVotes, UserID, question, false);
                            }
                            else
                            {
                                if (await _ISurveys.ExistUserQuestionPermission(UserID, question.QuestionID))
                                {
                                    return await CommandVote(surveysVotes, UserID, question, true);
                                }
                                else
                                {
                                    return await CommandVote(surveysVotes, UserID, question, null);
                                }
                            }
                        }
                    }
                }
            }
            return ViewComponent("ShowSurveysQuestionViewComponent",
                new
                {
                    id = question.Surveys.SurveyID,
                    title = question.Surveys.Title,
                    Command = "در ثبت رأی مشکلی به وجود آمده است."
                });
        }

        private async Task<IActionResult> CommandVote(SurveysVotes surveysVotes, string UserID, SurveysQuestions question, bool? IsEdit)
        {
            if (IsEdit == false)
            {
                surveysVotes.DateTime = DateTime.Now;
                surveysVotes.UserID = UserID;
                surveysVotes.VoteID = CodeGeneratore.ActiveCode();
                await _ISurveys.AddVote(surveysVotes);
                question.SumVote = await _ISurveys.SumVotes(question.QuestionID);
                await _ISurveys.EditQuestion(question);
                return ViewComponent("ShowSurveysQuestionViewComponent",
                    new
                    {
                        id = question.Surveys.SurveyID,
                        title = question.Surveys.Title,
                        Command = " کاربر عزیز رأی شما با موفقیت ثبت شد."
                    });
            }
            else if (IsEdit == true)
            {
                var SVote = await _ISurveys.GetVote(UserID, surveysVotes.QuestionID);
                if (SVote.Vote != surveysVotes.Vote)
                {
                    SVote.DateTime = DateTime.Now;
                    SVote.Vote = surveysVotes.Vote;
                    await _ISurveys.EditVote(SVote);
                    question.SumVote = await _ISurveys.SumVotes(question.QuestionID);
                    await _ISurveys.EditQuestion(question);
                }

                return ViewComponent("ShowSurveysQuestionViewComponent",
                   new
                   {
                       id = question.Surveys.SurveyID,
                       title = question.Surveys.Title,
                       Command = " کاربر عزیز رأی شما با موفقیت ویرایش شد."
                   });
            }
            else
            {
                string questionIdOld = String.Empty;
                var vote = await _ISurveys.GetVoteinSurveynotPermission(UserID, question.SurveyID);
                questionIdOld = vote.QuestionID;
                vote.DateTime = DateTime.Now;
                vote.Vote = surveysVotes.Vote;
                vote.QuestionID = surveysVotes.QuestionID;
                await _ISurveys.EditVote(vote);

                var questionOld = await _ISurveys.GetQuestion(questionIdOld);
                questionOld.SumVote = await _ISurveys.SumVotes(questionIdOld);
                await _ISurveys.EditQuestion(questionOld);

                question.SumVote = await _ISurveys.SumVotes(question.QuestionID);
                await _ISurveys.EditQuestion(question);

                return ViewComponent("ShowSurveysQuestionViewComponent",
                    new
                    {
                        id = question.Surveys.SurveyID,
                        title = question.Surveys.Title,
                        Command = "کاربر عزیز رأی جدید شما برای سوال دیگری ثبت شد."
                    });
            }
        }
    }
}
