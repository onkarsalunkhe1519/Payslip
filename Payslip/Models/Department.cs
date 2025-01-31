using System.ComponentModel.DataAnnotations;

namespace Pulse360Payslip.Models
{
    public class Department
    {
		[Key]
		public int DepartmentId { get; set; }


		public string Name { get; set; }


		public string Status { get; set; }

   


    }
}
