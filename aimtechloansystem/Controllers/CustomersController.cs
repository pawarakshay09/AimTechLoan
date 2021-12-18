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
using Microsoft.AspNetCore.Authorization;

namespace aimtechloansystem.Controllers
{
   
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search = null)
        {
            try
            {
                if (search != null)
                {
                    var Data = await _context.Customer.Where(m => m.CustomerName.Contains(search) ).ToListAsync();
                    return View(Data);
                }
                else
                {
                    var Data = await _context.Customer.ToListAsync();
                    return View(Data);
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.ToString();
                return View();
            }
        }

        public IActionResult GetALL()
        {
            return Json(new {sucess=true,message="List of Customer", data = _context.Customer.ToList() });
        }

        public async Task<IActionResult> CustomerList(string search = null)
        {
            try
            {
                int cout = System.DateTime.Today.Month;

                if (search != null)
                {
                    var Data = await _context.Customer.Where(m => m.CustomerName.Contains(search) && m.NextPaid.Month.Equals(cout) && m.Status.Equals(SD.unpaid)).ToListAsync();
                    return View(Data);
                }
                else
                {
                    var data = await _context.Customer.Where(m => m.NextPaid.Month.Equals(cout) && m.Status.Equals(SD.unpaid)).ToListAsync();
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.ToString();
                return View();
            }
        }

        public async Task<IActionResult> PaidCustomerList(string search = null)
        {
            try
            {
                if (search != null)
                {
                    var Data = await _context.Customer.Where(m => m.CustomerName.Contains(search) && m.Status.Equals(SD.unpaid)).ToListAsync();
                    return View(Data);
                }
                else
                {
                    var data = await _context.Customer.Where(m => m.Status.Equals(SD.settel) || m.Status.Equals(SD.paid)).ToListAsync();
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.ToString();
                return View();
            }
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .SingleOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> EMIDetails(int? id)
        {
            var data = _context.Customer.Find(id);
            @TempData["id"] = id;
            @TempData["Name"] = data.CustomerName;
            @TempData["Address"] = data.Address;
            @TempData["NextPaid"] = data.NextPaid.ToString();
            @TempData["Date"] = data.Date;
            @TempData["LastDate"] = data.LastDate;
            @TempData["Fee"] = data.Fee;
            @TempData["Mobile"] = data.Mobile;
            @TempData["LoanAmount"] = data.LoanAmount;
            @TempData["Months"] = data.Months;
            @TempData["Percent"] = data.Percent;
            @TempData["LoanBal"] = data.LoanBalance;
            var data1 = await _context.EMI.Where(m => m.CustomerID.Equals(id)).ToListAsync();
            return View(data1);
        }



        // GET: Customers/Create
        public IActionResult Create()
        {
            Customer c = new Customer();
            c.Date = System.DateTime.Today;
            c.NextPaid = System.DateTime.Today;
            c.Months = 12;
            c.Percent = 1;
            c.LoanAmount = 100000;
            c.Fee = 2000;
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            long intrate = customer.LoanAmount / 100;
            float data = customer.Months * customer.Percent;
            float totalintrate = intrate * data;
            float LaonBal = customer.LoanAmount + totalintrate;
            int months = customer.Months;
            customer.Fee = customer.LoanAmount / 100 * 2;
            customer.LoanBalance = LaonBal;
            customer.LastDate = customer.Date.AddMonths(months);
            customer.NextPaid = customer.Date.AddMonths(1);
            customer.Status = SD.unpaid;
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .SingleOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.SingleOrDefaultAsync(m => m.ID == id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.ID == id);
        }

    }
}
