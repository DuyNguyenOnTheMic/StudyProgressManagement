using StudyProgressManagement.Areas.Student.Middleware;
using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Student.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Student/Statistics
        [GetStudentID]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetStatistics(string studentId, int studentCourseId)
        {
            // Declare variables
            var query_studyResult = db.study_results.Where(s => s.student_id == studentId && s.is_pass != null);

            // Get credits count for each knowledge type based on study result
            var query_knowledge = db.knowledge_type.Where(k => k.student_course_id == studentCourseId).GroupBy(k => k.knowledge_type_alias).Select(k => new
            {
                id = k.Key,
                group_2 = k.Select(item => item.group_2).FirstOrDefault(),
                group_3 = k.Select(item => item.group_3).FirstOrDefault(),
                compulsory_credits = k.Select(item => item.compulsory_credits).FirstOrDefault(),
                optional_credits = k.Select(item => item.optional_credits).FirstOrDefault(),
                sum = query_studyResult.Where(item => item.curriculum.knowledge_type.knowledge_type_alias == k.Key).Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum()
            });

            return Json(query_knowledge.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}