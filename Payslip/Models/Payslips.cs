using Pulse360Payslip.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Payslip.Models
{
    public class Payslips
    {
        [Key]
        public int PayslipId { get; set; }  // Primary Key

        [ForeignKey("User")]
        public int UserId { get; set; }  // Foreign Key to User Table
        public virtual User User { get; set; }

        [Required]
        public string Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string PayslipPath { get; set; }  // Path where PDF is stored

        [Required]
        public DateTime GeneratedOn { get; set; }  // Timestamp of Payslip Generation

    }
}
