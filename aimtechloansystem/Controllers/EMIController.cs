using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aimtechloansystem.Data;
using aimtechloansystem.Models;
using aimtechloansystem.Utility;
using aimtechloansystem.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace aimtechloansystem.Controllers
{
    public class EMIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EMIController(ApplicationDbContext context)
        {
            int a=1;
            _context = context;
        }


        // GET: EMI
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EMI.Include(e => e.Customer);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EMI/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eMI = await _context.EMI.Include(e => e.Customer).SingleOrDefaultAsync(m => m.ID == id);
            if (eMI == null)
            {
                return NotFound();
            }

            return View(eMI);
        }

        public IActionResult EndOfmonths(int? months, int? Year)
        {

            var dta =  _context.EMI.Include(e => e.Customer);

            var data = dta.Where(m => m.EMIDate.Month.Equals(System.DateTime.Today.Month));
            if (months != null)
            {

                data = dta.Where(m => m.EMIDate.Month.Equals(months) && m.EMIDate.Year.Equals(Year));
                TempData["PrincipalTotal"] = data.Sum(m => m.Mudal);
                TempData["InterestTotal"] = data.Sum(m => m.Vyaj);
                TempData["EMITotal"] = data.Sum(m => m.EMIAmount);
                TempData["LateFee"] = data.Sum(m => m.LateFee);
            }
            else
            {

                data = dta.Where(m => m.EMIDate.Month.Equals(System.DateTime.Today.Month));
                TempData["PrincipalTotal"] = data.Sum(m => m.Mudal);
                TempData["InterestTotal"] = data.Sum(m => m.Vyaj);
                TempData["EMITotal"] = data.Sum(m => m.EMIAmount);
                TempData["LateFee"] = data.Sum(m => m.LateFee);
            }
            return View(data);
        }

        // GET: EMI/Create
        public IActionResult Create(int? id)
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
            return View(emi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EMIVM eMI, int? id)
        {
            Customer customer;

            if (id != null)
            {
                customer = _context.Customer.Find(id);
            }
            else
            {
                int Customerid = eMI.CustomerID;
                customer = _context.Customer.Find(Customerid);
            }
            eMI.LoanBal = customer.LoanBalance - eMI.EMIAmount;


            EMI emi = new EMI();
            emi.CustomerID = eMI.CustomerID;
            emi.EMIDate = eMI.EMIDate;
            emi.EMIAmount = eMI.EMIAmount;
            emi.LateFee = eMI.LateFee;
            emi.LoanBal = eMI.LoanBal;
            emi.Mudal = eMI.Mudal;
            emi.Vyaj = eMI.Vyaj;
            if (id != null)
            {
                customer.LoanBalance = eMI.LoanBal;
                customer.NextPaid = eMI.EMIDate.AddMonths(1);
                if (customer.LoanBalance == 0)
                { customer.Status = SD.paid; }
                else { customer.Status = SD.unpaid; }
            }
            else
            {
                customer.LoanBalance = 0;
                customer.Fee = Convert.ToInt64(eMI.LoanBal);
                customer.NextPaid = eMI.EMIDate;
                customer.Status = SD.settel;

            }
            if (ModelState.IsValid)
            {

                _context.Add(emi);
                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Customers");
            }
            return View(eMI);
        }

        public IActionResult Settel(int? id)
        {
            EMIVM emi = new EMIVM();
            if (id != null)
            {
                ViewData["CustomerName"] = _context.Customer.Find(id).CustomerName;
            }

            var Customer =  _context.Customer.Where(m => m.ID == id).FirstOrDefault();
            float data2 = _context.EMI.Where(m => m.CustomerID == id).Sum(m => m.Mudal);
            if (data2 != 0)
            {
                float mudal = Customer.LoanAmount - data2;
                float intrate = mudal / 100;
                float final = mudal + intrate;


                emi.EMIAmount = final;
                emi.EMIDate = System.DateTime.Today;
                emi.Mudal = mudal;
                emi.Vyaj = intrate;
                emi.CustomerID = Customer.ID;
                emi.LateFee = 0;
            }
            else
            {

                emi.EMIDate = System.DateTime.Today;
                emi.Mudal = Customer.LoanAmount;
                emi.Vyaj = Customer.LoanAmount / 100;
                emi.EMIAmount = Customer.LoanAmount;
                emi.CustomerID = Customer.ID;
                emi.LateFee = 0;
            }
            return View(emi);
        }


        // GET: EMI/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            DateTime d1 = System.DateTime.Today;
            var eMI = await _context.EMI.SingleOrDefaultAsync(m => m.ID == id);
            if (eMI == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = eMI.CustomerID;
            return View(eMI);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EMI eMI)
        {
            if (id != eMI.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime d1 = System.DateTime.Today;
                    _context.Update(eMI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EMIExists(eMI.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Customers");
            }
            ViewData["CustomerID"] = new SelectList(_context.Customer, "ID", "ID", eMI.Customer.CustomerName);
            return View(eMI);
        }

        // GET: EMI/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eMI = await _context.EMI
                .Include(e => e.Customer)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (eMI == null)
            {
                return NotFound();
            }

            return View(eMI);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eMI = await _context.EMI.SingleOrDefaultAsync(m => m.ID == id);

            int custid = eMI.CustomerID;

            var customer = _context.Customer.Find(custid);

            customer.LoanBalance = customer.LoanAmount + eMI.EMIAmount;

            _context.EMI.Remove(eMI);
            _context.Customer.Update(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Customers");
        }

        private bool EMIExists(int id)
        {
            return _context.EMI.Any(e => e.ID == id);
        }

    }
}
