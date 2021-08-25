using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{
    [ViewComponent(Name = "ShowSurveysQuestionViewComponent")]
    public class ShowSurveysQuestionViewComponent : ViewComponent
    {
        private readonly ISurveys _ISurveys;

        public ShowSurveysQuestionViewComponent(ISurveys iSurveys)
        {
            _ISurveys = iSurveys;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id, string title, string Command = "")
        {
            if (Command != "")
            {
                ViewData["Command"] = Command;
            }

            var Survey = await _ISurveys.GetSurveys(id, title);
            ViewData["IsActive"] =
                Survey.IsActive && Survey.StartDate <= DateTime.Now && Survey.EndDate >= DateTime.Now;
            if (bool.Parse(ViewData["IsActive"].ToString()) == false)
            {
                if (User.Identity.IsAuthenticated)
                {
                    string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
                    ViewData["IsUser"] = await _ISurveys.ExistUserQuestion(UserID, Survey.SurveyID);
                }
            }
            List<string> Labels = await _ISurveys.GetLabels(id);
            List<float> Values = await _ISurveys.GetValues(id);
            ViewData["Labels"] = Labels;
            ViewData["Values"] = Values;
            return View(await _ISurveys.GetAllSurveysQuestion(id, title));
        }
    }
}
