using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers.Tests
{
    [TestClass()]
    public class MajorControllerTests
    {
        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new MajorController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Get_Major_Json_Data_Test()
        {
            // Arrange.
            var controller = new MajorController();

            // Act.
            var actionResult = controller.GetData();

            // Assert.
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            dynamic jsonCollection = actionResult.Data;
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.name,
                    "JSON record does not contain \"name\" required property.");
            }
        }

        [TestMethod()]
        public void Major_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new MajorController();

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
        public void Major_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new MajorController();

            // Act
            var actionResult = controller.GetData();
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void Major_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new MajorController();

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
                Assert.IsNotNull(json.name,
                    "JSON record does not contain \"name\" required property.");
            }
        }

        [TestMethod]
        public void Major_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new MajorController();
            var db = new SEP25Team03Entities();

            // Act
            var actionResult = controller.GetData();
            var model = db.majors.ToList();

            // Assert
            dynamic jsonCollection = actionResult.Data;

            Assert.AreEqual(model.Count, jsonCollection.Count);
        }

        [TestMethod()]
        public void Create_View_Test()
        {
            // Arrange
            var controller = new MajorController();

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create_Major_Test()
        {
            // Arrange
            var controller = new MajorController();
            SEP25Team03Entities db = new SEP25Team03Entities();
            major major = new major() { id = "7480104", name = "Hệ thống thông tin" };

            // Act
            controller.Create(major);
            var createResult = controller.GetData();
            dynamic jsonCollection = createResult.Data;

            // Assert
            Assert.AreEqual(db.majors.Count(), jsonCollection.Count);
        }

        [TestMethod]
        public void Create_Shoud_Be_Failed_When_Major_Id_Over_50_Characters_Test()
        {
            // Arrange
            var controller = new MajorController();
            major major = new major() { id = "usposueremisedaccumsanliguladiamatdsdasdsaddasasadd", name = "Test" };

            // Act
            var result = controller.Create(major) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.error);
        }

        [TestMethod]
        public void Create_Shoud_Be_Failed_When_Major_Name_Over_255_Characters_Test()
        {
            // Arrange
            var controller = new MajorController();
            major major = new major() { id = "7480104", name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris efficitur, massa non aliquet accumsan, ligula risus posuere mi, sed accumsan ligula diam at ante. Duis fermentum blandit ante, viverra convallis magna varius et. Sed congue ut elit vitae ufsa" };

            // Act
            var result = controller.Create(major) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.error);
        }

        [TestMethod]
        public void Create_Should_Be_Failed_When_Major_Exists_Test()
        {
            // Arrange
            var controller = new MajorController();
            major major = new major() { id = "7480104", name = "Hệ thống thông tin" };

            // Act
            var result = controller.Create(major) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert
            Assert.AreEqual(true, jsonCollection.error);
        }

        [TestMethod()]
        public void Edit_View_Test()
        {
            // Arrange
            var controller = new MajorController();

            // Act
            ViewResult result = controller.Edit("7480104") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Edit_Major_Data_Should_Load_Correctly_Test()
        {
            // Arrange
            var controller = new MajorController();
            major major = new major() { id = "7480104", name = "Hệ thống thông tin" };

            // Act
            controller.Create(major);
            var result = controller.Edit("7480104") as ViewResult;
            var model = result.Model as major;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(model.id, "7480104");
            Assert.AreEqual(model.name, "Hệ thống thông tin");
        }

        [TestMethod]
        public void Edit_Major_Test()
        {
            // Arrange...
            var controller = new MajorController();
            var major = new major();
            major.id = "7480104";
            major.name = "Hệ thống thông tin";

            // Act...
            var result = controller.Edit(major) as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert...
            Assert.AreEqual(true, jsonCollection.success);
        }

        [TestMethod]
        public void Delete_Major_Test()
        {
            // Arrange...
            var controller = new MajorController();

            // Act...
            var result = controller.Delete("7480104") as JsonResult;
            dynamic jsonCollection = result.Data;

            // Assert...
            Assert.AreEqual(true, jsonCollection.success);
        }
    }
}