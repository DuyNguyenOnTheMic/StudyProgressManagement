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
            return Json(db.AspNetUsers.Select(u => new
            {
                id = u.Id,
                email = u.Email,
                role = u.AspNetRoles.Select(item => item.Name),

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            // Get user role
            var query_role = db.AspNetUsers.Find(id);
            var role_id = query_role.AspNetRoles.Select(item => item.Id).FirstOrDefault();

            ViewBag.role_id = new SelectList(db.AspNetRoles, "id", "name", role_id);
            return View(db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault());
        }

        /*[HttpPost]
        public ActionResult Edit(major major)
        {
            // Update major
            db.Entry(major).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new { success = true, message = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
        }*/
    }
}