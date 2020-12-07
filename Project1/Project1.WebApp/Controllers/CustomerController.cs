using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Library.Interfaces;
using Project1.Library.Models;
using Project1.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project1.WebApp.Controllers {
    public class CustomerController : Controller {

        private readonly IStoreRepository _repository;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IStoreRepository repository, ILogger<CustomerController> logger) {
            _repository = repository;
            _logger = logger;
        }

        // GET: CustomerController
        public ActionResult Index([FromQuery]string searchFirstName = "", [FromQuery]string searchLastName = "") {

            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Customer/Index page at {DateTime.Now}");
                return StatusCode(401);
            }

            var customers = _repository.GetCustomersByName(searchFirstName, searchLastName);
            
            var viewModels = customers.Select(c => new CustomerViewModel {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            });

            ViewData["FirstName"] = searchFirstName;
            ViewData["LastName"] = searchLastName;
            _logger.LogInformation($"Visited Customer/Index with search params {nameof(searchFirstName)}={searchFirstName}, {nameof(searchLastName)}={searchLastName} at {DateTime.Now}");
            return View(viewModels);
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id) {

            if (!TempData.ContainsKey("IsAdmin") && id != (int)TempData.Peek("CurrentCustomer")) {
                _logger.LogInformation($"User Access denied to Customer/Details page at {DateTime.Now}");
                return StatusCode(401);
            }

            var customer = _repository.GetCustomerById(id);
            var viewModel = new CustomerViewModel {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };
            _logger.LogInformation($"Visited Customer/Details with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // GET: CustomerController/Create
        public ActionResult Register() {
            _logger.LogInformation($"Visited Customer/Register at {DateTime.Now}");
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error registering new customer, returning to registration page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var customer = new Customer(viewModel.FirstName, viewModel.LastName, viewModel.Email);

                _repository.AddCustomer(customer);
                _repository.Save();

                var dbCustomer = _repository.GetCustomerByEmail(customer.Email);

                if (!TempData.ContainsKey("CurrentCustomer")) {
                    TempData["CurrentCustomer"] = dbCustomer.Id;
                    TempData["CustomerName"] = dbCustomer.FirstName;
                    _logger.LogInformation($"Registered and logged in new customer [{dbCustomer.Id}] at {DateTime.Now}");
                    return RedirectToAction(nameof(Index), "Home");
                }
                _logger.LogInformation($"Registered new customer [{dbCustomer.Id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Error registering new customer.");
                _logger.LogWarning($"Encountered error registering new customer, returning to registration page at {DateTime.Now}");
                return View(viewModel);
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id) {

            if (!TempData.ContainsKey("IsAdmin") && id != (int)TempData.Peek("CurrentCustomer")) {
                _logger.LogInformation($"User Access denied to Customer/Edit page at {DateTime.Now}");
                return StatusCode(401);
            }

            var customer = _repository.GetCustomerById(id);
            var viewModel = new CustomerViewModel {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };
            ViewData["Editable"] = true;
            if (customer is Admin) {
                ViewData["Editable"] = false;
            }
            _logger.LogInformation($"Visited Customer/Edit with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error editing customer, returning to edit page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var customer = _repository.GetCustomerById(id);
                _repository.UpdateCustomer(customer, viewModel.FirstName, viewModel.LastName, viewModel.Email);
                _repository.Save();
                _logger.LogInformation($"Edited details of customer [{id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Unable to update customer details");
                _logger.LogWarning($"Encountered error editing customer, returning to edit page at {DateTime.Now}");
                return View(viewModel);
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id) {

            if (!TempData.ContainsKey("IsAdmin")) {
                return StatusCode(401);
            }

            var customer = _repository.GetCustomerById(id);
            var viewModel = new CustomerViewModel {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };
            ViewData["Editable"] = true;
            if (customer is Admin) {
                ViewData["Editable"] = false;
            }
            _logger.LogInformation($"Visited Customer/Delete with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CustomerViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error deleting customer, returning to delete page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var customer = _repository.GetCustomerById(id);
                _repository.RemoveCustomer(customer); // Not implemented in repository
                _repository.Save();
                _logger.LogInformation($"Deleted user [{id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                _logger.LogWarning($"Encountered error deleting customer, returning to delete page at {DateTime.Now}");
                return View(viewModel);
            }
        }
    }
}
