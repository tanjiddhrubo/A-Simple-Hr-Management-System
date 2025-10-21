using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Simple_Hr_Management_System.Models
{
    public class Department
    {
        [Key]
        public Guid DeptId { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string DeptName { get; set; }

        // --- Foreign Key to Company ---
        [Required]
        [Display(Name = "Company")]
        public Guid ComId { get; set; }

        [ForeignKey("ComId")]
        [ValidateNever]
        public Company Company { get; set; }
    }
}