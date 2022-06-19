using StudyProgressManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult GetData()
        {
            List<major> majorList = db.majors.ToList();
            return Json(new { data = majorList },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            return View(new major());
        }

        [HttpPost]
        public ActionResult AddOrEdit(major major)
        {
            db.majors.Add(major);
            db.SaveChanges();
            return Json(new { success = true, message = "Lưu thành công!" }, JsonRequestBehavior.AllowGet);
        }
        
    }
}