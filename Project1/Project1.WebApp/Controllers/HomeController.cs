using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Library.Interfaces;
using Project1.Library.Models;
using Project1.WebApp.Models;
using System;
using System.Diagnostics;
using System.Linq;

// TODO:
//  documentation
//  unit tests

namespace Project1.WebApp.Controllers {
    public class HomeController : Controller {
        private readonly IStoreRepository _repository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IStoreRepository repository, ILogger<HomeController> logger) {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index() {
            var total = _repository.GetCustomers().Count();
            ViewData["TotalCustomers"] = total;
            _logger.LogInformation($"Home/Index visited at {DateTime.Now}");
            return View();
        }

        public IActionResult Login() {
            if (TempData.ContainsKey("CurrentCustomer")) {
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation($"Home/Login visited at {DateTime.Now}");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            try {
                var customer = _repository.GetCustomerByEmail(viewModel.Email);

                if (customer is null) {
                    ModelState.AddModelError("", $"No user exists with email address: {viewModel.Email}");
                    return View(viewModel);
                }
                if (customer is Admin) {
                    TempData["IsAdmin"] = true;
                }

                TempData["CurrentCustomer"] = customer.Id;
                TempData["CustomerName"] = customer.FirstName;
                _logger.LogInformation($"User [{customer.Id}] logged in at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch (Exception) {
                ModelState.AddModelError("", "Invalid user login");
                return View(viewModel);
            }
        }

        public IActionResult Logout() {
            _logger.LogInformation($"User [{(int)TempData.Peek("CurrentCustomer")}] logged out at {DateTime.Now}");
            TempData.Clear();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
