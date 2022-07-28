using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers.Tests
{
    [TestClass()]
    public class StudentCourseControllerTests
    {
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
        public void Get_Student_Course_Json_Data_Test()
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
                Assert.IsNotNull(json.year_study,
                    "JSON record does not contain \"year_study\" required property.");
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
                Assert.IsNotNull(json.year_study,
                    "JSON record does not contain \"year_study\" required property.");
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
        public void Add_View_Test()
        {
            // Arrange
            var controller = new StudentCourseController();

            // Act
            ViewResult result = controller.AddOrEdit() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Add_View_Redirect_Test()
        {
            // Arrange
            var controller = new StudentCourseController();

            // Act
            var result = controller.AddOrEdit(2) as ViewResult;

            // Assert
            Assert.AreEqual("About Us", result.ViewData["Title"]);
        }
    }
}