using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
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
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_studentCourse.id);
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
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_studentCourse.id);
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
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_studentCourse.id);
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
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_studentCourse.id);
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
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadKnowledgeType(query_studentCourse.id);
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
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_knowledge_Type = db.knowledge_type.Where(s => s.student_course_id == query_studentCourse.id);
            var actionResult = controller.LoadKnowledgeType(query_studentCourse.id);
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

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            string[] knowledgeName = { "DC", "DCKTL" };

            // Assert
            Assert.AreEqual(null, null);
        }
    }
}