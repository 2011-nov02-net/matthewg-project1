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
    public class OrderController : Controller {

        private readonly IStoreRepository _repository;

        public OrderController(IStoreRepository repository) {
            _repository = repository;
        }

        // GET: OrderController
        public ActionResult Index() {
            IEnumerable<Order> orders;
            if (TempData.ContainsKey("IsAdmin")) {
                orders = _repository.GetOrders();
            } else {
                var customer = _repository.GetCustomerById((int)TempData.Peek("CurrentCustomer"));
                orders = _repository.GetCustomerOrders(customer);
            }

            var viewModels = orders.Select(o => new OrderViewModel {
                Id = o.Id,
                Time = o.Time,
                Location = o.Location,
                Customer = o.Customer,
                Products = o.Products,
                PricePaid = o.PricePaid
            });
            return View(viewModels);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id) {
            var order = _repository.GetOrderById(id);
            if (!TempData.ContainsKey("IsAdmin") && order.Customer.Id != (int)TempData.Peek("CurrentCustomer")) {
                return StatusCode(401);
            }

            decimal totalPrice = 0;
            foreach (var item in order.Products) {
                totalPrice += order.PricePaid[item.Key] * item.Value;
            }

            var viewModel = new OrderViewModel {
                Id = order.Id,
                Time = order.Time,
                Location = order.Location,
                Customer = order.Customer,
                Products = order.Products,
                PricePaid = order.PricePaid,
                TotalPrice = totalPrice
            };

            return View(viewModel);
        }

        // GET: ProductController/Create
        public ActionResult Create(int id) {
            var location = _repository.GetLocationById(id);
            var viewModel = new OrderViewModel() {
                Location = location
            };
            return View(viewModel);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            try {
                var customer = _repository.GetCustomerById((int)TempData.Peek("CurrentCustomer"));
                //var order = new Order(viewModel.Location, customer, DateTime.Now,);

                //_repository.AddOrder(order);
                //_repository.Save();

                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Error creating new product.");
                return View(viewModel);
            }
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id) {
            if (!TempData.ContainsKey("IsAdmin")) {
                return StatusCode(401);
            }

            var order = _repository.GetOrderById(id);
            var viewModel = new OrderViewModel {
                Id = order.Id,
                Time = order.Time,
                Location = order.Location,
                Customer = order.Customer,
                Products = order.Products,
                PricePaid = order.PricePaid
            };

            return View(viewModel);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            try {
                var order = _repository.GetOrderById(id);
                _repository.UpdateOrder(order); // Not implemented in repository
                _repository.Save();
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Unable to update order details");
                return View(viewModel);
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id) {
            if (!TempData.ContainsKey("IsAdmin")) {
                return StatusCode(401);
            }

            var order = _repository.GetOrderById(id);
            var viewModel = new OrderViewModel {
                Id = order.Id,
                Time = order.Time,
                Location = order.Location,
                Customer = order.Customer,
                Products = order.Products,
                PricePaid = order.PricePaid
            };

            return View(viewModel);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrderViewModel viewModel) {
            if (!ModelState.IsValid) {
                return View(viewModel);
            }
            try {
                var order = _repository.GetOrderById(id);
                _repository.RemoveOrder(order); // Not implemented in repository
                _repository.Save();
                return RedirectToAction(nameof(Index));
            } catch {
                return View(viewModel);
            }
        }
    }
}
