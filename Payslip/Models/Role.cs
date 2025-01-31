using System.ComponentModel.DataAnnotations;

namespace Pulse360Payslip.Models
{
    public class Role
    {
		[Key]
		public int RoleId { get; set; }

		public string RoleName { get; set; }

		public string Status { get; set; }

        //public List<User> Users { get; set; }
    }
}
