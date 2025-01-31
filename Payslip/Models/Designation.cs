using System.ComponentModel.DataAnnotations;

namespace Pulse360Payslip.Models
{
    public class Designation
    {
		[Key]
		public int DesignationId { get; set; }

		public string Department { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string status { get; set; }	

        //public List<User> Users { get; set; }
    }
}
