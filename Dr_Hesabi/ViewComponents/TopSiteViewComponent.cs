using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{
    public class TopSiteViewComponent : ViewComponent
    {
        private readonly ISetting _ISetting;

        public TopSiteViewComponent(ISetting iSetting)
        {
            _ISetting = iSetting;
        }
        [ResponseCache(Duration = 3600)]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Setting setting = await _ISetting.GetSetting();
            return View(new TopSiteViewModel()
            {
                ImgLogo = setting.ImgLogo,
                NameSite = setting.NameSite,
                NameSite2 = setting.NameSite2
            });
        }
    }
}
