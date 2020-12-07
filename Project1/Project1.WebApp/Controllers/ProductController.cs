using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Library.Interfaces;
using Project1.Library.Models;
using Project1.WebApp.Models;
using System;
using System.Linq;

namespace Project1.WebApp.Controllers {
    public class ProductController : Controller {

        private readonly IStoreRepository _repository;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IStoreRepository repository, ILogger<ProductController> logger) {
            _repository = repository;
            _logger = logger;
        }

        // GET: ProductController
        public ActionResult Index() {
            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Product/Index page at {DateTime.Now}");
                return StatusCode(401);
            }

            var products = _repository.GetProducts();
            var viewModels = products.Select(p => new ProductViewModel {
                Id = p.Id,
                Name = p.DisplayName
            });
            _logger.LogInformation($"Visited Product/Index at {DateTime.Now}");
            return View(viewModels);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id) {
            var product = _repository.GetProductById(id);
            var viewModel = new ProductViewModel {
                Id = product.Id,
                Name = product.DisplayName
            };
            _logger.LogInformation($"Visited Product/Details with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // GET: ProductController/Create
        public ActionResult Create() {
            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Product/Create page at {DateTime.Now}");
                return StatusCode(401);
            }
            _logger.LogInformation($"Visited Product/Create at {DateTime.Now}");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error creating new product, returning to creation page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var product = new Product(viewModel.Name);

                _repository.AddProduct(product);
                _repository.Save();
                _logger.LogInformation($"Created new product, {product.DisplayName}, at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Error creating new product.");
                _logger.LogWarning($"Encountered error creating new product, returning to creation page at {DateTime.Now}");
                return View(viewModel);
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id) {
            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Product/Edit page at {DateTime.Now}");
                return StatusCode(401);
            }

            var product = _repository.GetProductById(id);
            var viewModel = new ProductViewModel {
                Id = product.Id,
                Name = product.DisplayName
            };
            _logger.LogInformation($"Visited Product/Edit with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error editing product, returning to edit page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var product = _repository.GetProductById(id);
                _repository.UpdateProduct(product); // Not implemented in repository
                _repository.Save();
                _logger.LogInformation($"Edited details of product [{id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                ModelState.AddModelError("", "Unable to update product details");
                _logger.LogWarning($"Encountered error editing product, returning to edit page at {DateTime.Now}");
                return View(viewModel);
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id) {
            if (!TempData.ContainsKey("IsAdmin")) {
                _logger.LogInformation($"User Access denied to Product/Delete page at {DateTime.Now}");
                return StatusCode(401);
            }

            var product = _repository.GetProductById(id);
            var viewModel = new ProductViewModel {
                Id = product.Id,
                Name = product.DisplayName
            };
            _logger.LogInformation($"Visited Location/Delete with params {nameof(id)}={id} at {DateTime.Now}");
            return View(viewModel);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ProductViewModel viewModel) {
            if (!ModelState.IsValid) {
                _logger.LogWarning($"Encountered error deleting product, returning to delete page at {DateTime.Now}");
                return View(viewModel);
            }
            try {
                var product = _repository.GetProductById(id);
                _repository.RemoveProduct(product); // Not implemented in repository
                _repository.Save();
                _logger.LogInformation($"Deleted product [{id}] at {DateTime.Now}");
                return RedirectToAction(nameof(Index));
            } catch {
                _logger.LogWarning($"Encountered error deleting product, returning to delete page at {DateTime.Now}");
                return View(viewModel);
            }
        }
    }
}
