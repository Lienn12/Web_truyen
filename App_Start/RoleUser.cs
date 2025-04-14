using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web_truyen.Models;
namespace Web_truyen.App_Start
{
    public class RoleUser: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = SessionConfig.GetUser();
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary( new 
                    {
                        controller = "Users",
                        action = "Login",
                        area="Admin",
                    }));
                return;
            }
            if (string.IsNullOrEmpty(Roles) == false)
            {
                //var check = 
            }
            return;
        }
    }
}