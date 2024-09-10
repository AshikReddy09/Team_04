using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sprint_sol1.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Contact_Sh()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the form data (e.g., save to database, send email, etc.)
                // For demonstration, we'll just return a simple success message.

                // Example: await _emailService.SendContactEmail(model);

                // Redirect to the same view with a success message
                TempData["Message"] = "Your message has been sent successfully!";
                return RedirectToAction("Contact_Sh");
            }

            // If the model state is not valid, return the same view with validation messages
            return View("Contact_Sh", model);
        }
    }

    public class ContactFormModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
