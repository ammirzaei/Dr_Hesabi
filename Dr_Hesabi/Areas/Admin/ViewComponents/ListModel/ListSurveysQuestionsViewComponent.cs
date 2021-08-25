using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Admin.ViewComponents.ListModel
{
    [ViewComponent(Name = "ListSurveysQuestionsViewComponent")]
    public class ListSurveysQuestionsViewComponent : ViewComponent
    {
        private readonly DataBaseContext _context;

        public ListSurveysQuestionsViewComponent(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var dataBaseContext = _context.SurveysQuestions.Include(s => s.Surveys).Include(s => s.SurveysVotes).Where(s => s.SurveyID == id);
            return View(await dataBaseContext.ToListAsync());
        }
    }
}
