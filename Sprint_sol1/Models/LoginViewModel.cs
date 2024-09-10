using System.ComponentModel.DataAnnotations;

namespace Sprint_sol1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter username")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; } = string.Empty;

        // Parameterless constructor is not strictly needed as it is the default constructor.
        // You can remove it if you want to simplify your code.
        public LoginViewModel()
        {
        }
    }
}
    