using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{
    [ViewComponent(Name = "Majors")]
    public class MajorsViewComponent : ViewComponent
    {
        private readonly IViewComponents _IViewComponent;

        public MajorsViewComponent(IViewComponents IViewComponent)
        {
            _IViewComponent = IViewComponent;
        }
        [ResponseCache(Duration = 3600)]
        public async Task<IViewComponentResult> InvokeAsync(bool IsView = false)
        {
            if (IsView == true)
            {
                ViewBag.IsView = true;
            }
            return View(await _IViewComponent.GetAllMajors());
        }
    }
}
