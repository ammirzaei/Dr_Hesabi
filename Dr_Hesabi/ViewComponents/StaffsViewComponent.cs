using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{
    public class StaffsViewComponent:ViewComponent
    {
        private readonly IViewComponents _IViewComponent;

        public StaffsViewComponent(IViewComponents IViewComponent)
        {
            _IViewComponent = IViewComponent;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _IViewComponent.GetAllStaffs());
        }
    }
}
