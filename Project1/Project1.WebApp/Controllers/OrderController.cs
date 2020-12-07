using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project1.Library.Interfaces;
using Project1.Library.Models;
using Project1.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project1.WebApp.Controllers {
    public class OrderController : Controller {

        private readonly IStoreRepository _repository;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IStoreRepository repository, ILogger<OrderController> logger) {
            _repository = repository;
            _logger = logger;
        }

        // GET: OrderController
        public ActionResult Index([FromQuery]int searchLocation = 0, [FromQuery]string searchCustomer = "") {
            IEnumerable<Order> orders;
            Location location;
            if (TempData.ContainsKey("IsAdmin")) {
                IEnumerable<Order> customerOrders;
                IEnumerable<Order> locationOrders;
                if (!string.IsNullOrWhiteSpace(searchCustomer)) {
                    var customer = _repository.GetCustomerByEmail(searchCustomer);
                    customerOrders = _repository.GetCustomerOrders(customer);
                } else {
                    customerOrders = _repository.GetOrders();
                }

                if (searchLocation != 0) {
                    location = _repository.GetLocationById(searchLocation);
                    locationOrders = _repository.GetLocationOrders(location);
                } else {
                    locationOrders = _repository.GetOrders();
                }

                orders = customerOrders.Join(locationOrders, co => co.Id, lo => lo.Id, (co, lo) => co);
                ViewData["Locations"] = _repository.GetLocations();
                ViewData["Location"] = searchLocation;
                ViewData["Customer"] = searchCustomer;
                _logger.LogInformation($"Visited Customer/Index with search params {nameof(searchLocation)}={searchLocation}, {nameof(searchCustomer)}={searchCustomer} at {DateTime.Now}");
            } else {
                var customer = _repository.GetCustomerById((int)TempData.Peek("CurrentCustomer"));
                orders = _repository.GetCustomerOrders(customer);
                _logger.LogInformation($"Order/Index visited at {DateTime.Now}");
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
                _logger.LogInformation($"User Access denied to Order/Details page at {DateTime.Now}");
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
            foreach (var product in viewModel.Products) {
                ViewData[product.Key.ToString()] = _repository.GetProductById(product.Key).DisplayName;
            }
            _logger.LogInformation($"Visited Order/Details with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // GET: ProductController/Create
        public ActionResult Create(int id) {
            Location location = _repository.GetLocationById(id);
            if (location is null) {
                return StatusCode(404);
            }
            var viewModel = new OrderViewModel() {
                Location = location
            };
            foreach (var product in location.Stock) {
                viewModel.Products[product.Key] = 0;
                ViewData[product.Key.ToString()] = _repository.GetProductById(product.Key).DisplayName;
            }
            _logger.LogInformation($"Visited Order/Create with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, OrderViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error creating new order, returning to creation page at {DateTime.Now}");
                return View(viewModel);
            }
            Location location;
            IUser customer;
            Order order;
            try {
                location = _repository.GetLocationById(id);
                customer = _repository.GetCustomerById((int)TempData.Peek("CurrentCustomer"));
                order = new Order(location, customer, DateTime.Now, viewModel.Products);
            } catch {
                return StatusCode(404);
            }

            try {
                foreach (var item in order.Products) {
                    if (item.Value < 0) {
                        ModelState.AddModelError("", "Cannot place an order for negative stock");
                        _logger.LogWarning($"Encountered error creating new order, returning to creation page at {DateTime.Now}");
                        viewModel.Location = location;
                        return View(viewModel);
                    }
                    if (item.Value > location.Stock[item.Key]) {
                        ModelState.AddModelError("", "Insufficient stock to supply requested order.");
                        _logger.LogWarning($"Encountered error creating new order, returning to creation page at {DateTime.Now}");
                        viewModel.Location = location;
                        return View(viewModel);
                    }
                }

                foreach (var item in order.Products) {
                    location.AddStock(item.Key, -item.Value);
                }

                _repository.AddOrder(order);
                _repository.UpdateLocationStock(location);
                _repository.Save();

                return RedirectToAction(nameof(Index));
            } catch (DbUpdateException) {
                foreach (var item in order.Products) {
                    location.AddStock(item.Key, item.Value);
                }
                ModelState.AddModelError("", "Error creating new order.");
                _logger.LogWarning($"Encountered error creating new order, returning to creation page at {DateTime.Now}");
                return View(viewModel);
            }
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id) {
            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Order/Edit page at {DateTime.Now}");
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
            _logger.LogInformation($"Visited Order/Edit with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error editing order, returning to edit page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var order = _repository.GetOrderById(id);
                _repository.UpdateOrder(order); // Not implemented in repository
                _repository.Save();
                _logger.LogInformation($"Edited details of order [{id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Unable to update order details");
                _logger.LogWarning($"Encountered error editing order, returning to edit page at {DateTime.Now}");
                return View(viewModel);
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id) {
            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Order/Delete page at {DateTime.Now}");
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
            _logger.LogInformation($"Visited Order/Delete with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, OrderViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error deleting order, returning to delete page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var order = _repository.GetOrderById(id);
                _repository.RemoveOrder(order); // Not implemented in repository
                _repository.Save();
                _logger.LogInformation($"Deleted order [{id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                _logger.LogWarning($"Encountered error deleting order, returning to delete page at {DateTime.Now}");
                return View(viewModel);
            }
        }
    }
}
