using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.DataModel.Models;
using Project1.Library.Interfaces;
using Project1.Library.Models;
using Project1.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoreRepository _repository;

        public HomeController(ILogger<HomeController> logger, IStoreRepository repository) {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index() {
            var total = _repository.GetCustomers().Count();
            ViewData["TotalCustomers"] = total;
            return View();
        }

        public IActionResult Login() {
            if (TempData.ContainsKey("CurrentCustomer")) {
                return RedirectToAction(nameof(Index));
            }
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

                if (customer is Admin) {
                    TempData["IsAdmin"] = true;
                }

                TempData["CurrentCustomer"] = customer.Id;
                TempData["CustomerName"] = customer.FirstName;
                return RedirectToAction(nameof(Index));
            } catch (Exception) {
                ModelState.AddModelError("", "Invalid user login");
                return View(viewModel);
            }
        }

        public IActionResult Logout() {
            TempData.Clear();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
