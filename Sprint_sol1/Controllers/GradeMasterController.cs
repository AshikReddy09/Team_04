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
    [Route("api/[controller]")]
    [ApiController]
    public class GradeMasterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradeMasterController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetGrade")]
        public async Task<IActionResult> GetGrade()
        {
            var users = await _context.GradeMasters.ToListAsync();
            if (users == null)
                return View();
            return View(users);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeMaster = await _context.GradeMasters
                .FirstOrDefaultAsync(m => m.Grade_Code == id);
            if (gradeMaster == null)
            {
                return NotFound();
            }

            return View(gradeMaster);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] GradeMaster gradeMaster)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gradeMaster);
                await _context.SaveChangesAsync();
                TempData["success"] = "Record created successfully";
                return RedirectToAction(nameof(GetGrade));
            }
            return View(gradeMaster);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeMaster = await _context.GradeMasters.FindAsync(id);
            if (gradeMaster == null)
            {
                return NotFound();
            }
            return View(gradeMaster);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Grade_Code,Description,Min_Salary,Max_Salary")] GradeMaster gradeMaster)
        {
            if (id != gradeMaster.Grade_Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradeMaster);
                    await _context.SaveChangesAsync();
                    TempData["warning"] = "Record updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeMasterExists(gradeMaster.Grade_Code))
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
            return View(gradeMaster);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeMaster = await _context.GradeMasters
                .FirstOrDefaultAsync(m => m.Grade_Code == id);
            if (gradeMaster == null)
            {
                return NotFound();
            }

            return View(gradeMaster);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var gradeMaster = await _context.GradeMasters.FindAsync(id);
            if (gradeMaster != null)
            {
                _context.GradeMasters.Remove(gradeMaster);
            }

            await _context.SaveChangesAsync();
            TempData["error"] = "Record deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool GradeMasterExists(string id)
        {
            return _context.GradeMasters.Any(e => e.Grade_Code == id);
        }
    }
}
