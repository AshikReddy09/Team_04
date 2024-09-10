using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sprint_sol1.Contracts;
using Sprint_sol1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprint_sol1.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserMasterController : Controller
    {
        private readonly IGenericRepository<UserMaster, string> _repository;

        public UserMasterController(IGenericRepository<UserMaster, string> repository)
        {
            _repository = repository;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<UserMaster>>> GetUsers()
        {
            var users = await _repository.GetAllAsync();
            if (users == null)
                return View();
            return View(users);

        }
        /*[HttpGet]   
        public IActionResult GetUsers()
        {
            return View();
        }*/

        [HttpGet("Details/{id}")]
        public async Task<ActionResult<UserMaster>> Details(string id)
        {
            var userMaster = await _repository.GetByIdAsync(id);

            if (userMaster == null)
            {
                return NotFound();
            }

            return View(userMaster);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<ActionResult<UserMaster>> Create([FromForm] UserMaster userMaster)
        {
            if (await _repository.ExistsAsync(userMaster.UserID))
                return Conflict("UserMaster with this ID already exists.");

            // Hash the password before storing it (consider using ASP.NET Core Identity for this)
            userMaster.UserPassword = HashPassword(userMaster.UserPassword);
            if (ModelState.IsValid)
            {

                await _repository.AddAsync(userMaster);
                TempData["success"] = "Record created successfully";
                return RedirectToAction(nameof(GetUsers));
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult<UserMaster>> Edit(string id)
        {
            var userMaster = await _repository.GetByIdAsync(id);

            if (userMaster == null)
            {
                return NotFound();
            }

            return View(userMaster);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromForm] UserMaster userMaster)
        {
            if (id != userMaster.UserID)
            {
                return BadRequest();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    await _repository.UpdateAsync(userMaster);
                    TempData["warning"] = "Record updated successfully";
                    return RedirectToAction(nameof(GetUsers));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(userMaster.UserID))
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

        [Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public async Task<ActionResult<UserMaster>> Delete(string id)
        {
            var userMaster = await _repository.GetByIdAsync(id);

            if (userMaster == null)
            {
                return NotFound();
            }

            return View(userMaster);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userMaster = await _repository.GetByIdAsync(id);
            if (userMaster == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            TempData["error"] = "Record deleted successfully";
            return RedirectToAction(nameof(GetUsers));
        }

        /* private bool UserMasterExists(string id)
         {
             return _context.UserMasters.Any(e => e.UserID == id);
         }*/

        private string HashPassword(string password)
        {
            // Implement password hashing logic here (or use a library)
            return password; // Replace with actual hash logic
        }
    }
}
