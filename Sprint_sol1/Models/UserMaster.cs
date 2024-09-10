using System.ComponentModel.DataAnnotations;

namespace Sprint_sol1.Models
{
    public class UserMaster
    {
        [Key]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]*$|^[1-9][a-zA-Z0-9]*$", ErrorMessage = "empID should not start with 0 and Should be length of minimum 6")]
        [StringLength(6, MinimumLength = 6)]
        public string UserID { get; set; } = string.Empty;

        [Required]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "UserName must be between 8 and 15 characters.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "User Password")]
        public string UserPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        [RegularExpression("^(SU|RE)$", ErrorMessage = "UserType must be 'SU' for Supervisor or 'RE' for Regular user.")]
        [Display(Name = "User Type")]
        public string UserType { get; set; } = string.Empty;
    }
}
