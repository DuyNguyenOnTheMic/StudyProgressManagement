using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Student.Controllers
{
    [Authorize]
    public class LearningProgressController : Controller
    {
        // GET: Student/LearningProgress
        public ActionResult Index()
        {
            return View();
        }
    }
}