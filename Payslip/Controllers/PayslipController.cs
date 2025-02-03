using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Payslip.Models;
using Pulse360Payslip.Data;
using System.Net.Mail;
using System.Net;
using static System.Net.WebRequestMethods;

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
            ViewBag.Earnings = db.Earning.Include(e => e.EarningType).ToList();
            ViewBag.Deductions = db.Deduction.Include(d => d.DeductionType).ToList();

            return View();
        }
        [HttpPost]
        public IActionResult AddEmployeeSalary(EmployeeSalaries e, List<EmployeeEarnings> Earnings, List<EmployeeDeductions> Deductions)
        {
            ViewBag.Users = new SelectList(db.User.ToList(), "UserId", "FirstName");
            ViewBag.Earnings = db.Earning.Include(et => et.EarningType).ToList();
            ViewBag.Deductions = db.Deduction.Include(dt => dt.DeductionType).ToList();

            // Compute Total Earnings and Total Deductions
            decimal totalEarnings = Earnings.Sum(er => er.EarningAmount);
            decimal totalDeductions = Deductions.Sum(d => d.DeductionAmount);

            // Calculate Net Salary
            decimal netSalary = totalEarnings - totalDeductions;

            // Save Employee Salary Record
            var employeeSalary = new EmployeeSalaries
            {
                UserId = e.UserId,
                TotalSalary = e.TotalSalary,
                NetSalary = netSalary,
                CreatedDate = DateTime.Now
            };

            db.EmployeeSalaries.Add(employeeSalary);
            db.SaveChanges();

            // Save Earnings
            foreach (var earning in Earnings)
            {
                var employeeEarning = new EmployeeEarnings
                {
                    SalaryId = employeeSalary.SalaryId,
                    UserId = e.UserId,
                    EarningId = earning.EarningId,
                    EarningAmount = earning.EarningAmount
                };
                db.EmployeeEarnings.Add(employeeEarning);
            }

            // Save Deductions
            foreach (var deduction in Deductions)
            {
                var employeeDeduction = new EmployeeDeductions
                {
                    SalaryId = employeeSalary.SalaryId,
                    UserId = e.UserId,
                    DeductionId = deduction.DeductionId,
                    DeductionAmount = deduction.DeductionAmount
                };
                db.EmployeeDeductions.Add(employeeDeduction);
            }

            db.SaveChanges();
            return RedirectToAction("AddEmployeeSalary");
        }
        [Route("Payslip/GeneratePayslip/{userid}")]
        public IActionResult GeneratePayslip(int userid)

        {
            try
            {
                // **Fixed User ID for Testing**
                int userId = userid;

                // **Use February 2025**
                string month = DateTime.Now.ToString("MMMM");
                int year = DateTime.Now.Year;


                Console.WriteLine($"🔹 Generating payslip for User ID: {userId}, Month: {month}, Year: {year}");

                // **Fetch Organization Details**
                var organization = db.Organization.FirstOrDefault();
                if (organization == null)
                {
                    Console.WriteLine("❌ Organization details not found.");
                    return NotFound("Organization details not found.");
                }

                // **Fetch User Data**
                var user = db.User.Include(u => u.Designation).FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    Console.WriteLine("❌ User not found.");
                    return NotFound("User not found.");
                }

                // **Fetch Employee Salary**
                var employeeSalary = db.EmployeeSalaries.FirstOrDefault(s => s.UserId == userId);
                if (employeeSalary == null)
                {
                    Console.WriteLine("❌ Salary details not found.");
                    return NotFound("Salary details not found.");
                }

                // **Set Total Salary to $10,000 (for testing)**
                decimal totalSalary = employeeSalary.TotalSalary;

                // **Calculate total working hours for February 2025**
                int totalDaysInMonth = 28; // February 2025 has 28 days
                decimal totalWorkingHoursInMonth = totalDaysInMonth * 9m; // 9 working hours per day

                // **Calculate Hourly Rate (Rounded to 2 Decimal Places)**
                decimal hourlyRate = Math.Round(employeeSalary.TotalSalary / totalWorkingHoursInMonth, 2);

                // **Fetch Attendance Data for February 2025**
                var attendanceRecords = db.Attendance
                    .Where(a => a.UserId == userId && a.Date.Month == 2 && a.Date.Year == year)
                    .ToList();

                // **Calculate Total Hours Worked**
                decimal totalHoursWorked = attendanceRecords.Sum(a => a.WorkingHours);

                // **Fetch Earnings & Deductions**
                var earnings = db.Earning.Include(e => e.EarningType).Where(e => e.DepartmentId == user.DepartmentId).ToList();
                var deductions = db.Deduction.Include(d => d.DeductionType).Where(d => d.DepartmentId == user.DepartmentId).ToList();

                // **Calculate Total Earnings (Rounded to 2 Decimal Places)**
                decimal totalEarnings = Math.Round(earnings.Sum(e => (totalHoursWorked * e.EarningsPercentage * hourlyRate) / 100), 2);

                // **Calculate Total Deductions (Rounded to 2 Decimal Places)**
                decimal totalDeductions = Math.Round(deductions.Sum(d => (totalHoursWorked * d.DeductionPercentage * hourlyRate) / 100), 2);

                // **Calculate Net Salary (Rounded to 2 Decimal Places)**
                decimal netSalary = Math.Round(totalEarnings - totalDeductions, 2);

                Console.WriteLine($"✅ Total Working Hours: {totalHoursWorked}, Hourly Rate: {hourlyRate}");
                Console.WriteLine($"✅ Total Earnings: {totalEarnings}, Total Deductions: {totalDeductions}, Net Salary: {netSalary}");

                // **Pass Data to View using ViewBag**
                ViewBag.Organization = organization;
                ViewBag.User = user;
                ViewBag.Month = month;
                ViewBag.Year = year;
                ViewBag.TotalWorkingHours = totalHoursWorked;
                ViewBag.TotalHoursInMonth = totalWorkingHoursInMonth;
                ViewBag.HourlyRate = hourlyRate;
                ViewBag.TotalEarnings = totalEarnings;
                ViewBag.TotalDeductions = totalDeductions;
                ViewBag.NetSalary = netSalary;
                ViewBag.Earnings = earnings;
                ViewBag.Deductions = deductions;
                
                GeneratePayslipPDF(userid);
                return View("PayslipView");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error generating payslip: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
        public string GeneratePayslipPDF(int userId)
        {
            try
            {
                QuestPDF.Settings.License = LicenseType.Community;

                DateTime currentDate = DateTime.Now;
                string month = currentDate.ToString("MMMM");
                int year = currentDate.Year;

                // Fetch organization details
                var organization = db.Organization.FirstOrDefault();
                if (organization == null) throw new Exception("Organization details not found.");

                // Fetch user details
                var user = db.User.Include(u => u.Designation).FirstOrDefault(u => u.UserId == userId);
                if (user == null) throw new Exception("User not found.");

                // Fetch employee salary details
                var employeeSalary = db.EmployeeSalaries.FirstOrDefault(s => s.UserId == userId);
                if (employeeSalary == null) throw new Exception("Salary details not found.");

                // Fetch earnings (ENSURE NO DUPLICATES)
                var earnings = db.EmployeeEarnings
                                 .Include(e => e.Earning)
                                 .Where(e => e.UserId == userId)
                                 .GroupBy(e => e.Earning.EarningType.EarningName)
                                 .Select(g => g.First())
                                 .ToList();

                // Fetch deductions (ENSURE NO DUPLICATES)
                var deductions = db.EmployeeDeductions
                                   .Include(d => d.Deduction)
                                   .Where(d => d.UserId == userId)
                                   .GroupBy(d => d.Deduction.DeductionType.DeductionsName)
                                   .Select(g => g.First())
                                   .ToList();

                // Calculate Total Earnings and Total Deductions
                decimal totalEarnings = earnings.Sum(e => e.EarningAmount);
                decimal totalDeductions = deductions.Sum(d => d.DeductionAmount);
                decimal netSalary = totalEarnings - totalDeductions;

                // Calculate Year-To-Date (YTD) Values (Multiply Monthly Earnings/Deductions by 12)
                decimal totalEarningsYTD = totalEarnings * 12;
                decimal totalDeductionsYTD = totalDeductions * 12;

                // Calculate Working Hours Dynamically
                decimal calculatedHours = earnings.Count > 0 ? earnings.Average(e => e.EarningAmount / 39.68m) : 0m; // Assuming rate is 39.68

                // File path setup
                string fileName = $"Payslip_{user.FirstName}_{month}_{year}.pdf";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "payslips", fileName);

                // Ensure directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Generate PDF using QuestPDF
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(20);
                        page.PageColor(Colors.White);

                        // Header Section
                        page.Header().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text(organization.OrganizationName).FontSize(18).Bold();
                                col.Item().Text(organization.OrganizationAddress).FontSize(10);
                            });

                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().Text("Earnings Statement").FontSize(16).Bold();
                                col.Item().Text($"Period Ending: {currentDate:MM/dd/yyyy}").FontSize(10);
                                col.Item().Text($"Pay Date: {currentDate.AddDays(7):MM/dd/yyyy}").FontSize(10);
                            });
                        });

                        page.Content().Column(col =>
                        {
                            col.Spacing(5);

                            // Employee Details
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Text($"Exemptions: Federal: 3, State: 2").FontSize(10);
                                row.RelativeItem().AlignRight().Text($"{user.FirstName} {user.LastName}").FontSize(12).Bold();
                            });

                            col.Item().Text(user.Address).FontSize(10);

                            // Earnings & Deductions Table
                            col.Item().Row(row =>
                            {
                                // Earnings Table
                                row.RelativeItem().Column(earningsCol =>
                                {
                                    earningsCol.Item().Text("Earnings").FontSize(12).Bold().Underline();
                                    earningsCol.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("Earnings").Bold();
                                            header.Cell().Text("Rate").Bold();
                                            header.Cell().Text("Hours").Bold();
                                            header.Cell().Text("This Period").Bold();
                                            header.Cell().Text("Year to Date").Bold();
                                        });

                                        foreach (var earning in earnings)
                                        {
                                            decimal earningHours = earning.EarningAmount / 39.68m;
                                            table.Cell().Text(earning.Earning.EarningType.EarningName);
                                            table.Cell().Text("$39.68"); // Assuming fixed rate
                                            table.Cell().Text($"{earningHours:F2}");
                                            table.Cell().Text($"${earning.EarningAmount:F1}");
                                            table.Cell().Text($"${(earning.EarningAmount * 12):F1}"); // YTD Calculation
                                        }
                                    });

                                    earningsCol.Item().Text($"Gross Pay: ${totalEarnings:F2}").FontSize(12).Bold();

                                });

                                // Deductions Table
                                row.RelativeItem().Column(deductionsCol =>
                                {
                                    deductionsCol.Item().Text("Deductions").FontSize(12).Bold().Underline();
                                    deductionsCol.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Header(header =>
                                        {
                                            header.Cell().Text("Deductions").Bold();
                                            header.Cell().Text("This Period").Bold();
                                            header.Cell().Text("Year to Date").Bold();
                                        });

                                        foreach (var deduction in deductions)
                                        {
                                            if (deduction.Deduction != null && deduction.Deduction.DeductionType != null)
                                            {
                                                table.Cell().Text(deduction.Deduction.DeductionType.DeductionsName);
                                                table.Cell().Text($"${deduction.DeductionAmount:F1}");
                                                table.Cell().Text($"${(deduction.DeductionAmount * 12):F1}"); // YTD Calculation
                                            }
                                            else
                                            {
                                                // Handle the case where Deduction or DeductionType is null
                                                table.Cell().Text("No Deduction Name");
                                                table.Cell().Text("$0.00");
                                                table.Cell().Text("$0.00");
                                            }
                                        }

                                    });

                                    deductionsCol.Item().Text($"Total Deductions: ${totalDeductions:F2}").FontSize(12).Bold();
                                    deductionsCol.Item().Text($"Total Deductions YTD: ${totalDeductionsYTD:F2}").FontSize(12).Bold();
                                });
                            });

                            // Net Pay Section
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().Text("Net Pay:").FontSize(14).Bold();
                                row.RelativeItem().AlignRight().Text($"${netSalary:F2}").FontSize(14).Bold();
                            });

                            // Important Notes
                            col.Item().BorderBottom(1).PaddingBottom(5);
                            col.Item().Text("Important Notes:").FontSize(12).Bold();
                            col.Item().Text("Your hourly rate has been adjusted based on total working hours.");
                            col.Item().Text("We encourage participation in our upcoming United Way fund drive.");
                        });

                        // Footer
                        page.Footer().Text($"Generated on {DateTime.Now}").FontSize(10).AlignRight();
                    });
                }).GeneratePdf(filePath);

                // Save Payslip Record
                var payslip = new Payslips
                {
                    UserId = userId,
                    Month = month,
                    Year = year,
                    PayslipPath = filePath,
                    GeneratedOn = DateTime.Now
                };
                db.Payslips.Add(payslip);
                db.SaveChanges();

                // Send Payslip Email
                SendPayslipEmail(user.Email, filePath);

                Console.WriteLine($"✅ Payslip saved successfully: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error generating payslip: {ex.Message}");
                return null;
            }
        }
        public void SendPayslipEmail(string userEmail, string filePath)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string month = currentDate.ToString("MMMM");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("sakshimsawant162@gmail.com");
                mail.To.Add(userEmail);
                mail.Subject = "Your Payslip for the Month "+month;
                mail.Body = $"Please find your payslip attached for this month."+month;
                mail.Attachments.Add(new Attachment(filePath));
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("sakshimsawant162@gmail.com", "czscembogqficlkq");
                smtp.Send(mail);






                Console.WriteLine($"✅ Payslip sent to {userEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error sending payslip email: {ex.Message}");
            }
        }

    }
}
