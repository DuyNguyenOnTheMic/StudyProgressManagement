using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers.Tests
{
    [TestClass()]
    public class StudyProgramControllerTests
    {
        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            ViewResult result = controller.Index() as ViewResult;
            var major = new SelectList(db.majors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(major.Count(), ((IEnumerable<dynamic>)result.ViewBag.majors).Count());
        }

        [TestMethod()]
        public void Get_Study_Program_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.GetData(query_studentCourse.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.curriculum_id);
                Assert.IsNotNull(json.name);
            }
        }

        [TestMethod()]
        public void Get_Study_Program_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.GetData(query_studentCourse.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.curriculum_id,
                    "JSON record does not contain \"curriculum_id\" required property.");
                Assert.IsNotNull(json.name,
                    "JSON record does not contain \"name\" required property.");
            }
        }
    }
}