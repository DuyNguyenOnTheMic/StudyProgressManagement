using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Admin/Role
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetData()
        {
            // Get majors data from datatabse
            return Json(db.AspNetUsers.Select(m => new
            {
                id = m.Id,
                mail = m.UserName,
                role = m.PhoneNumber,

            }).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}