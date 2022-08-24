using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers.Tests
{
    [TestClass()]
    public class ProgressStatisticsControllerTests
    {
        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Get_Student_Course_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_major = db.majors.FirstOrDefault();
            var actionResult = controller.LoadStudentCourses(query_major.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id);
                Assert.IsNotNull(json.course);
            }
        }

        [TestMethod()]
        public void Get_Student_Course_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_major = db.majors.FirstOrDefault();
            var actionResult = controller.LoadStudentCourses(query_major.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.course,
                    "JSON record does not contain \"course\" required property.");
            }
        }

        [TestMethod()]
        public void Student_Course_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_major = db.majors.FirstOrDefault();
            var actionResult = controller.LoadStudentCourses(query_major.id);
            dynamic jsonCollection = actionResult.Data;
            int count = 0;
            foreach (var value in jsonCollection)
            {
                count++;
            }

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void Student_Course_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_major = db.majors.FirstOrDefault();
            var actionResult = controller.LoadStudentCourses(query_major.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Student_Course_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_major = db.majors.FirstOrDefault();
            var actionResult = controller.LoadStudentCourses(query_major.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            for (var i = 0; i < jsonCollection.Count; i++)
            {

                var json = jsonCollection[i];

                Assert.IsNotNull(json);
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.course,
                    "JSON record does not contain \"course\" required property.");
            }
        }

        [TestMethod()]
        public void Student_Course_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_major = db.majors.FirstOrDefault();
            var query_studentCourse = db.student_course.Where(s => s.major_id == query_major.id);
            var actionResult = controller.LoadStudentCourses(query_major.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_studentCourse.Count());
        }

        [TestMethod()]
        public void Get_Knowledge_Type_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_knowledgeType = db.knowledge_type.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_knowledgeType.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id);
            }
        }

        [TestMethod()]
        public void Get_Knowledge_Type_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_knowledgeType = db.knowledge_type.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_knowledgeType.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
            }
        }

        [TestMethod()]
        public void Knowledge_Type_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_knowledgeType = db.knowledge_type.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_knowledgeType.student_course_id);
            dynamic jsonCollection = actionResult.Data;
            int count = 0;
            foreach (var value in jsonCollection)
            {
                count++;
            }

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void Knowledge_Type_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_knowledgeType = db.knowledge_type.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_knowledgeType.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Knowledge_Type_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_knowledgeType = db.knowledge_type.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_knowledgeType.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            for (var i = 0; i < jsonCollection.Count; i++)
            {

                var json = jsonCollection[i];

                Assert.IsNotNull(json);
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
            }
        }

        [TestMethod()]
        public void Knowledge_Type_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_knowledgeType = db.knowledge_type.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_knowledgeType.student_course_id);
            var query_knowledge_Type = db.knowledge_type.Where(s => s.student_course_id == query_knowledgeType.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_knowledge_Type.Count());
        }

        [TestMethod()]
        public void Get_Statistics_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            string[] knowledgeName = { "Đại cương", "Đại cương không tích luỹ" };
            string[] knowledgeIds = { "DC", "DCKTL" };
            int[] credits = { 10, 2 };

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_studentCourse.id, knowledgeName, knowledgeIds, credits);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(null, null);
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.Item1);
                Assert.IsNotNull(json.Item2);
                Assert.IsNotNull(json.Item3);
                Assert.IsNotNull(json.Item4);
                Assert.IsNotNull(json.Item5);
            }
        }

        [TestMethod()]
        public void Get_Statistics_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            string[] knowledgeName = { "Đại cương", "Đại cương không tích luỹ" };
            string[] knowledgeIds = { "DC", "DCKTL" };
            int[] credits = { 10, 2 };

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_studentCourse.id, knowledgeName, knowledgeIds, credits);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.Item1,
                    "JSON record does not contain \"Item1\" required property.");
                Assert.IsNotNull(json.Item2,
                    "JSON record does not contain \"Item2\" required property.");
                Assert.IsNotNull(json.Item3,
                    "JSON record does not contain \"Item3\" required property.");
                Assert.IsNotNull(json.Item4,
                    "JSON record does not contain \"Item4\" required property.");
                Assert.IsNotNull(json.Item5,
                    "JSON record does not contain \"Item5\" required property.");
            }
        }

        [TestMethod()]
        public void Statistics_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            string[] knowledgeName = { "Đại cương", "Đại cương không tích luỹ" };
            string[] knowledgeIds = { "DC", "DCKTL" };
            int[] credits = { 10, 2 };

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_studentCourse.id, knowledgeName, knowledgeIds, credits);
            dynamic jsonCollection = actionResult.Data;
            int count = 0;
            foreach (var value in jsonCollection)
            {
                count++;
            }

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void Statistics_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            string[] knowledgeName = { "Đại cương", "Đại cương không tích luỹ" };
            string[] knowledgeIds = { "DC", "DCKTL" };
            int[] credits = { 10, 2 };

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_studentCourse.id, knowledgeName, knowledgeIds, credits);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Statistics_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            string[] knowledgeName = { "Đại cương", "Đại cương không tích luỹ" };
            string[] knowledgeIds = { "DC", "DCKTL" };
            int[] credits = { 10, 2 };

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_studentCourse.id, knowledgeName, knowledgeIds, credits);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            for (var i = 0; i < jsonCollection.Count; i++)
            {

                var json = jsonCollection[i];

                Assert.IsNotNull(json);
                Assert.IsNotNull(json.Item1,
                    "JSON record does not contain \"Item1\" required property.");
                Assert.IsNotNull(json.Item2,
                    "JSON record does not contain \"Item2\" required property.");
                Assert.IsNotNull(json.Item3,
                    "JSON record does not contain \"Item3\" required property.");
                Assert.IsNotNull(json.Item4,
                    "JSON record does not contain \"Item4\" required property.");
                Assert.IsNotNull(json.Item5,
                    "JSON record does not contain \"Item5\" required property.");
            }
        }

        [TestMethod()]
        public void Statistics_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            string[] knowledgeName = { "Đại cương", "Đại cương không tích luỹ" };
            string[] knowledgeIds = { "DC", "DCKTL" };
            int[] credits = { 10, 2 };

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var statisticsList = new List<Tuple<string, string, int, int, int>>();
            var query_studyResult = db.study_results.Where(s => s.student_course_id == query_studentCourse.id && s.is_pass != null);
            int studentsCount = db.students.Where(s => s.student_course_id == query_studentCourse.id).GroupBy(s => s.id).Count();

            foreach (var knowledge_id in knowledgeIds.Select((value, index) => new { value, index }))
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
                statisticsList.Add(Tuple.Create(knowledge_id.value, knowledgeName[i], passStudents, failStudents, credits[i]));
            }

            var actionResult = controller.GetStatistics(query_studentCourse.id, knowledgeName, knowledgeIds, credits);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, statisticsList.Count());
        }

        [TestMethod()]
        public void Get_Student_List_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            int inputCredits = 2;
            string inputKnowledge = "DC";
            bool isTrue = true;

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetStudentList(query_studyResult.student_course_id, inputCredits, inputKnowledge, isTrue);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id);
                Assert.IsNotNull(json.full_name);
                Assert.IsNotNull(json.class_student_id);
                Assert.IsNotNull(json.sum);
            }
        }

        [TestMethod()]
        public void Get_Student_List_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            int inputCredits = 2;
            string inputKnowledge = "DC";
            bool isTrue = true;

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetStudentList(query_studyResult.student_course_id, inputCredits, inputKnowledge, isTrue);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.full_name,
                    "JSON record does not contain \"full_name\" required property.");
                Assert.IsNotNull(json.class_student_id,
                    "JSON record does not contain \"class_student_id\" required property.");
                Assert.IsNotNull(json.sum,
                    "JSON record does not contain \"sum\" required property.");
            }
        }

        [TestMethod()]
        public void Student_List_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            int inputCredits = 2;
            string inputKnowledge = "DC";
            bool isTrue = true;

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetStudentList(query_studyResult.student_course_id, inputCredits, inputKnowledge, isTrue);
            dynamic jsonCollection = actionResult.Data;
            int count = 0;
            foreach (var value in jsonCollection)
            {
                count++;
            }

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void Student_List_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            int inputCredits = 2;
            string inputKnowledge = "DC";
            bool isTrue = true;

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetStudentList(query_studyResult.student_course_id, inputCredits, inputKnowledge, isTrue);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Student_List_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            int inputCredits = 2;
            string inputKnowledge = "DC";
            bool isTrue = true;

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetStudentList(query_studyResult.student_course_id, inputCredits, inputKnowledge, isTrue);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            for (var i = 0; i < jsonCollection.Count; i++)
            {

                var json = jsonCollection[i];

                Assert.IsNotNull(json);
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.full_name,
                    "JSON record does not contain \"full_name\" required property.");
                Assert.IsNotNull(json.class_student_id,
                    "JSON record does not contain \"class_student_id\" required property.");
                Assert.IsNotNull(json.sum,
                    "JSON record does not contain \"sum\" required property.");
            }
        }

        [TestMethod()]
        public void Student_List_JSon_Data_Count_Should_Be_Equal_If_True_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            int inputCredits = 2;
            string inputKnowledge = "DC";
            bool isTrue = true;

            // Act
            var query_studyResult = db.study_results.Where(item => item.is_pass != null && item.curriculum.knowledge_type.knowledge_type_alias.Equals(inputKnowledge));
            var query_studyResult_first = db.study_results.FirstOrDefault();
            var query_students = db.students.Where(s => s.student_course_id == query_studyResult_first.student_course_id).GroupBy(s => s.id).Select(s => new
            {
                id = s.Key,
                full_name = s.Select(n => n.full_name).FirstOrDefault(),
                class_student = s.Select(c => c.class_student_id).FirstOrDefault(),
                sum = query_studyResult.Where(item => item.student_id == s.Key).Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum()
            }).Where(s => s.sum >= inputCredits);

            var actionResult = controller.GetStudentList(query_studyResult_first.student_course_id, inputCredits, inputKnowledge, isTrue);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_students.Count());
        }

        [TestMethod()]
        public void Student_List_JSon_Data_Count_Should_Be_Equal_If_False_Test()
        {
            // Arrange
            var controller = new ProgressStatisticsController();
            var db = new SEP25Team03Entities();
            int inputCredits = 2;
            string inputKnowledge = "DC";
            bool isTrue = false;

            // Act
            var query_studyResult = db.study_results.Where(item => item.is_pass != null && item.curriculum.knowledge_type.knowledge_type_alias.Equals(inputKnowledge));
            var query_studyResult_first = db.study_results.FirstOrDefault();
            var query_students = db.students.Where(s => s.student_course_id == query_studyResult_first.id).GroupBy(s => s.id).Select(s => new
            {
                id = s.Key,
                full_name = s.Select(n => n.full_name).FirstOrDefault(),
                class_student = s.Select(c => c.class_student_id).FirstOrDefault(),
                sum = query_studyResult.Where(item => item.student_id == s.Key).Select(item => item.curriculum.credits).DefaultIfEmpty(0).Sum()
            }).Where(s => s.sum < inputCredits);

            var actionResult = controller.GetStudentList(query_studyResult_first.id, inputCredits, inputKnowledge, isTrue);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_students.Count());
        }
    }
}