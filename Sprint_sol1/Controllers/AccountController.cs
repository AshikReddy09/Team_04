using Microsoft.AspNetCore.Mvc;
using Sprint_sol1.Models;

namespace Sprint_sol1.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                if (model.Password == "password" && model.UserName == "admin")
                {
                    return RedirectToAction("Index", "Home");
                }
                else {
                    ModelState.AddModelError("","Invalid username or password");
                }
            }
            return View(model);
        }
        public ActionResult Logout() {
            return RedirectToAction("Login");
        }
    }
}
