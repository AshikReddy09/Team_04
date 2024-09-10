using System.ComponentModel.DataAnnotations;

namespace Sprint_sol1.Models
{
    public class GradeMaster
    {
        [Key]
        [StringLength(2)]
        [RegularExpression(@"^M[1-7]$", ErrorMessage = "Invalid grade. Valid grades are M1, M2, M3, M4, M5, M6, M7.")]
        [Required]
        public string Grade_Code { get; set; } = string.Empty;

        [StringLength(10)]
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Min_Salary { get; set; }
        [Required]
        public int Max_Salary { get; set; }
       // public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }

}
