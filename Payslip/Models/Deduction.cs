using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payslip.Models
{
    public class Deduction
    {
        [Key]
        public int DeductionId { get; set; } // Primary Key

        [ForeignKey("DeductionType")]
        public int DeductionTypeId { get; set; } // Foreign Key to DeductionType

        public int DepartmentId { get; set; } // Foreign Key to Department

        public int DesignationId { get; set; } // Foreign Key to Designation

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal DeductionPercentage { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; } // Foreign Key to User who created the record

        public DateTime? ModifiedAt { get; set; }

        public int? ModifiedBy { get; set; } // Foreign Key to User who modified the record

        // Navigation properties
        public virtual DeductionType DeductionType { get; set; }
    }
}
