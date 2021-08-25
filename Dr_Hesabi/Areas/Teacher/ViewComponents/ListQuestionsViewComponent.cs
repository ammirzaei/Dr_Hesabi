using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Testing.ViewComponents
{
    [ViewComponent(Name = "ListQuestions")]
    public class ListQuestionsViewComponent : ViewComponent
    {
        private readonly DataBaseContext _context;

        public ListQuestionsViewComponent(DataBaseContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var dataBaseContext = _context.Questions.Include(s=>s.QuestionReplys).Where(s => s.TestID == id).Include(q => q.Tests).Include(s => s.Choices);
            
            return View(await dataBaseContext.ToListAsync());
        }
    }
}
