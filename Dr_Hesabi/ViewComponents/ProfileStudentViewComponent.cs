using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.Controllers;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Hesabi.ViewComponents
{
    public class ProfileStudentViewComponent : ViewComponent
    {
        private readonly IProfile _IProfile;
        public ProfileStudentViewComponent(IProfile iProfile)
        {
            _IProfile = iProfile;
        }
        private async Task<DashboardController.ConditionProfileRequest> GetConditionProfileRequest(string UserID)
        {
            DashboardController.ConditionProfileRequest condition = DashboardController.ConditionProfileRequest.Null;
            if (await _IProfile.ExistProfileRequest(UserID))
            {
                bool? IsCondition = await _IProfile.IsConditionProfileRequest(UserID);
                if (IsCondition == true)
                    condition = DashboardController.ConditionProfileRequest.True;
                else if (IsCondition == false)
                    condition = DashboardController.ConditionProfileRequest.False;
                else
                    condition = DashboardController.ConditionProfileRequest.Awit;
            }
            return condition;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string UserID = HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();
            ViewBag.ProfileRequest = await GetConditionProfileRequest(UserID);
            return View(await _IProfile.GetProfile(UserID));
        }
    }
}
