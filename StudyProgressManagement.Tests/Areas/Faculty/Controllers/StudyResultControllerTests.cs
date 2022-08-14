using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using StudyProgressManagement.Models;
using System.Collections.Generic;
using System.Data;
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
                Assert.IsNotNull(json.name);
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
                Assert.IsNotNull(json.name,
                    "JSON record does not contain \"name\" required property.");
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
                Assert.IsNotNull(json.name,
                    "JSON record does not contain \"name\" required property.");
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

        [TestMethod()]
        public void Get_Student_Course_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
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
            var controller = new StudyResultController();
            var db = new SEP25Team03Entities();

            // Act
            ViewResult result = controller.Import() as ViewResult;
            var major = new SelectList(db.majors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(major.Count(), ((IEnumerable<dynamic>)result.ViewBag.majors).Count());
        }

        [TestMethod]
        public void Delete_Study_Result_Test()
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

        [TestMethod()]
        public void Validate_Columns_Is_True_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[23]{
                        new DataColumn("StudentID"),
                        new DataColumn("StudentName"),
                        new DataColumn("BirthDay"),
                        new DataColumn("BirthPlace"),
                        new DataColumn("ClassStudentID"),
                        new DataColumn("ClassStudentName"),
                        new DataColumn("YearStudy"),
                        new DataColumn("TermID"),
                        new DataColumn("TermName"),
                        new DataColumn("CurriculumID"),
                        new DataColumn("StudyUnitID"),
                        new DataColumn("StudyUnitAlias"),
                        new DataColumn("CurriculumName"),
                        new DataColumn("Credits"),
                        new DataColumn("Mark10"),
                        new DataColumn("Mark10_2"),
                        new DataColumn("Mark10_3"),
                        new DataColumn("Mark10_4"),
                        new DataColumn("Mark10_5"),
                        new DataColumn("MaxMark10"),
                        new DataColumn("maxMark4"),
                        new DataColumn("MaxMarkLetter"),
                        new DataColumn("IsPass")
                });

            var result = controller.ValidateColumns(table);

            // Assert
            Assert.AreEqual(23, table.Columns.Count);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void Validate_Columns_Is_False_If_Missing_Columns_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[22]{
                        new DataColumn("StudentName"),
                        new DataColumn("BirthDay"),
                        new DataColumn("BirthPlace"),
                        new DataColumn("ClassStudentID"),
                        new DataColumn("ClassStudentName"),
                        new DataColumn("YearStudy"),
                        new DataColumn("TermID"),
                        new DataColumn("TermName"),
                        new DataColumn("CurriculumID"),
                        new DataColumn("StudyUnitID"),
                        new DataColumn("StudyUnitAlias"),
                        new DataColumn("CurriculumName"),
                        new DataColumn("Credits"),
                        new DataColumn("Mark10"),
                        new DataColumn("Mark10_2"),
                        new DataColumn("Mark10_3"),
                        new DataColumn("Mark10_4"),
                        new DataColumn("Mark10_5"),
                        new DataColumn("MaxMark10"),
                        new DataColumn("maxMark4"),
                        new DataColumn("MaxMarkLetter"),
                        new DataColumn("IsPass")
                });

            var result = controller.ValidateColumns(table);

            // Assert
            Assert.AreEqual(22, table.Columns.Count);
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void Contain_Column_Is_True_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[23]{
                        new DataColumn("StudentID"),
                        new DataColumn("StudentName"),
                        new DataColumn("BirthDay"),
                        new DataColumn("BirthPlace"),
                        new DataColumn("ClassStudentID"),
                        new DataColumn("ClassStudentName"),
                        new DataColumn("YearStudy"),
                        new DataColumn("TermID"),
                        new DataColumn("TermName"),
                        new DataColumn("CurriculumID"),
                        new DataColumn("StudyUnitID"),
                        new DataColumn("StudyUnitAlias"),
                        new DataColumn("CurriculumName"),
                        new DataColumn("Credits"),
                        new DataColumn("Mark10"),
                        new DataColumn("Mark10_2"),
                        new DataColumn("Mark10_3"),
                        new DataColumn("Mark10_4"),
                        new DataColumn("Mark10_5"),
                        new DataColumn("MaxMark10"),
                        new DataColumn("maxMark4"),
                        new DataColumn("MaxMarkLetter"),
                        new DataColumn("IsPass")
                });

            var result = controller.ContainColumn("StudentID", table);

            // Assert
            Assert.AreEqual(23, table.Columns.Count);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void Contain_Column_Is_False_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[23]{
                        new DataColumn("StudentID"),
                        new DataColumn("StudentName"),
                        new DataColumn("BirthDay"),
                        new DataColumn("BirthPlace"),
                        new DataColumn("ClassStudentID"),
                        new DataColumn("ClassStudentName"),
                        new DataColumn("YearStudy"),
                        new DataColumn("TermID"),
                        new DataColumn("TermName"),
                        new DataColumn("CurriculumID"),
                        new DataColumn("StudyUnitID"),
                        new DataColumn("StudyUnitAlias"),
                        new DataColumn("CurriculumName"),
                        new DataColumn("Credits"),
                        new DataColumn("Mark10"),
                        new DataColumn("Mark10_2"),
                        new DataColumn("Mark10_3"),
                        new DataColumn("Mark10_4"),
                        new DataColumn("Mark10_5"),
                        new DataColumn("MaxMark10"),
                        new DataColumn("maxMark4"),
                        new DataColumn("MaxMarkLetter"),
                        new DataColumn("IsPass")
                });

            var result = controller.ContainColumn("Test", table);

            // Assert
            Assert.AreEqual(23, table.Columns.Count);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Convert_Datatable_To_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new StudyResultController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[23]{
                        new DataColumn("StudentID"),
                        new DataColumn("StudentName"),
                        new DataColumn("BirthDay"),
                        new DataColumn("BirthPlace"),
                        new DataColumn("ClassStudentID"),
                        new DataColumn("ClassStudentName"),
                        new DataColumn("YearStudy"),
                        new DataColumn("TermID"),
                        new DataColumn("TermName"),
                        new DataColumn("CurriculumID"),
                        new DataColumn("StudyUnitID"),
                        new DataColumn("StudyUnitAlias"),
                        new DataColumn("CurriculumName"),
                        new DataColumn("Credits"),
                        new DataColumn("Mark10"),
                        new DataColumn("Mark10_2"),
                        new DataColumn("Mark10_3"),
                        new DataColumn("Mark10_4"),
                        new DataColumn("Mark10_5"),
                        new DataColumn("MaxMark10"),
                        new DataColumn("maxMark4"),
                        new DataColumn("MaxMarkLetter"),
                        new DataColumn("IsPass")
                });

            table.Rows.Add(
                    "197PM21905",
                    "Nguyễn Tân Duy",
                    "22/07/2001",
                    "TP.HCM",
                    "2",
                    "K25T-PM2",
                    "2022 - 2023",
                    "HK213",
                    "Học kỳ 3",
                    "21",
                    "studyUnitID_Test",
                    "studyUnitAlias_Test",
                    "Đồ Án Lập Trình Ứng Dụng phần mềm",
                    "4",
                    "9.5",
                    "",
                    "",
                    "",
                    "",
                    "9,5",
                    "3.6",
                    "A+",
                    "x"
                );
            string jsonString = JsonConvert.SerializeObject(table);


            var result = controller.DataTableToJson(table);
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.IsNotNull(result, "No ActionResult returned from action method.");
            Assert.IsNotNull(jsonString, jsonCollection);
            Assert.AreEqual("[{\"StudentID\":\"197PM21905\",\"StudentName\":\"Nguyễn Tân Duy\",\"BirthDay\":\"22/07/2001\",\"BirthPlace\":\"TP.HCM\",\"ClassStudentID\":\"2\",\"ClassStudentName\":\"K25T-PM2\",\"YearStudy\":\"2022 - 2023\",\"TermID\":\"HK213\",\"TermName\":\"Học kỳ 3\",\"CurriculumID\":\"21\",\"StudyUnitID\":\"studyUnitID_Test\",\"StudyUnitAlias\":\"studyUnitAlias_Test\",\"CurriculumName\":\"Đồ Án Lập Trình Ứng Dụng phần mềm\",\"Credits\":\"4\",\"Mark10\":\"9.5\",\"Mark10_2\":\"\",\"Mark10_3\":\"\",\"Mark10_4\":\"\",\"Mark10_5\":\"\",\"MaxMark10\":\"9,5\",\"maxMark4\":\"3.6\",\"MaxMarkLetter\":\"A+\",\"IsPass\":\"x\"}]", jsonCollection);
        }

        [TestMethod()]
        public void Set_Null_On_Empty_String_Test()
        {
            // Arrange
            string test = "Anh Văn 4";

            // Act
            var result = StudyResultController.SetNullOnEmpty(test);

            // Assert
            Assert.AreEqual("Anh Văn 4", result);
        }

        [TestMethod()]
        public void Set_Null_On_Empty_Is_True_String_Test()
        {
            // Arrange
            string test = "";

            // Act
            var result = StudyResultController.SetNullOnEmpty(test);

            // Assert
            Assert.AreEqual(null, result);
        }
    }
}