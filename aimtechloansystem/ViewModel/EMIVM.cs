using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aimtechloansystem.ViewModel
{
    public class EMIVM
    {
        public int ID { get; set; }

        [Display(Name = "EMI Amount")]
        public float EMIAmount { get; set; }
        [Display(Name = "EMI Date")]
        public DateTime EMIDate { get; set; }
        [Display(Name = "Late Fee")]
        public float LateFee { get; set; }

        public int CustomerID { get; set; }
        [Display(Name = "Loan Balance")]
        public float LoanBal { get; set; }
        [Display(Name = "Principal Amount")]
        public float Mudal { get; set; }
        [Display(Name = "Interest rate")]
        public float Vyaj { get; set; }
    }
}
