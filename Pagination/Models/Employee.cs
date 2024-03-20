using System.ComponentModel.DataAnnotations;

namespace Pagination.Models
{
    public class Employee
    {
        public int Id { get; set; }
        //EmployeeName, EmployeeStatus, Salary, PayBasis, PositionTitle
        [Required, Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required, Display(Name = "Employee Status")]
        public string EmployeeStatus { get; set; }
        public double Salary { get; set; }
        public string PayBasis { get; set; }

        [Display(Name = "Position Title")]
        public string PositionTitle { get; set; }
    }
}
