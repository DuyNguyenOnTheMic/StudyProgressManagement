using StudyProgressManagement.Models;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class ProgressStatisticsController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/ProgressStatistics
        public ActionResult Index()
        {
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        public JsonResult LoadStudentCourses(string majorId)
        {
            // get student courses data from database
            return Json(db.student_course.Where(s => s.major_id == majorId).Select(s => new
            {
                id = s.id,
                course = s.course
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStatistics(int studentCourseId, int credits)
        {
            int passStudents = 0, failStudents = 0;

            // Get study results group by student (except for optional knowledge_type)
            var query_studyResult = db.study_results.Where(s => s.student_course_id == studentCourseId && s.is_pass != null && s.curriculum.knowledge_type.knowledge_type_alias
            != "DCKTL").GroupBy(s => s.student_id).Select(s => new { Id = s.Key, Sum = s.Sum(item => item.curriculum.credits) }).ToList();

            foreach (var result in query_studyResult)
            {
                // Check if students's credits pass
                if (result.Sum >= credits)
                {
                    passStudents++;
                }
                else
                {
                    failStudents++;
                }
            }

            var finalResult = Json(new { passStudents = passStudents, failStudents = failStudents });
            return Json(finalResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentList(int studentCourseId, int credits, bool isTrue)
        {
            // Get study results group by student (except for optional knowledge_type)
            var query_studyResult = db.study_results.Where(s => s.student_course_id == studentCourseId && s.is_pass != null && s.curriculum.knowledge_type.knowledge_type_alias != "DCKTL").GroupBy(s => s.student_id).
            Select(s => new { id = s.Key, full_name = s.Select(n => n.student.full_name).Distinct(), class_student = s.Select(c => c.student.class_student_id).Distinct(), sum = s.Sum(item => item.curriculum.credits) });

            // Query for student list
            var query_studentList = query_studyResult;
            if (isTrue)
            {
                // Get success students
                query_studentList = query_studentList.Where(s => s.sum >= credits);
            }
            else
            {
                // Get fail students
                query_studentList = query_studentList.Where(s => s.sum < credits);
            }

            return Json(query_studentList.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}