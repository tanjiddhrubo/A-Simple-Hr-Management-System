using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace A_Simple_Hr_Management_System.Models
{
    public class Salary
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int dtYear { get; set; }

        [Required]
        public int dtMonth { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Gross { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Basic { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal HRent { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Medical { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AbsentAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PayableAmount { get; set; }

        public bool IsPaid { get; set; } = false;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PaidAmount { get; set; }

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