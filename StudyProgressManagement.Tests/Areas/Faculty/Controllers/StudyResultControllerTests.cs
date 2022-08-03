using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers.Tests
{
    [TestClass()]
    public class StudyResultControllerTests
    {
        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new StudyResultController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Search_View_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            ViewResult result = controller.Search() as ViewResult;
            var major = new SelectList(db.majors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(major.Count(), ((IEnumerable<dynamic>)result.ViewBag.majors).Count());
        }

        [TestMethod()]
        public void Get_Search_Study_Result_Json_Data_With_Class_Not_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, query_classStudent.id, "");
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id);
                Assert.IsNotNull(json.full_name);
            }
        }

        [TestMethod()]
        public void Get_Search_Study_Result_Json_Data_With_Class_Correctly_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, query_classStudent.id, "");
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.full_name,
                    "JSON record does not contain \"full_name\" required property.");
            }
        }

        [TestMethod()]
        public void Search_Study_Result_Json_Data_With_Class_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, query_classStudent.id, "");
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
        public void Search_Study_Result_Json_Data_With_Class_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, query_classStudent.id, "");
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Search_Study_Result_JSon_Data_With_Class_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, query_classStudent.id, "");
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
            }
        }

        [TestMethod()]
        public void Search_Study_Result_JSon_Data_With_Class_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var query_count = db.students.Where(s => s.student_course_id == query_studentCourse.id &&
                s.class_student_id == query_classStudent.id && s.full_name.Contains(""));

            var actionResult = controller.Search(query_studentCourse.id, query_classStudent.id, "");
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_count.Count());
        }

        [TestMethod()]
        public void Get_Search_Study_Result_Json_Data_With_Name_Not_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var query_student = db.students.Where(c => c.student_course_id == query_studentCourse.id && c.class_student_id == query_classStudent.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, "", query_student.full_name);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id);
                Assert.IsNotNull(json.full_name);
            }
        }

        [TestMethod()]
        public void Get_Search_Study_Result_Json_Data_With_Name_Correctly_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var query_student = db.students.Where(c => c.student_course_id == query_studentCourse.id && c.class_student_id == query_classStudent.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, "", query_student.full_name);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.full_name,
                    "JSON record does not contain \"full_name\" required property.");
            }
        }

        [TestMethod()]
        public void Search_Study_Result_Json_Data_With_Name_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var query_student = db.students.Where(c => c.student_course_id == query_studentCourse.id && c.class_student_id == query_classStudent.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, "", query_student.full_name);
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
        public void Search_Study_Result_Json_Data_With_Name_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var query_student = db.students.Where(c => c.student_course_id == query_studentCourse.id && c.class_student_id == query_classStudent.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, "", query_student.full_name);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Search_Study_Result_JSon_Data_With_Name_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var query_student = db.students.Where(c => c.student_course_id == query_studentCourse.id && c.class_student_id == query_classStudent.id).FirstOrDefault();
            var actionResult = controller.Search(query_studentCourse.id, "", query_student.full_name);
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
            }
        }

        [TestMethod()]
        public void Search_Study_Result_JSon_Data_With_Name_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(c => c.student_course_id == query_studentCourse.id).FirstOrDefault();
            var query_student = db.students.Where(c => c.student_course_id == query_studentCourse.id && c.class_student_id == query_classStudent.id).FirstOrDefault();
            var query_count = db.students.Where(s => s.student_course_id == query_studentCourse.id &&
                s.full_name.Contains(query_student.full_name));

            var actionResult = controller.Search(query_studentCourse.id, "", query_student.full_name);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_count.Count());
        }

        [TestMethod()]
        public void Get_Student_Info_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_student = db.students.FirstOrDefault();
            var expected = query_student.full_name + " - " + query_student.id + " - " + query_student.student_course.course + " Ngành " + query_student.student_course.major.name;
            var actionResult = controller.GetStudentInfo(query_student.id);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(actionResult, expected);
        }

        [TestMethod()]
        public void Get_Student_Info_Data_Null_When_Not_Found_Test()
        {
            // Arrange
            var controller = new StudyResultController();

            // Act
            var studentId = "197pm212212";
            var actionResult = controller.GetStudentInfo(studentId);

            // Assert
            Assert.IsNull(actionResult);
            Assert.AreEqual(null, actionResult);
        }

        [TestMethod()]
        public void Get_Study_Result_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetData(query_studyResult.student_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.curriculum_id);
                Assert.IsNotNull(json.curriculum_name);
                Assert.IsNotNull(json.mark10);
                Assert.IsNotNull(json.mark10_2);
                Assert.IsNotNull(json.max_mark_10);
                Assert.IsNotNull(json.max_mark_letter);
                Assert.IsNotNull(json.is_pass);
                Assert.IsNotNull(json.regis_result_id);
            }
        }

        [TestMethod()]
        public void Get_Study_Result_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetData(query_studyResult.student_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.curriculum_id,
                    "JSON record does not contain \"curriculum_id\" required property.");
                Assert.IsNotNull(json.curriculum_name,
                    "JSON record does not contain \"curriculum_name\" required property.");
                Assert.IsNotNull(json.mark10,
                    "JSON record does not contain \"mark10\" required property.");
                Assert.IsNotNull(json.mark10_2,
                    "JSON record does not contain \"mark10_2\" required property.");
                Assert.IsNotNull(json.max_mark_10,
                    "JSON record does not contain \"max_mark_10\" required property.");
                Assert.IsNotNull(json.max_mark_letter,
                    "JSON record does not contain \"max_mark_letter\" required property.");
                Assert.IsNotNull(json.is_pass,
                    "JSON record does not contain \"is_pass\" required property.");
                Assert.IsNotNull(json.regis_result_id,
                    "JSON record does not contain \"regis_result_id\" required property.");
            }
        }

        [TestMethod()]
        public void Study_Result_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetData(query_studyResult.student_id);
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
        public void Study_Result_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetData(query_studyResult.student_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Study_Result_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetData(query_studyResult.student_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            for (var i = 0; i < jsonCollection.Count; i++)
            {

                var json = jsonCollection[i];

                Assert.IsNotNull(json);
                Assert.IsNotNull(json.curriculum_id,
                    "JSON record does not contain \"curriculum_id\" required property.");
                Assert.IsNotNull(json.curriculum_name,
                    "JSON record does not contain \"curriculum_name\" required property.");
                Assert.IsNotNull(json.mark10,
                    "JSON record does not contain \"mark10\" required property.");
                Assert.IsNotNull(json.mark10_2,
                    "JSON record does not contain \"mark10_2\" required property.");
                Assert.IsNotNull(json.max_mark_10,
                    "JSON record does not contain \"max_mark_10\" required property.");
                Assert.IsNotNull(json.max_mark_letter,
                    "JSON record does not contain \"max_mark_letter\" required property.");
                Assert.IsNotNull(json.is_pass,
                    "JSON record does not contain \"is_pass\" required property.");
                Assert.IsNotNull(json.regis_result_id,
                    "JSON record does not contain \"regis_result_id\" required property.");
            }
        }

        [TestMethod()]
        public void Study_Result_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studyResult = db.study_results.FirstOrDefault();
            var actionResult = controller.GetData(query_studyResult.student_id);
            var query_curriculum = db.curricula.Where(s => s.student_course_id == query_studyResult.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_curriculum.Count());
        }
    }
}