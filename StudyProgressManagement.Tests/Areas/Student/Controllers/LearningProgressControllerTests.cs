using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Student.Controllers.Tests
{
    [TestClass()]
    public class LearningProgressControllerTests
    {
        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new LearningProgressController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Index_ViewName_Test()
        {
            // Arrange
            var controller = new LearningProgressController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod()]
        public void Get_Student_Info_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new LearningProgressController();
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
            var controller = new LearningProgressController();

            // Act
            var studentId = "197pm212212";
            var actionResult = controller.GetStudentInfo(studentId);

            // Assert
            Assert.IsNull(actionResult);
            Assert.AreEqual(null, actionResult);
        }

        [TestMethod()]
        public void Get_Learning_Progress_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new LearningProgressController();
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
                Assert.IsNotNull(json.name);
                Assert.IsNotNull(json.regis_result_id);
            }
        }

        [TestMethod()]
        public void Get_Learning_Progress_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new LearningProgressController();
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
                Assert.IsNotNull(json.name,
                    "JSON record does not contain \"name\" required property.");
                Assert.IsNotNull(json.regis_result_id,
                    "JSON record does not contain \"regis_result_id\" required property.");
            }
        }

        [TestMethod()]
        public void Learning_Progress_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new LearningProgressController();
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
        public void Learning_Progress_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new LearningProgressController();
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
            var controller = new LearningProgressController();
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
                Assert.IsNotNull(json.name,
                    "JSON record does not contain \"name\" required property.");
                Assert.IsNotNull(json.regis_result_id,
                    "JSON record does not contain \"regis_result_id\" required property.");
            }
        }

        [TestMethod()]
        public void Learning_Progress_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new LearningProgressController();
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