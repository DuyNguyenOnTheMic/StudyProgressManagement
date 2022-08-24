using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Collections.Generic;
using System.Data;
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
            var query_curriculum = db.curricula.FirstOrDefault();
            var actionResult = controller.GetData(query_curriculum.student_course_id);
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
            var query_curriculum = db.curricula.FirstOrDefault();
            var actionResult = controller.GetData(query_curriculum.student_course_id);
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

        [TestMethod()]
        public void Study_Program_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            var query_curriculum = db.curricula.FirstOrDefault();
            var actionResult = controller.GetData(query_curriculum.student_course_id);
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
        public void Study_Program_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            var query_curriculum = db.curricula.FirstOrDefault();
            var actionResult = controller.GetData(query_curriculum.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Study_Program_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            var query_curriculum = db.curricula.FirstOrDefault();
            var actionResult = controller.GetData(query_curriculum.student_course_id);
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
            }
        }

        [TestMethod()]
        public void Study_Program_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            var query_curriculum_first = db.curricula.FirstOrDefault();
            var query_curriculum = db.curricula.Where(s => s.student_course_id == query_curriculum_first.student_course_id);
            var actionResult = controller.GetData(query_curriculum_first.student_course_id);
            dynamic jsonCollection = actionResult.Data;

            // Assert
            Assert.AreEqual(jsonCollection.Count, query_curriculum.Count());
        }

        [TestMethod()]
        public void Get_Student_Course_Json_Data_Not_Null_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
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
            var controller = new StudyProgramController();
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
            var controller = new StudyProgramController();
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
            var controller = new StudyProgramController();
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
            var controller = new StudyProgramController();
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
            var controller = new StudyProgramController();
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
        public void Import_View_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            ViewResult result = controller.Import() as ViewResult;
            var major = new SelectList(db.majors);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(major.Count(), ((IEnumerable<dynamic>)result.ViewBag.majors).Count());
        }

        [TestMethod]
        public void Delete_Study_Program_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.Where(s => s.major_id == "7480104").FirstOrDefault();
            var result = controller.Delete(query_studentCourse.id) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.success);
        }

        [TestMethod]
        public void Delete_All_Study_Program_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var db = new SEP25Team03Entities();

            // Act
            var query_studentCourse = db.student_course.Where(s => s.major_id == "7480104").FirstOrDefault();
            var result = controller.DeleteAll(query_studentCourse.id) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.success);
        }

        [TestMethod()]
        public void Validate_Columns_Is_True_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[16]{
                        new DataColumn("Mã loại kiến thức"),
                        new DataColumn("Tên loại kiến thức"),
                        new DataColumn("Số chỉ BB"),
                        new DataColumn("Số chỉ TC"),
                        new DataColumn("Mã học phần"),
                        new DataColumn("Tên học phần (Tiếng Việt)"),
                        new DataColumn("Tên học phần (Tiếng Anh)"),
                        new DataColumn("TC"),
                        new DataColumn("LT"),
                        new DataColumn("TH"),
                        new DataColumn("TT"),
                        new DataColumn("DA"),
                        new DataColumn("Bắt buộc/ Tự chọn"),
                        new DataColumn("Điều kiện tiên quyết"),
                        new DataColumn("Học trước – học sau"),
                        new DataColumn("Ghi chú chỉnh sửa")
                });

            var result = controller.ValidateColumns(table);

            // Assert
            Assert.AreEqual(16, table.Columns.Count);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void Validate_Columns_Is_False_If_Missing_Columns_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[15]{
                        new DataColumn("Tên loại kiến thức"),
                        new DataColumn("Số chỉ BB"),
                        new DataColumn("Số chỉ TC"),
                        new DataColumn("Mã học phần"),
                        new DataColumn("Tên học phần (Tiếng Việt)"),
                        new DataColumn("Tên học phần (Tiếng Anh)"),
                        new DataColumn("TC"),
                        new DataColumn("LT"),
                        new DataColumn("TH"),
                        new DataColumn("TT"),
                        new DataColumn("DA"),
                        new DataColumn("Bắt buộc/ Tự chọn"),
                        new DataColumn("Điều kiện tiên quyết"),
                        new DataColumn("Học trước – học sau"),
                        new DataColumn("Ghi chú chỉnh sửa")
                });

            var result = controller.ValidateColumns(table);

            // Assert
            Assert.AreEqual(15, table.Columns.Count);
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void Contain_Column_Is_True_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[16]{
                        new DataColumn("Mã loại kiến thức"),
                        new DataColumn("Tên loại kiến thức"),
                        new DataColumn("Số chỉ BB"),
                        new DataColumn("Số chỉ TC"),
                        new DataColumn("Mã học phần"),
                        new DataColumn("Tên học phần (Tiếng Việt)"),
                        new DataColumn("Tên học phần (Tiếng Anh)"),
                        new DataColumn("TC"),
                        new DataColumn("LT"),
                        new DataColumn("TH"),
                        new DataColumn("TT"),
                        new DataColumn("DA"),
                        new DataColumn("Bắt buộc/ Tự chọn"),
                        new DataColumn("Điều kiện tiên quyết"),
                        new DataColumn("Học trước – học sau"),
                        new DataColumn("Ghi chú chỉnh sửa")
                });

            var result = controller.ContainColumn("Mã loại kiến thức", table);

            // Assert
            Assert.AreEqual(16, table.Columns.Count);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void Contain_Column_Is_False_Test()
        {
            // Arrange
            var controller = new StudyProgramController();
            var table = new DataTable();

            // Act
            table.Columns.AddRange(
                new DataColumn[16]{
                        new DataColumn("Mã loại kiến thức"),
                        new DataColumn("Tên loại kiến thức"),
                        new DataColumn("Số chỉ BB"),
                        new DataColumn("Số chỉ TC"),
                        new DataColumn("Mã học phần"),
                        new DataColumn("Tên học phần (Tiếng Việt)"),
                        new DataColumn("Tên học phần (Tiếng Anh)"),
                        new DataColumn("TC"),
                        new DataColumn("LT"),
                        new DataColumn("TH"),
                        new DataColumn("TT"),
                        new DataColumn("DA"),
                        new DataColumn("Bắt buộc/ Tự chọn"),
                        new DataColumn("Điều kiện tiên quyết"),
                        new DataColumn("Học trước – học sau"),
                        new DataColumn("Ghi chú chỉnh sửa")
                });

            var result = controller.ContainColumn("Test", table);

            // Assert
            Assert.AreEqual(16, table.Columns.Count);
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void To_Nullable_Int_Test()
        {
            // Arrange
            string test = "2020";

            // Act

            var result = StudyProgramController.ToNullableInt(test);

            // Assert
            Assert.AreEqual(2020, result);
        }

        [TestMethod()]
        public void To_Nullable_From_Empty_String_Is_True_Test()
        {
            // Arrange
            string test = "";

            // Act

            var result = StudyProgramController.ToNullableInt(test);

            // Assert
            Assert.AreEqual(null, result);
        }

        [TestMethod()]
        public void Set_Null_On_Empty_String_Test()
        {
            // Arrange
            string test = "Anh Văn 4";

            // Act

            var result = StudyProgramController.SetNullOnEmpty(test);

            // Assert
            Assert.AreEqual("Anh Văn 4", result);
        }

        [TestMethod()]
        public void Set_Null_On_Empty_Is_True_String_Test()
        {
            // Arrange
            string test = "";

            // Act

            var result = StudyProgramController.SetNullOnEmpty(test);

            // Assert
            Assert.AreEqual(null, result);
        }
    }
}