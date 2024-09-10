using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Sprint_sol1.Models;

namespace Sprint_sol1.Models
{
    public class Employee
    {
        [Key]
        [ForeignKey("UserMaster")]
        public string Emp_ID { get; set; } = String.Empty;


        [Required(ErrorMessage = "First name is required.")]
        [StringLength(25, ErrorMessage = "First name cannot exceed 25 characters.")]
        public string Emp_First_Name { get; set; } = String.Empty;


        [StringLength(25, ErrorMessage = "Last name cannot exceed 25 characters.")]
        public string? Emp_Last_Name { get; set; } = String.Empty;


        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [CustomValidation(typeof(Employee), nameof(ValidateAge))]
        public DateTime Emp_Date_of_Birth { get; set; }


        [Required(ErrorMessage = "Date of Joining is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [CustomValidation(typeof(Employee), nameof(ValidateDateOfJoining))]
        public DateTime Emp_Date_of_Joining { get; set; }


        [ForeignKey("Department")]
        public int Emp_Dept_ID { get; set; }


        [Required(ErrorMessage = "Employee grade is required.")]
        [ForeignKey("GradeMaster")]
        public string Emp_Grade { get; set; } = String.Empty;


        [StringLength(50, ErrorMessage = "Designation cannot exceed 50 characters.")]
        public string? Emp_Designation { get; set; } = String.Empty;

        [Required(ErrorMessage = "Basic Salary is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Basic salary must be greated than 0.")]
        public int Emp_Basic { get; set; }


        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^[MF]$", ErrorMessage = "Gender must be either M or F.")]
        public string Emp_Gender { get; set; } = String.Empty;


        [Required(ErrorMessage = "Marital status is required.")]
        [RegularExpression("^[YN]$", ErrorMessage = "Gender must be either Y or N.")]
        public string Emp_Marital_Status { get; set; } = String.Empty;


        [StringLength(100, ErrorMessage = "Home Address cannot exceed 100 characters.")]
        public string? Emp_Home_Address { get; set; } = String.Empty;


        [StringLength(15, ErrorMessage = "Contact Number cannot exceed 15 characters.")]
        public string? Emp_Contact_Num { get; set; } = String.Empty;



        [ForeignKey("Emp_Dept_ID")]
        public virtual Department? Department { get; set; }

        [ForeignKey("Emp_ID")]
        public virtual UserMaster? UserMaster { get; set; }

        [ForeignKey("Emp_Grade")]
        public virtual GradeMaster? GradeMaster { get; set; }

       
        public static ValidationResult ValidateAge(DateTime dob, ValidationContext context)
        {
            var age = DateTime.Now.Year - dob.Year;
            if (dob > DateTime.Now.AddYears(-age)) age--;

            if (age < 18)
                return new ValidationResult("Employee must be atleast between 18 and 58 years old.");
            if (age > 58)
                return new ValidationResult("Employee cannot be older than 58 years.");

            return ValidationResult.Success;
        }
        public static ValidationResult ValidateDateOfJoining(DateTime dob, ValidationContext context)
        {
            var employee = (Employee)context.ObjectInstance;
            if (dob <= employee.Emp_Date_of_Birth)
                return new ValidationResult("Date of Joining must be later than Date of Birth.");
            return ValidationResult.Success;
        }

    }
}








