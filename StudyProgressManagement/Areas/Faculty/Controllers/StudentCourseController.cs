using StudyProgressManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class StudentCourseController : Controller
    {
        sep_team03Entities db = new sep_team03Entities();

        // GET: Faculty/StudentCourse
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetData()
        {
            return Json(db.student_course.Select(s => new
            {
                id = s.id,
                course = s.course,
                major_id = s.major.name,
                
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}