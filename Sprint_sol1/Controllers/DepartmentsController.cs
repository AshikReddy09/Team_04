using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sprint_sol1.Data;
using Sprint_sol1.Models;

namespace Sprint_sol1.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Departments.ToListAsync();
            if (users == null)
                return View();
            return View(users);
        }
        [Authorize(Roles = "Employee")]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Dept_ID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                TempData["success"] = "Record created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet("Edit/{id}")]
        public async Task<ActionResult<Department>> Edit(int id)
        {
            var dept = await _context.Departments.FindAsync(id);

            if (dept == null)
            {
                return NotFound();
            }

            return View(dept);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Department department)
        {
            if (id != department.Dept_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                    TempData["warning"] = "Record updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Dept_ID))
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
            return View(department);
        }

        [HttpGet("Delete/{id}")]
        public async Task<ActionResult<Department>> Delete(int id)
        {
            var dept = await _context.Departments.FindAsync(id);

            if (dept == null)
            {
                return NotFound();
            }

            return View(dept);
        }

        [HttpPost("DeleteDept/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDept(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            TempData["error"] = "Record deleted successfully";

            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Dept_ID == id);
        }
    }
}
