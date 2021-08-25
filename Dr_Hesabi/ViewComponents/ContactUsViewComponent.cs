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
    public class ContactUsViewComponent : ViewComponent
    {
        private readonly ISetting _ISetting;

        public ContactUsViewComponent(ISetting iSetting)
        {
            _ISetting = iSetting;
        }

        [ResponseCache(Duration = 3600)]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Setting setting = await _ISetting.GetSetting();

            return View(new ContactUsViewModel
            {
                ImgCodeQR = setting.ImgCodeQR,
                Address = setting.Address,
                Telegram = setting.Telegram,
                Telephone = setting.Telephone
            });
        }
    }
}
