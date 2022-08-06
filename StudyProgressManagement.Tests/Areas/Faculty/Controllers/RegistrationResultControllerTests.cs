using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers.Tests
{
    [TestClass()]
    public class RegistrationResultControllerTests
    {
        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Search_View_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            ViewResult result = controller.Search() as ViewResult;
            var major = new SelectList(db.majors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(major.Count(), ((IEnumerable<dynamic>)result.ViewBag.majors).Count());
        }

        [TestMethod()]
        public void Get_Search_Registration_Result_Json_Data_With_Class_Not_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Get_Search_Registration_Result_Json_Data_With_Class_Correctly_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Search_Registration_Result_Json_Data_With_Class_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Search_Registration_Result_Json_Data_With_Class_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Search_Registration_Result_JSon_Data_With_Class_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Search_Registration_Result_JSon_Data_With_Class_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Get_Search_Registration_Result_Json_Data_With_Name_Not_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Get_Search_Registration_Result_Json_Data_With_Name_Correctly_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Search_Registration_Result_Json_Data_With_Name_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Search_Registration_Result_Json_Data_With_Name_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Search_Registration_Result_JSon_Data_With_Name_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
        public void Search_Registration_Result_JSon_Data_With_Name_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
            var controller = new RegistrationResultController();
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
            var controller = new RegistrationResultController();

            // Act
            var studentId = "197pm212212";
            var actionResult = controller.GetStudentInfo(studentId);

            // Assert
            Assert.IsNull(actionResult);
            Assert.AreEqual(null, actionResult);
        }

        [TestMethod()]
        public void Get_Registration_Result_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_registrationResult = db.registration_results.FirstOrDefault();
            var actionResult = controller.GetData(query_registrationResult.student_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.curriculum_id);
                Assert.IsNotNull(json.curriculum_name);
                Assert.IsNotNull(json.credits);
                Assert.IsNotNull(json.registration_type);
                Assert.IsNotNull(json.registration_date);
                Assert.IsNotNull(json.curriculum_class_id);
                Assert.IsNotNull(json.lecturer_id);
                Assert.IsNotNull(json.lecturer_name);
                Assert.IsNotNull(json.term_id);
            }
        }

        [TestMethod()]
        public void Get_Registration_Result_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_registrationResult = db.registration_results.FirstOrDefault();
            var actionResult = controller.GetData(query_registrationResult.student_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.curriculum_id,
                    "JSON record does not contain \"curriculum_id\" required property.");
                Assert.IsNotNull(json.curriculum_name,
                    "JSON record does not contain \"curriculum_name\" required property.");
                Assert.IsNotNull(json.credits,
                    "JSON record does not contain \"credits\" required property.");
                Assert.IsNotNull(json.registration_type,
                    "JSON record does not contain \"registration_type\" required property.");
                Assert.IsNotNull(json.registration_date,
                    "JSON record does not contain \"registration_date\" required property.");
                Assert.IsNotNull(json.curriculum_class_id,
                    "JSON record does not contain \"curriculum_class_id\" required property.");
                Assert.IsNotNull(json.lecturer_id,
                    "JSON record does not contain \"lecturer_id\" required property.");
                Assert.IsNotNull(json.lecturer_name,
                    "JSON record does not contain \"lecturer_name\" required property.");
                Assert.IsNotNull(json.term_id,
                    "JSON record does not contain \"term_id\" required property.");
            }
        }

        [TestMethod()]
        public void Registration_Result_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_registrationResult = db.registration_results.FirstOrDefault();
            var actionResult = controller.GetData(query_registrationResult.student_id);
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
        public void Registration_Result_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_registrationResult = db.registration_results.FirstOrDefault();
            var actionResult = controller.GetData(query_registrationResult.student_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Registration_Result_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_registrationResult = db.registration_results.FirstOrDefault();
            var actionResult = controller.GetData(query_registrationResult.student_id);
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
                Assert.IsNotNull(json.credits,
                    "JSON record does not contain \"credits\" required property.");
                Assert.IsNotNull(json.registration_type,
                    "JSON record does not contain \"registration_type\" required property.");
                Assert.IsNotNull(json.registration_date,
                    "JSON record does not contain \"registration_date\" required property.");
                Assert.IsNotNull(json.curriculum_class_id,
                    "JSON record does not contain \"curriculum_class_id\" required property.");
                Assert.IsNotNull(json.lecturer_id,
                    "JSON record does not contain \"lecturer_id\" required property.");
                Assert.IsNotNull(json.lecturer_name,
                    "JSON record does not contain \"lecturer_name\" required property.");
                Assert.IsNotNull(json.term_id,
                    "JSON record does not contain \"term_id\" required property.");
            }
        }

        [TestMethod()]
        public void Registration_Result_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_registrationResult = db.registration_results.FirstOrDefault();
            var actionResult = controller.GetData(query_registrationResult.student_id);
            var query_registrationResult_context = db.registration_results.Where(s => s.student_id == query_registrationResult.student_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_registrationResult_context.Count());
        }

        [TestMethod()]
        public void Get_Student_Course_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
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
            var controller = new RegistrationResultController();
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
            var controller = new RegistrationResultController();
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
            var controller = new RegistrationResultController();
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
            var controller = new RegistrationResultController();
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
            var controller = new RegistrationResultController();
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
        public void Get_Class_Student_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadClassStudents(query_studentCourse.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id);
            }
        }

        [TestMethod()]
        public void Get_Class_Student_Json_Data_Correctly_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadClassStudents(query_studentCourse.id);
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
        public void Class_Student_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadClassStudents(query_studentCourse.id);
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
        public void Class_Student_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadClassStudents(query_studentCourse.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Class_Student_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var actionResult = controller.LoadClassStudents(query_studentCourse.id);
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
        public void Class_Student_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.FirstOrDefault();
            var query_classStudent = db.class_student.Where(s => s.student_course_id == query_studentCourse.id);
            var actionResult = controller.LoadClassStudents(query_studentCourse.id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_classStudent.Count());
        }

        [TestMethod()]
        public void Import_View_Test()
        {
            // Arrange
            var controller = new RegistrationResultController();
            var db = new SEP25Team03Entities();

            // Act
            ViewResult result = controller.Import() as ViewResult;
            var major = new SelectList(db.majors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(major.Count(), ((IEnumerable<dynamic>)result.ViewBag.majors).Count());
        }

        [TestMethod]
        public void Delete_Registration_Result_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.Where(s => s.major_id == "7480104").FirstOrDefault();
            var result = controller.Delete(query_studentCourse.id) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.success);
        }
    }
}