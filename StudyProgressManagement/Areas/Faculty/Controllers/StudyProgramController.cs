using StudyProgressManagement.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class StudyProgramController : Controller
    {
        sep_team03Entities db = new sep_team03Entities();

        // GET: Faculty/StudyProgram
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadStudentCourses(string majorId)
        {
            // get student courses data from database
            return Json(db.student_course.Where(s => s.major_id == majorId).Select(s => new
            {
                id = s.id,
                course = s.course
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Import()
        {
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase postedFile, string student_course)
        {
            var postedMajor = Request.Form["major"].ToString();
            var postedStudentCourse = Request.Form["student_course"].ToString();
            foreach (var imageFile in Request.Files)
            {

            }

            return Json(new { status = true, Message = "Account created." + postedMajor + postedStudentCourse });
        }
    }
}