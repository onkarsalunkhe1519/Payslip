using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Payslip.Models;
using Pulse360Payslip.Data;

namespace Payslip.Controllers
{
    public class PayslipController : Controller
    {
        
        private readonly ApplicationDbContext db;

        public PayslipController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult AddEmployeeSalary()
        {
            var users = db.User.ToList();

            
            var usersWithFullName = users.Select(u => new
            {
                UserId = u.UserId,
                FullName = $"{u.FirstName} {u.LastName}" 
            }).ToList();

           
            ViewBag.Users = new SelectList(usersWithFullName, "UserId", "FullName");

            return View();
        }
        [HttpPost]
        public IActionResult AddEmployeeSalary(EmployeeSalaries e)
        {
            var employeeSalary = new EmployeeSalaries
            {
                UserId = e.UserId,
                BasicSalary = e.BasicSalary,
                NetSalary = e.BasicSalary, 
                CreatedDate = DateTime.Now
            };
            db.EmployeeSalaries.Add(employeeSalary);
            db.SaveChanges();
           
            return View();
        }
    }
}
