using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aimtechloansystem.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Required]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please Enter valid Number")]
        public long Mobile { get; set; }

        public string Address { get; set; }

        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Last Date")]
        public DateTime LastDate { get; set; }
        [Required]
        [Display(Name = "Loan Amonut")]
        public long LoanAmount { get; set; }
        [Required]

        public int Months { get; set; }
        [Display(Name = "Processing Fee")]
        public long Fee { get; set; }
        [Required]
        public float Percent { get; set; }
        [Display(Name = "Loan Balance")]
        public float LoanBalance { get; set; }
        [Display(Name = "Next Paid")]
        public DateTime NextPaid { get; set; }
        
        public string Status { get; set; }
    }
}
