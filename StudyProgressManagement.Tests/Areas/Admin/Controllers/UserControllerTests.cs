using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyProgressManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Admin.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        public void Index_Test()
        {
            // Arrange
            var controller = new UserController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Get_User_Json_Data_Test()
        {
            // Arrange.
            var controller = new UserController();

            // Act.
            var actionResult = controller.GetData();

            // Assert.
            Assert.IsNotNull(actionResult, "No ActionResult returned from action method.");
            dynamic jsonCollection = actionResult.Data;
            foreach (dynamic json in jsonCollection)
            {
                Assert.IsNotNull(json.id,
                    "JSON record does not contain \"id\" required property.");
                Assert.IsNotNull(json.email,
                    "JSON record does not contain \"email\" required property.");
                Assert.IsNotNull(json.role,
                    "JSON record does not contain \"role\" required property.");
            }
        }

        [TestMethod()]
        public void User_Json_Data_Should_Convert_To_IEnumerable_Test()
        {
            // Arrange
            var controller = new UserController();

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
        public void User_Json_Data_Index_at_0_Should_Not_Be_Null_Test()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var actionResult = controller.GetData();
            dynamic jsonCollection = actionResult.Data;

            // Assert                
            Assert.IsNotNull(jsonCollection[0]);
        }

        [TestMethod]
        public void User_JSon_Data_Should_Be_Indexable_Test()
        {
            // Arrange
            var controller = new UserController();

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
                Assert.IsNotNull(json.email,
                    "JSON record does not contain \"email\" required property.");
                Assert.IsNotNull(json.role,
                    "JSON record does not contain \"role\" required property.");
            }
        }

        [TestMethod]
        public void User_JSon_Data_Count_Should_Be_Equal_Test()
        {
            // Arrange
            var controller = new UserController();
            var db = new SEP25Team03Entities();

            // Act
            var actionResult = controller.GetData();
            var model = db.AspNetUsers.ToList();

            // Assert
            dynamic jsonCollection = actionResult.Data;

            Assert.AreEqual(model.Count, jsonCollection.Count);
        }
    }
}