using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_sol1.Data;
using Sprint_sol1.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sprint_sol1.Controllers
{
    public class AccEmpController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccEmpController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult LoginEmp()
        {
            // Check if there are any messages from TempData
            ViewBag.Message = TempData["Message"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginEmp(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await _context.UserMasters
                    .FirstOrDefaultAsync(e => e.UserID == model.UserName && e.UserPassword == model.Password);

                if (employee != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, employee.UserID),
                        new Claim(ClaimTypes.Role, "Employee")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    TempData["success"] = "Login successful!";
                    return RedirectToAction("Details", "Employee", new { id = employee.UserID });
                }
                else
                {
                    TempData["Message"] = "Incorrect username or password.";
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UserMaster(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Message"] = "Logged out successfully.";
            return RedirectToAction("LoginEmp");
        }
    }
}
