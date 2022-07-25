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

        public JsonResult GetStatistics()
        {
            string studentId = Session["StudentId"].ToString();
            // Get credits count for each knowledge type based on study result
            var query_studyResult = db.study_results.Where(s => s.student_id == studentId && s.is_pass != null)
            .GroupBy(s => s.curriculum.knowledge_type.knowledge_type_alias).Select(s => new { Id = s.Key, group_2 = s.Select(k => k.curriculum.knowledge_type.group_2).FirstOrDefault(), Sum = s.Sum(item => item.curriculum.credits) });

            return Json(query_studyResult.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}