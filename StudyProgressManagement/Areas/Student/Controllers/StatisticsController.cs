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
            var query_knowledge = db.knowledge_type.Where(k => k.student_course_id == studentCourseId).Select(k => new
            {
                id = k.knowledge_type_alias,
                group_2 = k.group_2,
                group_3 = k.group_3,
                compulsory_credits = k.compulsory_credits,
                optional_credits = k.optional_credits,
                sum = query_studyResult.Where(item => item.curriculum.knowledge_type_id == k.id).Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum()
            });

            return Json(query_knowledge.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}