using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Simple_Hr_Management_System.Models
{
    public class AttendanceSummary
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int dtYear { get; set; }

        [Required]
        public int dtMonth { get; set; }

        [Required]
        public int Present { get; set; }

        [Required]
        public int Late { get; set; }

        [Required]
        public int Absent { get; set; }

        // --- Foreign Keys ---
        [Required]
        public Guid ComId { get; set; }

        [Required]
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