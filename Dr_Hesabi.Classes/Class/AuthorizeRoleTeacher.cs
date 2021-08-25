using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dr_Hesabi.Classes.Class
{
    public class AuthorizeRoleTeacher : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var UserID = int.Parse(context.HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                context.Result = new RedirectResult("/Account/Login");
            }
        }
    }
}
