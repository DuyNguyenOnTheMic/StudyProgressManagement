﻿using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty
{
    public class FacultyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Faculty";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Faculty_default",
                "Faculty/{controller}/{action}/{id}",
                new { controller = "FacultyHome", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}