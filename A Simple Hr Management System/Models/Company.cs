using System.ComponentModel.DataAnnotations;

namespace A_Simple_Hr_Management_System.Models
{
    public class Company
    {
        [Key]
        public Guid ComId { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string ComName { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public decimal Basic { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public decimal Hrent { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public decimal Medical { get; set; }

        public bool IsInactive { get; set; } = false;
    }
}