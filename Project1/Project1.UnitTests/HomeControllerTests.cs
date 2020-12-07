using Moq;
using Project1.Library.Models;
using Project1.Library.Interfaces;
using Xunit;
using Project1.WebApp.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Project1.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace Project1.UnitTests {
    public class HomeControllerTests {

        [Fact]
        public void Login_InvalidValue_Rejects() {
            // arrange
            var mockRepo = new Mock<IStoreRepository>();

            mockRepo.Setup(r => r.GetCustomerByEmail(It.IsAny<string>()))
                .Returns((IUser)null);

            var controller = new HomeController(mockRepo.Object, new NullLogger<HomeController>());
            var viewModel = new LoginViewModel() { Email = "asdf@email.com" };
            // act
            IActionResult actionResult = controller.Login(viewModel);

            // assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            Assert.True(controller.ViewData.ModelState.ErrorCount == 1);

        }

        [Fact]
        public void Login_ValidValue_Accepts() {
            // arrange
            var mockRepo = new Mock<IStoreRepository>();
            var mockTempData = new Mock<ITempDataDictionary>();

            mockRepo.Setup(r => r.GetCustomerByEmail(It.IsAny<string>()))
                .Returns(new Customer("John", "Doe", "asdf@email.com") { Id = 1 });

            var controller = new HomeController(mockRepo.Object, new NullLogger<HomeController>()) { TempData = mockTempData.Object };
            var viewModel = new LoginViewModel() { Email = "asdf@email.com" };
            // act
            IActionResult actionResult = controller.Login(viewModel);

            // assert
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            Assert.True(controller.ViewData.ModelState.ErrorCount == 0);

        }

        [Fact]
        public void Logout_ClearsTempData() {
            // Arrange
            var mockRepo = new Mock<IStoreRepository>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object);

            var controller = new HomeController(mockRepo.Object, new NullLogger<HomeController>()) { TempData = tempData };
            controller.TempData["CurrentCustomer"] = 1;

            // Act
            IActionResult actionResult = controller.Logout();

            // Assert
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            Assert.Empty(controller.TempData);
        }
    }
}
