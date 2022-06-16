using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    public class FacultyHomeController : Controller
    {
        // GET: Faculty/FacultyHome
        public ActionResult Index()
        {
            return View();
        }
    }
}