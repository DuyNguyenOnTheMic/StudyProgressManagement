using StudyProgressManagement.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class MajorController : Controller
    {
        sep_team03Entities db = new sep_team03Entities();

        // GET: Faculty/Major
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetData()
        {
            // Get majors data from datatabse
            return Json(db.majors.Select(m => new
            {
                id = m.id,
                name = m.name,

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(string id = null)
        {
            if (id == null)
            {
                return View(new major());
            }
            else
            {
                return View(db.majors.Where(x => x.id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(major major)
        {
            var query = db.majors.AsNoTracking().Where(x => x.id == major.id).FirstOrDefault();
            // Add or edit major
            if (query == null)
            {
                db.majors.Add(major);
                db.SaveChanges();
                return Json(new { success = true, message = "Lưu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.Entry(major).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, message = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                // Delete major
                major major = db.majors.Where(x => x.id == id).FirstOrDefault();
                db.majors.Remove(major);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Xoá thành công!" }, JsonRequestBehavior.AllowGet);
        }

    }
}