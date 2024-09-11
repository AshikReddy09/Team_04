using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Route("api/[controller]")]
    [ApiController]
    public class GradeMasterController : Controller
    {
        private readonly IGenericRepository<GradeMaster, string> _repository;

        public GradeMasterController(IGenericRepository<GradeMaster, string> repository)
        {
            _repository = repository;
        }

        [HttpGet("GetGrade")]
        public async Task<ActionResult<IEnumerable<GradeMaster>>> GetGrade()
        {
            var users = await _repository.GetAllAsync();
            if (users == null)
                return View();
            return View(users);
        }

        [HttpGet("Details/{id}")]
        public async Task<ActionResult<IEnumerable<GradeMaster>>> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeMaster = await _repository.GetByIdAsync(id);
            if (gradeMaster == null)
            {
                return NotFound();
            }

            return View(gradeMaster);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<IEnumerable<GradeMaster>>> Create([FromForm] GradeMaster gradeMaster)
        {
            if (await _repository.ExistsAsync(gradeMaster.Grade_Code))
                {
                TempData["errorMessage"] = "Grade Code already exists";
                return View(gradeMaster);
            }
            var validationResult = gradeMaster.Validate();
            if (validationResult != ValidationResult.Success)
            {
                TempData["errorMessage"] = "Max salary must be greater than Min salary";
                return View(gradeMaster);
            }

            if (ModelState.IsValid)
            {
                await _repository.AddAsync(gradeMaster);
                TempData["success"] = "Record created successfully";
                return RedirectToAction(nameof(GetGrade));
            }
            return View(gradeMaster);
        }


        [HttpGet("Edit/{id}")]
        public async Task<ActionResult<GradeMaster>> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeMaster = await _repository.GetByIdAsync(id);

            if (gradeMaster == null)
            {
                return NotFound();
            }

            return View(gradeMaster);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GradeMaster>> Edit(string id, [FromForm] GradeMaster gradeMaster)
        {
            if (id != gradeMaster.Grade_Code)
            {
                return NotFound();
            }
            var validationResult = gradeMaster.Validate();
            if (validationResult != ValidationResult.Success)
            {
                TempData["errorMessage"] = "Max salary must be greater than Min salary";
                return View(gradeMaster);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.UpdateAsync(gradeMaster);
                    TempData["warning"] = "Record updated successfully";
                    return RedirectToAction(nameof(GetGrade));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(gradeMaster.Grade_Code))
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
        public async Task<ActionResult<GradeMaster>> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradeMaster = await _repository.GetByIdAsync(id);

            if (gradeMaster == null)
            {
                return NotFound();
            }

            return View(gradeMaster);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GradeMaster>> DeleteConfirmed(string id)
        {
            var gradeMaster = await _repository.GetByIdAsync(id);
            if (gradeMaster == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            TempData["error"] = "Record deleted successfully";
            return RedirectToAction(nameof(GetGrade));
        }
    }
}
