using StudyProgressManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class StudentCourseController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/StudentCourse
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetData()
        {
            // Get student courses data from datatabse
            return Json(db.student_course.Select(s => new
            {
                id = s.id,
                course = s.course,
                major_id = s.major_id,
                major_name = s.major.name,
                year_study = s.year_study

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            // Set Viewbag from major table
            ViewBag.major_id = new SelectList(db.majors, "id", "name");

            if (id == 0)
            {
                // Return add student course view
                return View(new student_course());
            }
            else
            {
                // Return edit student course view
                student_course student_course = db.student_course.Find(id);
                ViewBag.major_id = new SelectList(db.majors, "id", "name", student_course.major_id);
                return View(db.student_course.Where(x => x.id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(student_course student_course)
        {
            // Add or edit student course
            if (student_course.id == 0)
            {
                db.student_course.Add(student_course);
                db.SaveChanges();
                return Json(new { success = true, message = "Lưu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.Entry(student_course).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.major_id = new SelectList(db.majors, "id", "name", student_course.major_id);

                return Json(new { success = true, message = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // Delete student course
                student_course student_course = db.student_course.Where(x => x.id == id).FirstOrDefault();
                db.student_course.Remove(student_course);
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