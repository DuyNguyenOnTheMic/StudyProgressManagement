using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            MajorController controller = new MajorController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void Get_Major_Json_Data_Test()
        {
            // Arrange.
            MajorController controller = new MajorController();

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
    }
}