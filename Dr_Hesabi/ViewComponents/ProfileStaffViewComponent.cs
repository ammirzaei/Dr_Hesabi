using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{
    public class ProfileStaffViewComponent:ViewComponent
    {
        private readonly IProfile _IProfile;
        public ProfileStaffViewComponent(IProfile iProfile)
        {
            _IProfile = iProfile;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            return View(await _IProfile.GetProfileStaff(UserID));
        }
    }
}
