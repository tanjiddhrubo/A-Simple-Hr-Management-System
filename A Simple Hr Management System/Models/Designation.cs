using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace A_Simple_Hr_Management_System.Models
{
    public class Designation
    {
        [Key]
        public Guid DesigId { get; set; }

        [Required]
        [Display(Name = "Designation Name")]
        public string DesigName { get; set; }

        // --- Foreign Key to Company ---
        [Required]
        [Display(Name = "Company")]
        public Guid ComId { get; set; }

        [ForeignKey("ComId")]
        [ValidateNever]
        public Company Company { get; set; }
    }
}