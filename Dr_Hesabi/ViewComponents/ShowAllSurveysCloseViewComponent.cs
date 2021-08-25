using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{

    public class ShowAllSurveysCloseViewComponent : ViewComponent
    {
        private readonly ISurveys _ISurveys;

        public ShowAllSurveysCloseViewComponent(ISurveys iSurveys)
        {
            _ISurveys = iSurveys;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _ISurveys.GetAllSurveysClose());
        }
    }
}
