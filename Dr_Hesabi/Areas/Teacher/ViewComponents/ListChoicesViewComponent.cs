using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Areas.Testing.ViewComponents
{
    [ViewComponent(Name = "ListChoices")]
    public class ListChoicesViewComponent : ViewComponent
    {
        private readonly DataBaseContext db;

        public ListChoicesViewComponent(DataBaseContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var dataBaseContext = db.Choices.Include(c => c.Questions).Where(s => s.QuestionID == id);
            return View(await dataBaseContext.ToListAsync());
        }
    }
}
