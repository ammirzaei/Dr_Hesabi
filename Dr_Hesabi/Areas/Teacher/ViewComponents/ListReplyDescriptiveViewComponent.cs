using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Testing.ViewComponents
{
    [ViewComponent(Name = "ListReplyDescriptive")]
    public class ListReplyDescriptiveViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public ListReplyDescriptiveViewComponent(DataBaseContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var Question = await db.Questions.FindAsync(id);
            var List = db.ReplyDescriptives.Include(s => s.QuestionReplys.Users).ThenInclude(s => s.ProfileStudents).Include(s => s.QuestionReplys).ThenInclude(s => s.Questions).Where(s => s.QuestionReplys.QuestionID == id);

            ViewData["QuestionTitle"] = Question.Title;
            ViewData["TestID"] = Question.TestID;
            return View(await List.ToListAsync());
        }
    }
}
