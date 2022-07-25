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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStatistics()
        {
            string studentId = ViewBag.StudentID;
            var query_student = db.students.Where(s => s.id == studentId).FirstOrDefault();
            if (query_student != null)
            {
                var query_studyResult = db.study_results.Where(s => s.student_id == studentId && s.is_pass != null)
                .GroupBy(s => s.curriculum.knowledge_type.knowledge_type_alias).Select(s => new { Id = s.Key, Sum = s.Sum(item => item.curriculum.credits) }).ToList();
                return Json(query_studyResult.ToList(), JsonRequestBehavior.AllowGet);
            }
            return View();
        }
    }
}