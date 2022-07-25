using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Student.Controllers
{
    [Authorize]
    public class LearningProgressController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Student/LearningProgress
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetStudentInfo(string studentId)
        {
            // Get student information
            var query_student = db.students.Where(s => s.id == studentId).FirstOrDefault();
            if (query_student != null)
            {
                var studentInfo = query_student.full_name + " - " + studentId + " - " + query_student.student_course.course + " Ngành " + query_student.student_course.major.name;
                return studentInfo;
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetData(string studentId)
        {
            var query_student = db.students.Where(s => s.id == studentId).FirstOrDefault();
            var query_studyResult = db.study_results.OrderByDescending(s => s.id);
            // Get study results of student
            if (query_student != null)
            {
                return Json(db.curricula.Where(s => s.student_course_id == query_student.student_course_id).Select(s => new
                {
                    curriculum_id = s.curriculum_id,
                    curriculum_name = s.name,
                    credits = s.credits,
                    theoretical_hours = s.theoretical_hours,
                    practice_hours = s.practice_hours,
                    internship_hours = s.internship_hours,
                    project_hours = s.project_hours,
                    compulsory_or_optional = s.compulsory_or_optional,
                    knowledge_type_group_1 = s.knowledge_type.group_1,
                    knowledge_type_group_2 = s.knowledge_type.group_2,
                    knowledge_type_group_3 = s.knowledge_type.group_3,
                    compulsory_credits = s.knowledge_type.compulsory_credits,
                    optional_credits = s.knowledge_type.optional_credits,
                    mark10 = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.mark10).FirstOrDefault().ToString(),
                    mark10_2 = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.mark10_2).FirstOrDefault().ToString(),
                    max_mark_10 = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.max_mark_10).FirstOrDefault().ToString(),
                    max_mark_letter = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.max_mark_letter).FirstOrDefault().ToString(),
                    is_pass = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.is_pass).FirstOrDefault().ToString()

                }).ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = true }, JsonRequestBehavior.AllowGet);
        }
    }
}