using StudyProgressManagement.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class StudentCourseController : Controller
    {
        readonly SEP25Team03Entities db = new SEP25Team03Entities();

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
                s.id,
                s.course,
                s.major_id,
                major_name = s.major.name,
                s.year_study

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                // Return add student course view
                ViewBag.major_id = new SelectList(db.majors, "id", "name");
                return View(new student_course());
            }
            else
            {
                // Return edit student course view
                student_course student_course = db.student_course.Find(id);
                ViewBag.major_id = new SelectList(db.majors, "id", "name", student_course.major_id);
                return View(db.student_course.Find(id));
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
                student_course student_course = db.student_course.Find(id);
                db.student_course.Remove(student_course);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, message = "Xoá thành công!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteAll(int id)
        {
            try
            {
                // Delete all records about student course
                db.study_results.RemoveRange(db.study_results.Where(c => c.student_course_id == id));
                db.registration_results.RemoveRange(db.registration_results.Where(c => c.student_course_id == id));
                db.curricula.RemoveRange(db.curricula.Where(c => c.student_course_id == id));
                db.knowledge_type.RemoveRange(db.knowledge_type.Where(c => c.student_course_id == id));
                db.students.RemoveRange(db.students.Where(c => c.student_course_id == id));
                db.class_student.RemoveRange(db.class_student.Where(c => c.student_course_id == id));

                // Remove student course
                student_course student_course = db.student_course.Find(id);
                db.student_course.Remove(student_course);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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