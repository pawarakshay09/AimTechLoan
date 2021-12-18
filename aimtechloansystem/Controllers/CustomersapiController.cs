using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aimtechloansystem.Data;
using aimtechloansystem.Data.Migrations;
using aimtechloansystem.Utility;
using aimtechloansystem.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace aimtechloansystem.Controllers
{
    public class CustomersapiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersapiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult GetallCustomer()
        {
            var result = _context.Customer.ToList();
            return Json(new { sucess = true, message = "List of Customer", data = result });
        }

        public IActionResult GetCustomerRemain()
        {
            int cout = System.DateTime.Today.Month;
            var result =  _context.Customer.Where(m => m.NextPaid.Month.Equals(cout) && m.Status.Equals(SD.unpaid)).ToList();
            return Json(new { sucess = true, message = "List of Customer EMI Remain", data = result });

        }

        public IActionResult NewLoan()
        {
        //    Customer customer = new Customer();
        //    long intrate = customer.LoanAmount / 100;
        //    float data = customer.Months * customer.Percent;
        //    float totalintrate = intrate * data;
        //    float LaonBal = customer.LoanAmount + totalintrate;
        //    int months = customer.Months;
        //    customer.Fee = 100.00;// customer.LoanAmount / 100 * 2;
        //    customer.LoanBalance = LaonBal;
        //    customer.LastDate = customer.Date.AddMonths(months);
        //    customer.NextPaid = customer.Date.AddMonths(1);
        //    customer.Status = SD.unpaid;
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(customer);
        //        _context.SaveChanges();
        //        return Json(new { sucess = true, message = "Loan Add", data = "" });
        //    }
            return Json(new { sucess = true, message = "Loan Add", data = "" });
        }

        public IActionResult PaidCustomerList()
        {
            var result =  _context.Customer.Where(m => m.Status.Equals(SD.settel) || m.Status.Equals(SD.paid)).ToList();
            return Json(new { sucess = true, message = "List of Nill or Settel Customer ", data = result });
        }

        public IActionResult PersonalDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result =  _context.Customer.SingleOrDefault(m => m.ID == id);
            if (result == null)
            {
                return NotFound();
            }
            return Json(new { sucess = true, message = "Customer Details", data = result });

        }

        public IActionResult CustomerDetails(int? id)
        {
            var Personal = _context.Customer.Find(id);
           
            var result =  _context.EMI.Where(m => m.CustomerID.Equals(id)).ToList();
         
            return Json(new { sucess = true, message = "List of Customer", data = result,data2= Personal
 });
        }

        public IActionResult CustomerDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _context.Customer.SingleOrDefault(m => m.ID == id);
            if (result == null)
            {
                return NotFound();
            }
            _context.Remove(result);
            _context.SaveChanges();
            return Json(new { sucess = true, message = "Customer Delete", data = "" });
        }

        public IActionResult GetAddEMI(int? id)
        {
            if (id != null)
            {
                ViewData["CustomerName"] = _context.Customer.Find(id).CustomerName;
            }
            var data = _context.Customer.Find(id);

            long princepal = data.LoanAmount / data.Months;
            float intrate = (data.LoanAmount / 100) * data.Percent;
            float EMI = princepal + intrate;

            EMIVM emi = new EMIVM();
            emi.EMIAmount = EMI;
            emi.EMIDate = System.DateTime.Today;
            emi.Mudal = princepal;
            emi.Vyaj = intrate;
            emi.CustomerID = data.ID;

            TimeSpan ts = emi.EMIDate - data.NextPaid;
            float diff = ts.Days;

            int date = emi.EMIDate.Day;
            int month = emi.EMIDate.Month;
            int NextPaidmonth = data.NextPaid.Month;

            if (0 < diff)
            {
                //   int diff = date - 15;

                emi.LateFee = (intrate) + (diff * 100);
            }
            else
            {
                emi.LateFee = 0;
            }
            
            return Json(new { sucess = true, message = "Add EMI", data = emi });
        }

        public IActionResult AddEMIS(int? CustomerID, string EMIDate, float EMIAmount,float LateFee,float LoanBal,float Mudal,float Vyaj)
        {
            return Json(new { sucess = true, message = "Work in Process", data = "" });
        }

        public IActionResult EMIInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _context.EMI.SingleOrDefault(m => m.ID == id);
            if (result == null)
            {
                return NotFound();
            }
            return Json(new { sucess = true, message = "EMI" +
                " Details", data = result });
        }

        public IActionResult DeleteEMI(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _context.EMI.SingleOrDefault(m => m.ID == id);
            int Custid = result.CustomerID;
            var customers = _context.Customer.Find(Custid);
            
            if (result == null)
            {
                return NotFound();
            }
            _context.Remove(result);
            _context.SaveChanges();
            return Json(new { sucess = true, message = "EMI Delete", data = "" });
        }
    }
}