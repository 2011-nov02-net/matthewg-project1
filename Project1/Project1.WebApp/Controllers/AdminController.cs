using Microsoft.AspNetCore.Mvc;

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
