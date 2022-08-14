using StudyProgressManagement.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Student.Middleware
{
    public class GetStudentID : ActionFilterAttribute
    {
        private readonly SEP25Team03Entities db = new SEP25Team03Entities();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            dynamic ViewBag = filterContext.Controller.ViewBag;

            // Get studentId from email
            string studentEmail = HttpContext.Current.User.Identity.Name;
            int pFrom = studentEmail.IndexOf(".") + 1;
            int pTo = studentEmail.LastIndexOf("@");

            string studentId = studentEmail.Substring(pFrom, pTo - pFrom);

            // Check if student has in database
            var query_student = db.students.Where(s => s.id == studentId).FirstOrDefault();
            if (query_student != null)
            {
                ViewBag.StudentId = query_student.id;
                ViewBag.StudentCourseId = query_student.student_course_id;
            }
        }
    }
}