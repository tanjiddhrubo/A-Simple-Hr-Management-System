using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Simple_Hr_Management_System.Models
{
    public class Employee
    {
        [Key]
        public Guid EmpId { get; set; }

        [Required]
        [Display(Name = "Employee Code")]
        public string EmpCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Employee Name")]
        public string EmpName { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateOnly dtJoin { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Gross { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Basic { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal HRent { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Medical { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Others { get; set; } // <-- THE FIX IS HERE

        [Required]
        public Guid ComId { get; set; }

        [Required]
        [Display(Name = "Shift")]
        public Guid ShiftId { get; set; }

        [Required]
        [Display(Name = "Department")]
        public Guid DeptId { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public Guid DesigId { get; set; }

        [ForeignKey("ComId")]
        [ValidateNever]
        public Company? Company { get; set; }

        [ForeignKey("ShiftId")]
        [ValidateNever]
        public Shift? Shift { get; set; }

        [ForeignKey("DeptId")]
        [ValidateNever]
        public Department? Department { get; set; }

        [ForeignKey("DesigId")]
        [ValidateNever]
        public Designation? Designation { get; set; }
    }
}