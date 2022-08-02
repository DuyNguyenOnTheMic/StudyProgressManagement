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
    }
}