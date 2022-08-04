using StudyProgressManagement.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    public class ProgressStatisticsController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/ProgressStatistics
        public ActionResult Index()
        {
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        public JsonResult LoadStudentCourses(string majorId)
        {
            // get student courses data from database
            return Json(db.student_course.Where(s => s.major_id == majorId).Select(s => new
            {
                id = s.id,
                course = s.course
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadKnowledgeType(int studentCourseId)
        {
            // get student courses data from database
            return Json(db.knowledge_type.Where(s => s.student_course_id == studentCourseId).Select(s => new
            {
                id = s.knowledge_type_alias,
                group_2 = s.group_2,
                group_3 = s.group_3
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStatistics(int studentCourseId, string[] knowledge_name, string[] knowledge_ids, int[] credits)
        {
            // Declare variables
            var list = new List<Tuple<string, string, int, int>>();
            var query_studyResult = db.study_results.Where(s => s.student_course_id == studentCourseId && s.is_pass != null);
            var query_knowledge = db.knowledge_type.Where(k => k.student_course_id == studentCourseId);
            int studentsCount = db.students.Where(s => s.student_course_id == studentCourseId).GroupBy(s => s.id).Count();


            foreach (var knowledge_id in knowledge_ids.Select((value, index) => new { value, index }))
            {
                // Reset number in every foreach
                int passStudents = 0;
                int failStudents = 0;
                int i = knowledge_id.index;

                // Get study results group by student base on each knowledge type
                var query_sum = query_studyResult.Where(s => s.curriculum.knowledge_type.knowledge_type_alias.Equals(knowledge_id.value))
                .GroupBy(s => s.student_id).Select(s => new { Id = s.Key, Sum = s.Sum(item => item.curriculum.credits) }).ToList();

                foreach (var result in query_sum)
                {
                    // Check if students's credits pass
                    if (result.Sum >= credits[i])
                    {
                        passStudents++;
                    }
                }
                failStudents = studentsCount - passStudents;
                list.Add(Tuple.Create(knowledge_id.value, knowledge_name[i], passStudents, failStudents));
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentList(int studentCourseId, int inputCreditsFrom, int inputCreditsTo)
        {

            var query_originalNoCulmulative = db.knowledge_type.Where(s => s.student_course_id == studentCourseId && s.knowledge_type_alias == "DCKTL").FirstOrDefault();

            // Get study results statistics group by student
            var query_studyResult = db.study_results.Where(s => s.student_course_id == studentCourseId && s.is_pass != null).GroupBy(s => s.student_id).Select(s => new
            {
                id = s.Key,
                full_name = s.Select(n => n.student.full_name).FirstOrDefault(),
                class_student = s.Select(c => c.student.class_student_id).FirstOrDefault(),
                sum = s.Where(item => item.curriculum.knowledge_type.knowledge_type_alias != "DCKTL").Sum(item => item.curriculum.credits),
                current_no_culmulative =
                s.Where(item => item.curriculum.knowledge_type.knowledge_type_alias == "DCKTL").Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum() + "/" + query_originalNoCulmulative.compulsory_credits
            });

            // Query for student list
            var query_studentList = query_studyResult.Where(s => s.sum >= inputCreditsFrom && s.sum <= inputCreditsTo);

            return Json(query_studentList.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}