using System.ComponentModel.DataAnnotations;

namespace Payslip.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }

        [Required]
        [StringLength(100)]
        public string OrganizationName { get; set; }

        [StringLength(500)]
        public string OrganizationDescription { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationPhone { get; set; }
        public string OrganizationEmail { get; set; }
        public string OrganizationLogo { get; set; }
    }
}
