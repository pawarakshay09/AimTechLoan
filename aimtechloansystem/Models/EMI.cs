using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aimtechloansystem.Models
{
    public class EMI
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "EMI Amount")]
        public float EMIAmount { get; set; }
        [Required]
        [Display(Name = "EMI Date")]
        public DateTime EMIDate { get; set; }
        [Required]
        [Display(Name = "Late Fee")]
        public float LateFee { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Display(Name = "Principal Amount")]
        public float Mudal { get; set; }
        [Display(Name = "Interest rate")]
        public float Vyaj { get; set; }
        [Display(Name = "Loan Balance")]
        public float LoanBal { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }
}
