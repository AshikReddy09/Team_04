using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Sprint_sol1.Models
{
    public class LoginViewModel : Controller
    {
        [Required(ErrorMessage = "Please enter username")]
        public string UserName {  get; set; }
        [Required(ErrorMessage ="Please enter password")]
        public string Password { get; set; }
    }
}
