using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sprint_sol1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sprint_sol1.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Sprint_sol1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: Employees
        [HttpGet("GetEmp")]
        public async Task<IActionResult> GetEmp()
        {
            var users = await _context.Employees.ToListAsync();
            if (users == null)
                return View();
            return View(users);
        }
        //[Authorize(Roles = "Admin")]
        // GET: Employees/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Emp_ID == id);
           
            var user = User.FindFirst(ClaimTypes.Name)?.Value;
            if (User.IsInRole("Admin") || user == id)
            {
                return View(employee);
            }

            return RedirectToAction("AccessDenied", "Account");
        }
        [Authorize(Roles = "Admin")]
        // GET: Employees/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if UserID exists in UserMasters table
                    bool userExists = await _context.UserMasters.AnyAsync(u => u.UserID == employee.Emp_ID);
                    if (!userExists)
                    {
                        TempData["ErrorMessage"] = "The specified UserID does not exist.";
                        return View(employee);
                    }
                    bool DeptExists = await _context.Departments.AnyAsync(d => d.Dept_ID == employee.Emp_Dept_ID);
                    if (!DeptExists)
                    {
                        TempData["ErrorMessage"] = "The specified Dept ID does not exist.";
                        return View(employee);
                    }
                    bool GradeExists = await _context.GradeMasters.AnyAsync(d => d.Grade_Code == employee.Emp_Grade);
                    if (!GradeExists)
                    {
                        TempData["ErrorMessage"] = "The specified Grade Code does not exist.";
                        return View(employee);
                    }
                    // Proceed to add employee
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Record created successfully";
                    return RedirectToAction(nameof(GetEmp));
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error (optional)
                // _logger.LogError(ex, "An error occurred while saving the department.");

                // Set an error message to be displayed in the view
                TempData["ErrorMessage"] = "An error occurred while saving the department. The ID may already exist.";
            }
            // Add error message if model state is not valid
            //TempData["ErrorMessage"] = "There was a problem with the data you submitted.";
            return View(employee);
        }


        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Emp_ID,Emp_First_Name,Emp_Last_Name,Emp_Date_of_Birth,Emp_Date_of_Joining,Emp_Dept_ID,Emp_Grade,Emp_Designation,Emp_Basic,Emp_Gender,Emp_Marital_Status,Emp_Home_Address,Emp_Contact_Num")] Employee employee)
        {
            if (id != employee.Emp_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid )
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["warning"] = "Record updated successfully";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Emp_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public async Task<ActionResult<UserMaster>> Delete(string id)
        {
            var Empl = await _context.Employees.FindAsync(id);

            if (Empl == null)
            {
                return NotFound();
            }

            return View(Empl);
        }
        // POST: Employees/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteEmp/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmp(string id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            TempData["error"] = "Record deleted successfully";

            return RedirectToAction(nameof(GetEmp));
        }
        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.Emp_ID == id);
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Dept_ID == id);
        }


    }
}