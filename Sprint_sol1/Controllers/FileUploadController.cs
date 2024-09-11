using Sprint_sol1.Models;
using Sprint_sol1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprint_sol1.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IAzureStorage _azureStorage;

        public FileUploadController(IAzureStorage azureStorage)
        {
            _azureStorage = azureStorage;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.UploadResult = "Please select a file.";
                return View("Index");
            }

            var response = await _azureStorage.UploadAsync(file);

            if (response.Error)
            {
                ViewBag.UploadResult = $"Error: {response.Status}";
            }
            else
            {
                ViewBag.UploadResult = $"Success: {response.Status}";
            }

            return View("Index");
        }

        // GET: FileUpload/GetEmployees
        public async Task<IActionResult> GetEmployees()
        {
            var blobs = await _azureStorage.ListBlobsAsync();
            return View(blobs);
        }
    }
}
