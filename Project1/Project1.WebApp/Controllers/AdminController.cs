using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Controllers {
    public class AdminController : Controller {
        // GET: AdminController
        public ActionResult Index() {

            if (!TempData.ContainsKey("IsAdmin")) {
                return StatusCode(401);
            }

            return View();
        }
    }
}
