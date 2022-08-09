using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers.Tests
{
    [TestClass()]
    public class StudentCourseControllerTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new StudentCourseController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Get_Student_Course_Json_Data_Not_Null_Test()
        {
            // Arrange.
            var controller = new StudentCourseController();

            // Act.
            var actionResult = controller.GetData();

            // Assert.
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            dynamic jsonCollection = actionResult.Data;
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id);
                Assert.IsNotNull(json.course);
                Assert.IsNotNull(json.major_id);
                Assert.IsNotNull(json.major_name);
            }
        }

        [TestMethod()]
        public void Get_Student_Course_Json_Data_Correctly_Test()
        {
            // Arrange.
            var controller = new StudentCourseController();

            // Act.
            var actionResult = controller.GetData();

            // Assert.
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            dynamic jsonCollection = actionResult.Data;
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.course,
                    "JSON record does not contain \"course\" required property.");
                Assert.IsNotNull(json.major_id,
                    "JSON record does not contain \"major_id\" required property.");
                Assert.IsNotNull(json.major_name,
                    "JSON record does not contain \"major_name\" required property.");
            }
        }

        [TestMethod()]
        public void Student_Course_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new StudentCourseController();

            // Act
            var actionResult = controller.GetData();
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
            var controller = new StudentCourseController();

            // Act
            var actionResult = controller.GetData();
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Student_Course_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new StudentCourseController();

            // Act
            var actionResult = controller.GetData();
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
                Assert.IsNotNull(json.major_id,
                    "JSON record does not contain \"major_id\" required property.");
                Assert.IsNotNull(json.major_name,
                    "JSON record does not contain \"major_name\" required property.");
            }
        }

        [TestMethod]
        public void Student_Course_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var db = new SEP25Team03Entities();

            // Act
            var actionResult = controller.GetData();
            var model = db.student_course.ToList();

            // Assert
            dynamic jsonCollection = actionResult.Data;

            Assert.AreEqual(model.Count, jsonCollection.Count);
        }

        [TestMethod()]
        public void Add_View_Redirect_Test()
        {
            // Arrange
            var controller = new StudentCourseController();

            // Act
            ViewResult result = controller.AddOrEdit() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.ViewData["Title"]);
        }

        [TestMethod()]
        public void Edit_View_Redirect_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var result = controller.AddOrEdit(query_studentCourse.id) as ViewResult;
            var major = new SelectList(db.majors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.ViewData["Title"]);
            Assert.AreEqual(major.Count(), ((IEnumerable<dynamic>)result.ViewBag.major_id).Count());
        }

        [TestMethod]
        public void Add_StudentCourse_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var db = new SEP25Team03Entities();
            var student_Course = new student_course() { course = "Khoá 1", year_study = "2012 - 2016", major_id = "7480104" };

            // Act
            var query_studentCourse = db.student_course.Where(s => s.major_id == student_Course.major_id).FirstOrDefault();
            if (query_studentCourse == null)
            {
                controller.AddOrEdit(student_Course);
            }
            var createResult = controller.GetData();
            dynamic jsonCollection = createResult.Data;

            // Assert
            Assert.AreEqual(db.student_course.Count(), jsonCollection.Count);
        }

        [TestMethod]
        public void Add_Shoud_Be_Failed_When_Course_Is_Null_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var student_Course = new student_course() { course = null, year_study = "2012 - 2016", major_id = "7480104" };

            // Assert
            Assert.IsTrue(ValidateModel(student_Course).Where(x => x.ErrorMessage.Equals("Bạn chưa nhập khoá sinh viên")).Count() > 0);
        }

        [TestMethod]
        public void Add_Shoud_Be_Failed_When_Course_Over_100_Characters_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var student_Course = new student_course() { course = "usposueremisedaccumsanliguladiamatdsdasdsaddasasaddusposueremisedaccumsanliguladiamatdsdasdsaddasasad", year_study = "2022", major_id = "7480104" };

            // Assert
            Assert.IsTrue(ValidateModel(student_Course).Where(x => x.ErrorMessage.Equals("Tối đa 100 kí tự được cho phép")).Count() > 0);
        }

        [TestMethod()]
        public void Edit_Student_Course_Data_Should_Load_Correctly_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var db = new SEP25Team03Entities();
            var student_Course = new student_course() { course = "Khoá 1", year_study = "2012 - 2016", major_id = "7480104" };

            // Act
            controller.AddOrEdit(student_Course);
            var query_studentCourse = db.student_course.Where(s => s.major_id == student_Course.major_id).FirstOrDefault();
            student_Course.id = query_studentCourse.id;
            controller.AddOrEdit(student_Course);
            var result = controller.AddOrEdit(student_Course) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(query_studentCourse);
            Assert.AreEqual(query_studentCourse.id, student_Course.id);
            Assert.AreEqual(query_studentCourse.course, student_Course.course);
            Assert.AreEqual(query_studentCourse.year_study, student_Course.year_study);
            Assert.AreEqual(query_studentCourse.major_id, student_Course.major_id);
        }

        [TestMethod]
        public void Edit_Student_Course_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var db = new SEP25Team03Entities();
            var student_Course = new student_course() { course = "Khoá 1", year_study = "2012 - 2016", major_id = "7480104" };

            // Act
            var query_studentCourse = db.student_course.Where(s => s.major_id == student_Course.major_id).FirstOrDefault();
            student_Course.id = query_studentCourse.id;
            var result = controller.AddOrEdit(student_Course) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.success);
        }

        [TestMethod]
        public void Delete_Student_Course_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.Where(s => s.major_id == "7480104").FirstOrDefault();
            var result = controller.Delete(query_studentCourse.id) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.success);
        }

        [TestMethod()]
        public void Delete_All_Data_About_Student_Course_Test()
        {
            // Arrange
            var controller = new StudentCourseController();
            var db = new SEP25Team03Entities();
            var student_Course = new student_course() { course = "Khoá 1", year_study = "2012 - 2016", major_id = "7480104" };

            // Act
            var query_studentCourse = db.student_course.Where(s => s.major_id == student_Course.major_id).FirstOrDefault();
            controller.AddOrEdit(student_Course);

            var result = controller.DeleteAll(query_studentCourse.id) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.success);
        }
    }
}