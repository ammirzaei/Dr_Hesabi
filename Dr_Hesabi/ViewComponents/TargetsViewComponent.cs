using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{
    public class TargetsViewComponent : ViewComponent
    {
        private readonly ISetting _ISetting;

        public TargetsViewComponent(ISetting iSetting)
        {
            _ISetting = iSetting;
        }
        [ResponseCache(Duration = 3600)]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Setting setting = await _ISetting.GetSetting();
            List<string> List = new List<string>();
            if(setting.Targets!=null)
                List = setting.Targets.Split(',').ToList();
            return View(List.ToList());
        }
    }
}
