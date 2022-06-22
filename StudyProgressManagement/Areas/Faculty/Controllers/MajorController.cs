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
                major_id = m.major_id,
                major_name = m.major_name,

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
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
            // Add or edit major
            if (major.id == 0)
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
        public ActionResult Delete(int id)
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