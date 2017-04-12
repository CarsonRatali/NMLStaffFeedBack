using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NMLStaffFeedback
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            #region Custom URLS (User Friendly)
            routes.MapRoute(
                                "Login", 
                                "Login/", 
                                new { controller = "Login", action = "Login" } 
                            );

            routes.MapRoute(
                              "Feedback", 
                              "Feedback/", 
                              new { controller = "Feedback", action = "Index" } 
                            );

            routes.MapRoute(
                             "Register", 
                             "Register/",  
                             new { controller = "Login", action = "Register" } 
                          );

            routes.MapRoute(
                             "Questions",
                             "Questions/",
                             new { controller = "Question", action = "Index" }
                          );

            routes.MapRoute(
                         "Answers",
                         "Answers/",
                         new { controller = "Answers", action = "Index" }
                       );

            routes.MapRoute(
                            "Employees",
                            "Employees/",
                            new { controller = "Employee", action = "Index" }
                           );
            #endregion

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
