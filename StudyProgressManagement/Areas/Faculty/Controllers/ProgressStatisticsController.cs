using StudyProgressManagement.Models;
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
    }
}