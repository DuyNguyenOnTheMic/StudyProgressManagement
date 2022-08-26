using StudyProgressManagement.Areas.Student.Middleware;
using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Student.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        readonly SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Student/Statistics
        [GetStudentID]
        [OutputCache(Duration = 600, VaryByParam = "userMail")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(Duration = 600, VaryByParam = "studentId")]
        public JsonResult GetStatistics(string studentId, int studentCourseId)
        {
            // Declare variables
            var query_studyResult = db.study_results.Where(s => s.student_id == studentId && s.is_pass != null);

            // Get credits count for each knowledge type based on study result
            var query_knowledge = db.knowledge_type.Where(k => k.student_course_id == studentCourseId).Select(k => new
            {
                id = k.knowledge_type_alias,
                k.group_2,
                k.group_3,
                k.compulsory_credits,
                k.optional_credits,
                sum = query_studyResult.Where(item => item.curriculum.knowledge_type_id == k.id).Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum()
            });

            return Json(query_knowledge.ToList(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}