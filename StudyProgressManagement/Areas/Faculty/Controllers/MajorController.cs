using StudyProgressManagement.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class MajorController : Controller
    {
        readonly SEP25Team03Entities db = new SEP25Team03Entities();

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
                m.id,
                m.name,

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new major());
        }

        [HttpPost]
        public ActionResult Create(major major)
        {
            try
            {
                // Create new major
                db.majors.Add(major);
                db.SaveChanges();
                return Json(new { success = true, message = "Lưu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(db.majors.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(major major)
        {
            // Update major
            db.Entry(major).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(new { success = true, message = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                // Delete major
                major major = db.majors.Find(id);
                db.majors.Remove(major);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Xoá thành công!" }, JsonRequestBehavior.AllowGet);
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