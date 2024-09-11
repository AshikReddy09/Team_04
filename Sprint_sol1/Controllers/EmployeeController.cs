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
using System.Text;
using Sprint_sol1.Contracts;


namespace Sprint_sol1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IGenericRepository<Employee, string> _repository;
        private readonly IGenericRepository<Department, int> _departmentRepository;
        private readonly IGenericRepository<GradeMaster, string> _gradeRepository;
        private readonly IGenericRepository<UserMaster, string> _UserRepository;
        public EmployeeController(
            IGenericRepository<Employee, string> repository,
            IGenericRepository<Department, int> departmentRepository,
            IGenericRepository<GradeMaster, string> gradeRepository,
            IGenericRepository<UserMaster, string> userRepository)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
            _gradeRepository = gradeRepository;
            _UserRepository = userRepository;
        }


        // GET: Employees
        [HttpGet("GetEmp")]
        public async Task<IActionResult> GetEmp()
        {
            /* var users = await _repository.GetAllAsync();
             if (users == null || !users.Any())
             {
                 ViewBag.AdditionalString = "No employees found.";
                 return View();
             }

             // Fetch all departments
             var departmentIds = users.Select(e => e.Emp_Dept_ID).Distinct().ToList();
             var departments = await _departmentRepository.GetAllAsync();
             var departmentDictionary = departments.ToDictionary(d => d.Dept_ID, d => d.Dept_Name);

             // If there are department IDs, get the name of the first department
             var firstDepartmentId = departmentIds.FirstOrDefault();
             var departmentName = departmentDictionary.ContainsKey(firstDepartmentId)
                 ? departmentDictionary[firstDepartmentId]
                 : "Unknown";

             ViewBag.AdditionalString = $"Department: {departmentName}";
             ViewBag.Employees = users;

             return View();*/


            /*var users = await _repository.GetAllAsync();
            if (users == null)
                return View();
            return View(users);*/
            var employees = await _repository.GetAllAsync();
            if (employees == null || !employees.Any())
            {
                return View();
            }

            // Fetch all departments
            var departments = await _departmentRepository.GetAllAsync();
            var departmentDictionary = departments.ToDictionary(d => d.Dept_ID, d => d.Dept_Name);

            // Create a view model with department names included
            var employeeList = employees.Select(emp => new
            {
                emp.Emp_ID,
                emp.Emp_First_Name,
                emp.Emp_Last_Name,
                DepartmentName = departmentDictionary.TryGetValue(emp.Emp_Dept_ID, out var deptName) ? deptName : "Unknown",
                emp.Emp_Grade,
                emp.Emp_Gender,
                emp.Emp_Marital_Status
            }).ToList();

            // Pass the data to the view
            return View(employeeList);


        }

        // GET: Employees/Details/5
        [HttpGet("Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if related data exists
                    bool userExists = await _UserRepository.ExistsAsync(employee.Emp_ID);
                    if (!userExists)
                    {
                        TempData["ErrorMessage"] = "The specified UserID does not exist.";
                        return View(employee);
                    }
                    /*bool empExists = await _repository.ExistsAsync(employee.Emp_ID);
                    if (!empExists)
                    {
                        TempData["ErrorMessage"] = "The Employee already exist.";
                        return View(employee);
                    }*/

                    bool deptExists = await _departmentRepository.ExistsAsync(employee.Emp_Dept_ID);
                    if (!deptExists)
                    {
                        TempData["ErrorMessage"] = "The specified Dept ID does not exist.";
                        return View(employee);
                    }

                    
                    var grade = await _gradeRepository.GetByIdAsync(employee.Emp_Grade);
                    if (grade == null)
                    {
                        TempData["ErrorMessage"] = "The specified Grade Code does not exist.";
                        return View(employee);
                    }

                    // Validate salary range
                    if (employee.Emp_Basic < grade.Min_Salary || employee.Emp_Basic > grade.Max_Salary)
                    {
                        TempData["ErrorMessage"] = $"Salary must be between {grade.Min_Salary} and {grade.Max_Salary} for the grade {employee.Emp_Grade}.";
                        return View(employee);
                    }

                    // Proceed to add employee
                    await _repository.AddAsync(employee);
                    TempData["success"] = "Record created successfully";
                    return RedirectToAction(nameof(GetEmp));
                }
                catch (Exception ex)
                {
                    // Log the error (optional)
                    TempData["ErrorMessage"] = $"Employee Exists";
                }
            }

            return View(employee);
        }





        // GET: Employees/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromForm] Employee employee)
        {
            if (id != employee.Emp_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool deptExists = await _departmentRepository.ExistsAsync(employee.Emp_Dept_ID);
                    if (!deptExists)
                    {
                        TempData["ErrorMessage"] = "The specified Dept ID does not exist.";
                        return View(employee);
                    }


                    var grade = await _gradeRepository.GetByIdAsync(employee.Emp_Grade);
                    if (grade == null)
                    {
                        TempData["ErrorMessage"] = "The specified Grade Code does not exist.";
                        return View(employee);
                    }
                    if (employee.Emp_Gender != "M" && employee.Emp_Gender != "F")
                    {
                        TempData["ErrorMessage"] = "Gender must be 'M' or 'F'.";
                        return View(employee);
                    }

                    // Validate marital status
                    if (employee.Emp_Marital_Status != "Y" && employee.Emp_Marital_Status != "N")
                    {
                        TempData["ErrorMessage"] = "Marital Status must be 'Y' or 'N'.";
                        return View(employee);
                    }

                    // Validate salary range
                    if (employee.Emp_Basic < grade.Min_Salary || employee.Emp_Basic > grade.Max_Salary)
                    {
                        TempData["ErrorMessage"] = $"Salary must be between {grade.Min_Salary} and {grade.Max_Salary} for the grade {employee.Emp_Grade}.";
                        return View(employee);
                    }
                    
                    await _repository.UpdateAsync(employee);
                    TempData["warning"] = "Record updated successfully";
                    return RedirectToAction(nameof(GetEmp));

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repository.ExistsAsync(employee.Emp_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return NoContent();
        }

        // GET: Employees/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<ActionResult<UserMaster>> Delete(string id)
        {
            var Empl = await _repository.GetByIdAsync(id);

            if (Empl == null)
            {
                return NotFound();
            }

            return View(Empl);
        }

        // POST: Employees/Delete/5
        [HttpPost("DeleteEmp/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmp(string id)
        {
            var emp = await _repository.GetByIdAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            TempData["error"] = "Record deleted successfully";
            return RedirectToAction(nameof(GetEmp));
        }

        [HttpGet("GetTable")]
        public async Task<IActionResult> GetTable()
        {
            var users = await _repository.GetAllAsync();
            if (users == null)
                return View();
            return View(users);
        }

        [HttpGet("ExportToCsv")]
        public async Task<IActionResult> ExportToCsv()
        {
            var employees = await _repository.GetAllAsync();

            if (employees == null || !employees.Any())
            {
                TempData["error"] = "No records available to download.";
                return RedirectToAction(nameof(GetEmp));
            }

            // Fetch all departments to create a lookup dictionary
            var departments = await _departmentRepository.GetAllAsync();
            var departmentDictionary = departments.ToDictionary(d => d.Dept_ID, d => d.Dept_Name);

            var csv = new StringBuilder();

            // Add header line
            csv.AppendLine("Emp ID,First Name,Last Name,Department,Grade,Gender,Marital Status,Date of Birth,Date of Joining,Basic Salary,Designation,Home Address,Contact Number");

            foreach (var employee in employees)
            {
                // Use the department ID to get the department name
                var departmentName = departmentDictionary.ContainsKey(employee.Emp_Dept_ID)
                    ? departmentDictionary[employee.Emp_Dept_ID]
                    : "NA";

                // Create CSV line with "NA" for missing or null values
                var csvLine = $"{employee.Emp_ID ?? "NA"}," +
                              $"{employee.Emp_First_Name ?? "NA"}," +
                              $"{employee.Emp_Last_Name ?? "NA"}," +
                              $"{departmentName}," +
                              $"{employee.Emp_Grade ?? "NA"}," +
                              $"{employee.Emp_Gender ?? "NA"}," +
                              $"{employee.Emp_Marital_Status ?? "NA"}," +
                              $"{(employee.Emp_Date_of_Birth != default(DateTime) ? employee.Emp_Date_of_Birth.ToString("yyyy-MM-dd") : "NA")}," +
                              $"{(employee.Emp_Date_of_Joining != default(DateTime) ? employee.Emp_Date_of_Joining.ToString("yyyy-MM-dd") : "NA")}," +
                              $"{employee.Emp_Basic.ToString() ?? "NA"}," +
                              $"{employee.Emp_Designation ?? "NA"}," +
                              $"{employee.Emp_Home_Address ?? "NA"}," +
                              $"{employee.Emp_Contact_Num ?? "NA"}";

                csv.AppendLine(csvLine);
            }

            var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
            var stream = new MemoryStream(csvBytes);
            return File(stream, "text/csv", "employees.csv");
        }

    }
}