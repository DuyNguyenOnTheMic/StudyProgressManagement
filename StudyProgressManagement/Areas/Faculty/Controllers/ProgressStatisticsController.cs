﻿using StudyProgressManagement.Models;
using StudyProgressManagement.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class ProgressStatisticsController : Controller
    {
        readonly SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/ProgressStatistics
        public ActionResult Index()
        {
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        public JsonResult LoadStudentCourses(string majorId)
        {
            // get student courses data from database
            return Json(db.student_course.Where(s => s.major_id == majorId).ToList().OrderBy(x => new string(x.course.Where(char.IsLetter).ToArray())).ThenBy(x =>
            {
                // Natural sorting
                if (int.TryParse(new string(x.course.Where(char.IsDigit).ToArray()), out int number))
                    return number;
                return -1;
            }).Select(s => new
            {
                s.id,
                s.course
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadKnowledgeType(int studentCourseId)
        {
            // get student courses data from database
            return Json(db.knowledge_type.Where(s => s.student_course_id == studentCourseId).Select(s => new
            {
                id = s.knowledge_type_alias,
                s.group_2,
                s.group_3
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStatistics(int studentCourseId, string[] knowledge_name, string[] knowledge_ids, int[] credits)
        {
            // Declare variables
            var statisticsList = new List<Tuple<string, string, int, int, int>>();
            var query_studyResult = db.study_results.Where(s => s.student_course_id == studentCourseId && s.is_pass != null);
            int studentsCount = db.students.Where(s => s.student_course_id == studentCourseId).GroupBy(s => s.id).Count();

            foreach (var knowledge_id in knowledge_ids.Select((value, index) => new { value, index }))
            {
                // Reset number in every foreach
                int passStudents = 0;
                int failStudents = 0;
                int i = knowledge_id.index;

                // Get study results group by student base on each knowledge type
                var query_sum = query_studyResult.Where(s => s.curriculum.knowledge_type.knowledge_type_alias.Equals(knowledge_id.value))
                .GroupBy(s => s.student_id).Select(s => new { Id = s.Key, Sum = s.Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum() }).ToList();

                foreach (var result in query_sum)
                {
                    // Check if students's credits pass
                    if (result.Sum >= credits[i])
                    {
                        passStudents++;
                    }
                }
                failStudents = studentsCount - passStudents;
                statisticsList.Add(Tuple.Create(knowledge_id.value, knowledge_name[i], passStudents, failStudents, credits[i]));

                // Send progress to progress bar
                Functions.SendProgress("Đang lấy dữ liệu...", i, knowledge_ids.Count());
            }

            return Json(statisticsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentList(int studentCourseId, int inputCredits, string inputKnowledge, bool isTrue)
        {
            // Get study results of input knowledge type
            var query_studyResult = db.study_results.Where(item => item.is_pass != null && item.curriculum.knowledge_type.knowledge_type_alias.Equals(inputKnowledge));

            // Query for student lists
            var query_students = db.students.Where(s => s.student_course_id == studentCourseId).GroupBy(s => s.id).Select(s => new
            {
                id = s.Key,
                s.FirstOrDefault().full_name,
                s.FirstOrDefault().class_student_id,
                sum = query_studyResult.Where(item => item.student_id == s.Key).Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum()
            });

            var query_studentList = query_students;
            if (isTrue)
            {
                // Get success students
                query_studentList = query_studentList.Where(s => s.sum >= inputCredits);
            }
            else
            {
                // Get fail students
                query_studentList = query_studentList.Where(s => s.sum < inputCredits);
            }

            return Json(query_studentList.ToList(), JsonRequestBehavior.AllowGet);
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