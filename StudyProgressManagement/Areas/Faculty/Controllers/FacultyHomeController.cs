using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class FacultyHomeController : Controller
    {
        // GET: Faculty/FacultyHome
        public ActionResult Index()
        {
            return View();
        }
    }
}