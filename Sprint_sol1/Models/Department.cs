using Sprint_sol1.Models;
using System.ComponentModel.DataAnnotations;

namespace Sprint_sol1.Models
{
    public class Department
    {
        [Key]
        [Required]
        public int Dept_ID { get; set; }
        [StringLength(50, ErrorMessage = "Department name cannot exceed 50 characters.")]
        public string Dept_Name { get; set; } = string.Empty;
       // public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
