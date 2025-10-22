using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Simple_Hr_Management_System.Models
{
    public class Attendance
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Attendance Date")]
        [DataType(DataType.Date)]
        public DateOnly dtDate { get; set; }

        [Display(Name = "In Time")]
        [DataType(DataType.Time)]
        public TimeOnly? InTime { get; set; } // Nullable if absent

        [Display(Name = "Out Time")]
        [DataType(DataType.Time)]
        public TimeOnly? OutTime { get; set; } // Nullable if absent

        [Required]
        [Display(Name = "Status")]
        public string AttStatus { get; set; } = "A"; // Default to 'Absent'. Can be 'P' (Present), 'A' (Absent), 'L' (Late)

        // --- Foreign Keys ---
        [Required]
        public Guid ComId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public Guid EmpId { get; set; }

        // --- Navigation Properties ---
        [ForeignKey("ComId")]
        [ValidateNever]
        public Company? Company { get; set; }

        [ForeignKey("EmpId")]
        [ValidateNever]
        public Employee? Employee { get; set; }
    }
}