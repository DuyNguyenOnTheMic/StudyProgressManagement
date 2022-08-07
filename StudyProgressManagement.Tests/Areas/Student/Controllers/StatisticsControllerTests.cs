using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Student.Controllers.Tests
{
    [TestClass()]
    public class StatisticsControllerTests
    {
        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new StatisticsController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Get_Statistics_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new StatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_student = db.students.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_student.id, query_student.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id);
                Assert.IsNotNull(json.sum);
            }
        }

        [TestMethod()]
        public void Get_Statistics_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new StatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_student = db.students.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_student.id, query_student.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.sum,
                    "JSON record does not contain \"sum\" required property.");
            }
        }

        [TestMethod()]
        public void Statistics_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new StatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_student = db.students.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_student.id, query_student.student_course_id);
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
            var controller = new StatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_student = db.students.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_student.id, query_student.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Statistics_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new StatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_student = db.students.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_student.id, query_student.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            for (var i = 0; i < jsonCollection.Count; i++)
            {

                var json = jsonCollection[i];

                Assert.IsNotNull(json);
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.sum,
                    "JSON record does not contain \"sum\" required property.");
            }
        }

        [TestMethod()]
        public void Statistics_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new StatisticsController();
            var db = new SEP25Team03Entities();

            // Act
            var query_student = db.students.FirstOrDefault();
            var actionResult = controller.GetStatistics(query_student.id, query_student.student_course_id);
            var query_knowledge = db.knowledge_type.Where(k => k.student_course_id == query_student.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_knowledge.Count());
        }
    }
}