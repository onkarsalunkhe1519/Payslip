using System.ComponentModel.DataAnnotations;

namespace Payslip.Models
{
    public class EarningType
    {
        [Key]
        public int EarntypeId { get; set; }
        public string EarningName { get; set; }

        public List<Earning> Earnings { get; set; }
    }
}
