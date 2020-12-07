using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Library.Interfaces;
using Project1.Library.Models;
using Project1.WebApp.Models;
using System;
using System.Linq;

namespace Project1.WebApp.Controllers {
    public class LocationController : Controller {

        private readonly IStoreRepository _repository;
        private readonly ILogger<LocationController> _logger;

        public LocationController(IStoreRepository repository, ILogger<LocationController> logger) {
            _repository = repository;
            _logger = logger;
        }

        // GET: LocationController
        public ActionResult Index() {

            var locations = _repository.GetLocations();
            var viewModels = locations.Select(l => new LocationViewModel {
                Id = l.Id,
                Name = l.Name,
                Address = l.Address,
                City = l.City,
                State = l.State,
                Country = l.Country,
                Zip = l.Zip,
                Phone = l.Phone,
                Stock = l.Stock,
                Prices = l.Prices
            });
            _logger.LogInformation($"Visited Location/Index at {DateTime.Now}");
            return View(viewModels);
        }

        // GET: LocationController/Details/5
        public ActionResult Details(int id) {
            var location = _repository.GetLocationById(id);
            var viewModel = new LocationViewModel {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                City = location.City,
                State = location.State,
                Country = location.Country,
                Zip = location.Zip,
                Phone = location.Phone,
                Stock = location.Stock,
                Prices = location.Prices

            };
            foreach (var product in viewModel.Stock) {
                ViewData[product.Key.ToString()] = _repository.GetProductById(product.Key).DisplayName;
            }
            _logger.LogInformation($"Visited Customer/Details with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // GET: LocationController/Create
        public ActionResult Create() {
            if (!TempData.ContainsKey("IsAdmin")) {
                return StatusCode(401);
            }
            _logger.LogInformation($"Visited Location/Create at {DateTime.Now}");
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error creating new location, returning to creation page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var location = new Location(viewModel.Name, viewModel.Address, viewModel.City, viewModel.State, viewModel.Country, viewModel.Zip, viewModel.Phone);

                _repository.AddLocation(location);
                _repository.Save();
                _logger.LogInformation($"Created new location, {location.Name}, at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Error creating new location.");
                _logger.LogWarning($"Encountered error creating new location, returning to creation page at {DateTime.Now}");
                return View(viewModel);
            }
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(int id) {
            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Location/Edit page at {DateTime.Now}");
                return StatusCode(401);
            }

            var location = _repository.GetLocationById(id);
            var viewModel = new LocationViewModel {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                City = location.City,
                State = location.State,
                Country = location.Country,
                Zip = location.Zip,
                Phone = location.Phone,
                Stock = location.Stock,
                Prices = location.Prices
            };
            _logger.LogInformation($"Visited Location/Edit with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocationViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error editing location, returning to edit page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var location = _repository.GetLocationById(id);
                _repository.UpdateLocation(location); // Not implemented in repository
                _repository.Save();
                _logger.LogInformation($"Edited details of location [{id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Unable to update location details");
                _logger.LogWarning($"Encountered error editing location, returning to edit page at {DateTime.Now}");
                return View(viewModel);
            }
        }

        // GET: LocationController/Delete/5
        public ActionResult Delete(int id) {

            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Location/Delete page at {DateTime.Now}");
                return StatusCode(401);
            }

            var location = _repository.GetLocationById(id);
            var viewModel = new LocationViewModel {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                City = location.City,
                State = location.State,
                Country = location.Country,
                Zip = location.Zip,
                Phone = location.Phone,
                Stock = location.Stock,
                Prices = location.Prices
            };
            _logger.LogInformation($"Visited Location/Delete with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LocationViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error deleting location, returning to delete page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var location = _repository.GetLocationById(id);
                _repository.RemoveLocation(location); // Not implemented in repository
                _repository.Save();
                _logger.LogInformation($"Deleted location [{id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                _logger.LogWarning($"Encountered error deleting location, returning to delete page at {DateTime.Now}");
                return View(viewModel);
            }
        }
    }
}
