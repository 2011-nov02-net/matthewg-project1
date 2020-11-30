using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.Library.Interfaces;
using Project1.Library.Models;
using Project1.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Controllers {
    public class CustomerController : Controller {

        private readonly IStoreRepository _repository;
        
        public CustomerController(IStoreRepository repository) {
            _repository = repository;
        }

        // GET: CustomerController
        public ActionResult Index() {

            if (!TempData.ContainsKey("IsAdmin")) {
                return StatusCode(401);
            }

            var customers = _repository.GetCustomers();
            var viewModels = customers.Select(c => new CustomerViewModel {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            });
            return View(viewModels);
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id) {

            if (!TempData.ContainsKey("IsAdmin") && id != (int)TempData.Peek("CurrentCustomer")) {
                return StatusCode(401);
            }

            var customer = _repository.GetCustomerById(id);
            var viewModel = new CustomerViewModel {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };
            return View(viewModel);
        }

        // GET: CustomerController/Create
        public ActionResult Register() {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerViewModel viewModel) {
            if (!ModelState.IsValid) {
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

                    return RedirectToAction(nameof(Index), "Home");
                }

                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Error registering new customer.");
                return View(viewModel);
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id) {

            if (!TempData.ContainsKey("IsAdmin") && id != (int)TempData.Peek("CurrentCustomer")) {
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

            return View(viewModel);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            try {
                var customer = _repository.GetCustomerById(id);
                _repository.UpdateCustomer(customer, viewModel.FirstName, viewModel.LastName, viewModel.Email);
                _repository.Save();
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Unable to update customer details");
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

            return View(viewModel);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CustomerViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            try {
                var customer = _repository.GetCustomerById(id);
                _repository.RemoveCustomer(customer); // Not implemented in repository
                _repository.Save();
                return RedirectToAction(nameof(Index));
            } catch {
                return View(viewModel);
            }
        }
    }
}
