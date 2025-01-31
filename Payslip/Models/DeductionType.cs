using System.ComponentModel.DataAnnotations;

namespace Payslip.Models
{
    public class DeductionType
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string DeductionsName { get; set; }

        // Navigation property
        public virtual ICollection<Deduction> Deductions { get; set; }
    }
}
