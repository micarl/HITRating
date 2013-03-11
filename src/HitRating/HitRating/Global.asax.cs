using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tword
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Apies of Account
            routes.MapRoute(
                "AccountSearch",
                "Api/Accounts",
                new { controller = "RestfulAccount", action = "List" },
                new { Grendal = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                "AccountRead",
                "Api/Account/{userName}",
                new { controller = "RestfulAccount", action = "Read" },
                new { Grendal = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                "AccountCreate",
                "Api/Accounts",
                new { controller = "RestfulAccount", action = "Create" },
                new { Grendal = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                "AccountDelete",
                "Api/Account/{userName}",
                new { controller = "RestfulAccount", action = "Delete" },
                new { Grendal = new HttpMethodConstraint("DELETE") }
            );

            routes.MapRoute(
                "AccountLogOn",
                "Api/Account/{userName}/Session",
                new { controller = "RestfulAccount", action = "LogOn" },
                new { Grendal = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                "AccountLogOut",
                "Api/Account/{userName}/Session",
                new { controller = "RestfulAccount", action = "LogOut" },
                new { Grendal = new HttpMethodConstraint("DELETE") }
            );

            routes.MapRoute(
                "AccountLogStatusCheck",
                "Api/Account/{userName}/Session",
                new { controller = "RestfulAccount", action = "IsLogged" },
                new { Grendal = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                "AccountEmailRead",
                "Api/Account/{userName}/Email",
                new { controller = "RestfulAccount", action = "Email" },
                new { Grendal = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                "AccountPhotoRead",
                "Api/Account/{userName}/Photo",
                new { controller = "RestfulAccount", action = "Photo" },
                new { Grendal = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                "AccountEmailEdit",
                "Api/Account/{userName}/Email",
                new { controller = "RestfulAccount", action = "SetEmail" },
                new { Grendal = new HttpMethodConstraint("PUT") }
            );

            routes.MapRoute(
                "AccountPhotoEdit",
                "Api/Account/{userName}/Photo",
                new { controller = "RestfulAccount", action = "SetPhoto" },
                new { Grendal = new HttpMethodConstraint("PUT") }
            );

            routes.MapRoute(
                "AccountChangePassword",
                "Api/Account/{userName}/Password",
                new { controller = "RestfulAccount", action = "ChangePassword" },
                new { Grendal = new HttpMethodConstraint("PUT") }
            );

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}