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
        public JsonResult GetStatistics(int studentCourseId, int creditsFrom, int creditsTo)
        {
            int passStudents = 0;

            // Get study results group by student (except for optional knowledge_type)
            var query_studyResult = db.study_results.Where(s => s.student_course_id == studentCourseId && s.is_pass != null && s.curriculum.knowledge_type.knowledge_type_alias
            != "DCKTL").GroupBy(s => s.student_id).Select(s => new { Id = s.Key, Sum = s.Sum(item => item.curriculum.credits) }).ToList();

            foreach (var result in query_studyResult)
            {
                // Check if students's credits pass
                if (result.Sum >= creditsFrom && result.Sum <= creditsTo)
                {
                    passStudents++;
                }
            }

            var finalResult = Json(new { passStudents = passStudents });
            return Json(finalResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentList(int studentCourseId, int inputCreditsFrom, int inputCreditsTo)
        {

            var query_originalNoCulmulative = db.knowledge_type.Where(s => s.student_course_id == studentCourseId && s.knowledge_type_alias == "DCKTL").FirstOrDefault();

            // Get study results statistics group by student
            var query_studyResult = db.study_results.Where(s => s.student_course_id == studentCourseId && s.is_pass != null).GroupBy(s => s.student_id).Select(s => new
            {
                id = s.Key,
                full_name = s.Select(n => n.student.full_name).Distinct(),
                class_student = s.Select(c => c.student.class_student_id).Distinct(),
                sum = s.Where(item => item.curriculum.knowledge_type.knowledge_type_alias != "DCKTL").Sum(item => item.curriculum.credits),
                current_no_culmulative =
                s.Where(item => item.curriculum.knowledge_type.knowledge_type_alias == "DCKTL").Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum() + "/" + query_originalNoCulmulative.compulsory_credits
            });

            // Query for student list
            var query_studentList = query_studyResult;
            query_studentList = query_studentList.Where(s => s.sum >= inputCreditsFrom && s.sum <= inputCreditsTo);

            return Json(query_studentList.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}