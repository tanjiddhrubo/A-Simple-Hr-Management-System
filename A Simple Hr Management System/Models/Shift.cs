using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Simple_Hr_Management_System.Models
{
    public class Shift
    {
        [Key]
        public Guid ShiftId { get; set; }

        [Required]
        [Display(Name = "Shift Name")]
        public string ShiftName { get; set; }

        [Required]
        [Display(Name = "In Time")]
        [DataType(DataType.Time)] // This helps the form display a time picker
        public TimeOnly In { get; set; }

        [Required]
        [Display(Name = "Out Time")]
        [DataType(DataType.Time)]
        public TimeOnly Out { get; set; }

        [Required]
        [Display(Name = "Late Time")]
        [DataType(DataType.Time)]
        public TimeOnly Late { get; set; }

        // --- Foreign Key to Company ---
        [Required]
        [Display(Name = "Company")]
        public Guid ComId { get; set; }

        [ForeignKey("ComId")]
        [ValidateNever]
        public Company Company { get; set; }
    }
}