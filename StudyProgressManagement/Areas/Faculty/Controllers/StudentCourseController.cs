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

        public ActionResult GetData()
        {
            List<student_course> majorList = db.student_course.ToList();
            return Json(new { data = majorList }, JsonRequestBehavior.AllowGet);
        }
    }
}