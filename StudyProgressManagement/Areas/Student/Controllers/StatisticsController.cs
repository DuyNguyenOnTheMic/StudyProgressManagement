using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Student.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Student/Statistics
        public ActionResult Index()
        {
            return View();
        }
    }
}