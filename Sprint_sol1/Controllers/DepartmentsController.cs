using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sprint_sol1.Contracts;
using Sprint_sol1.Data;
using Sprint_sol1.Models;

namespace Sprint_sol1.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : Controller
    {
        private readonly IGenericRepository<Department, int> _repository;



        public DepartmentsController(IGenericRepository<Department, int> repository)
        {
            _repository = repository;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var users = await _repository.GetAllAsync();
            if (users == null)
                return View();
            return View(users);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _repository.GetByIdAsync(id);

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
            if (await _repository.ExistsAsync(department.Dept_ID))
            {
                TempData["ErrorMessage"] = "The specified Dept ID  exist.";
                return View(department);
            }


            if (ModelState.IsValid)
            {

                await _repository.AddAsync(department);
                TempData["success"] = "Record created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult<Department>> Edit(int id)
        {
            var dept = await _repository.GetByIdAsync(id);

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


            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.UpdateAsync(department);
                    TempData["warning"] = "Record updated successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(department.Dept_ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();

        }


        [HttpGet("Delete/{id}")]
        public async Task<ActionResult<Department>> Delete(int id)
        {
            var dept = await _repository.GetByIdAsync(id);

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
            var department = await _repository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            TempData["error"] = "Record deleted successfully";
            return RedirectToAction(nameof(Index));
        }


    }
}
