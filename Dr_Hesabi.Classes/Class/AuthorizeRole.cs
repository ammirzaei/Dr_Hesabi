using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Dr_Hesabi.Classes.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace Dr_Hesabi.Classes.Class
{
    public class AuthorizeRole : Attribute, IAuthorizationFilter
    {
        private string Role;

        public AuthorizeRole(string role)
        {
            Role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                IAccount IAccount = (IAccount)context.HttpContext.RequestServices.GetService(typeof(IAccount));
                string UserID = context.HttpContext.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value.ToString();

                if (!IAccount.ExistRoleUser(UserID, Role))
                {
                    context.Result = new RedirectResult("/Dashboard");
                }
            }
            else
            {
                context.Result = new RedirectResult("/Account/Login");
            }
        }
    }
}
