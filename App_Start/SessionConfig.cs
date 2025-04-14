using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_truyen.Models;

namespace Web_truyen.App_Start
{
    public static class SessionConfig
    {
        public static void SetUser(Users user)
        {
            HttpContext.Current.Session["user"] = user;
        }
        public static Users GetUser()
        {
            return (Users)HttpContext.Current.Session["user"];
        }
    }
}