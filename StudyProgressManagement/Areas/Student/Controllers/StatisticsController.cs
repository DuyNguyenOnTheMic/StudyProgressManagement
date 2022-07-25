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

        [HttpPost]
        public JsonResult GetStatistics()
        {
            // Get student id
            string studentId = Session["StudentId"].ToString();

            // Get credits count for each knowledge type based on study result
            var query_studyResult = db.study_results.Where(s => s.student_id == studentId && s.is_pass != null)
            .GroupBy(s => s.curriculum.knowledge_type.knowledge_type_alias).Select(s => new
            {
                id = s.Key,
                group_2 = s.Select(k => k.curriculum.knowledge_type.group_2).FirstOrDefault(),
                group_3 = s.Select(k => k.curriculum.knowledge_type.group_3).FirstOrDefault(),
                compulsory_credits = s.Select(k => k.curriculum.knowledge_type.compulsory_credits).FirstOrDefault(),
                optional_credits = s.Select(k => k.curriculum.knowledge_type.optional_credits).FirstOrDefault(),
                sum = s.Sum(item => item.curriculum.credits)
            });

            return Json(query_studyResult.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}