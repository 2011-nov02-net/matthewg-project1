using Microsoft.EntityFrameworkCore;
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
using System.Collections.Generic;
using System.Linq;

namespace Project1.UnitTests {
    public class CustomerControllerTests {

        [Fact]
        public void Index_Admin_WithLocations_DisplaysCustomers() {
            // arrange
            var mockRepo = new Mock<IStoreRepository>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object);

            mockRepo.Setup(r => r.GetCustomersByName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { new Customer("John", "Doe", "asdf@email.com"), new Customer("Jane", "Doe", "fdsa@gmail.com") });

            var controller = new CustomerController(mockRepo.Object, new NullLogger<CustomerController>()) { TempData = tempData };
            controller.TempData["IsAdmin"] = true;

            // act
            IActionResult actionResult = controller.Index();

            // assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var customers = Assert.IsAssignableFrom<IEnumerable<CustomerViewModel>>(viewResult.Model);
            var customerList = customers.ToList();
            Assert.Equal(2, customerList.Count);
            Assert.Equal("John", customerList[0].FirstName);
            Assert.Equal("fdsa@gmail.com", customerList[1].Email);
        }

        [Fact]
        public void Index_Customer_AccessDenied() {
            // arrange
            var mockRepo = new Mock<IStoreRepository>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object);
            var controller = new CustomerController(mockRepo.Object, new NullLogger<CustomerController>()) { TempData = tempData };

            // act
            IActionResult actionResult = controller.Index();

            // assert
            var viewResult = Assert.IsAssignableFrom<StatusCodeResult>(actionResult);
            Assert.Equal(401, viewResult.StatusCode);
        }

        [Fact]
        public void Details_Customer_DisplaysInfo() {
            // arrange
            var mockRepo = new Mock<IStoreRepository>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object);

            mockRepo.Setup(r => r.GetCustomerById(It.IsAny<int>()))
                .Returns(new Customer("John", "Doe", "asdf@email.com") { Id = 1 });

            var controller = new CustomerController(mockRepo.Object, new NullLogger<CustomerController>()) { TempData = tempData };
            controller.TempData["CurrentCustomer"] = 1;

            // act
            IActionResult actionResult = controller.Details(1);

            // assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            var customer = Assert.IsAssignableFrom<CustomerViewModel>(viewResult.Model);
            Assert.Equal("Doe", customer.LastName);
        }

        [Fact]
        public void Register_ValidInput_AddsNewCustomer() {
            // arrange
            var mockRepo = new Mock<IStoreRepository>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object);

            mockRepo.Setup(r => r.AddCustomer(It.IsAny<Customer>())).Verifiable();
            mockRepo.Setup(r => r.Save()).Verifiable();
            mockRepo.Setup(r => r.GetCustomerByEmail(It.IsAny<string>()))
                .Returns(new Customer("John", "Doe", "asdf@email.com") { Id = 1 });

            var controller = new CustomerController(mockRepo.Object, new NullLogger<CustomerController>()) { TempData = tempData };

            var viewModel = new CustomerViewModel() {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "asdf@gmail.com"
            };
            // act
            IActionResult actionResult = controller.Register(viewModel);

            // assert
            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(actionResult);
            Assert.True(controller.ViewData.ModelState.ErrorCount == 0);
            mockRepo.Verify(r => r.AddCustomer(It.IsAny<Customer>()), Times.Once);
            mockRepo.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void Register_InvalidInput_RejectsNewCustomer() {
            // arrange
            var mockRepo = new Mock<IStoreRepository>();
            var tempData = new TempDataDictionary(new DefaultHttpContext(), new Mock<ITempDataProvider>().Object);

            mockRepo.Setup(r => r.AddCustomer(It.IsAny<Customer>())).Verifiable();
            mockRepo.Setup(r => r.Save()).Throws(new DbUpdateException()).Verifiable();

            var controller = new CustomerController(mockRepo.Object, new NullLogger<CustomerController>()) { TempData = tempData };

            var viewModel = new CustomerViewModel() {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "asdf@gmail.com"
            };
            // act
            IActionResult actionResult = controller.Register(viewModel);

            // assert
            var viewResult = Assert.IsAssignableFrom<ViewResult>(actionResult);
            Assert.True(controller.ViewData.ModelState.ErrorCount == 1);
            mockRepo.Verify(r => r.AddCustomer(It.IsAny<Customer>()), Times.Once);
            mockRepo.Verify(r => r.Save(), Times.Once);
        }
    }
}
