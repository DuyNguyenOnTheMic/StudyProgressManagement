using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudyProgressManagement.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();
        private ApplicationUserManager _userManager;

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
                role = u.AspNetRoles.FirstOrDefault().Name,

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            // Get user role
            var query_role = db.AspNetUsers.Find(id).AspNetRoles.FirstOrDefault();
            if (query_role != null)
            {
                // Set selected role
                ViewBag.role_id = new SelectList(db.AspNetRoles, "id", "name", query_role.Id);
            }
            else
            {
                // Populate new role select list
                ViewBag.role_id = new SelectList(db.AspNetRoles, "id", "name");
            }

            return View(db.AspNetUsers.Where(x => x.Id == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(AspNetUser aspNetUser, string role_id)
        {
            // Get old info
            var oldUser = UserManager.FindById(aspNetUser.Id);
            var oldRole = UserManager.GetRoles(oldUser.Id).FirstOrDefault();
            var result = new IdentityResult();

            int adminCount = db.AspNetUsers.Where(u => u.AspNetRoles.FirstOrDefault().Name == "Admin").Count();

            // Check if user has any role
            var role = db.AspNetRoles.Find(role_id);

            if (oldRole == null)
            {
                // Add user to role
                result = UserManager.AddToRole(aspNetUser.Id, role.Name);
            }
            else
            {
                // Update user role
                UserManager.RemoveFromRole(aspNetUser.Id, oldRole);
                result = UserManager.AddToRole(aspNetUser.Id, role.Name);
            }

            return Json(new { result.Succeeded, message = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}