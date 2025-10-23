using A_Simple_Hr_Management_System.Data;
using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace A_Simple_Hr_Management_System.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context; // We need direct DbContext access to call the function

        public ReportController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        // GET: /Report
        public IActionResult Index(int? year, int? month)
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            List<AttendanceSummary> summaries = new List<AttendanceSummary>();
            List<Salary> salaries = new List<Salary>();

            int reportYear = year ?? DateTime.Now.Year;
            int reportMonth = month ?? DateTime.Now.Month;

            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                // Get Attendance Summary data
                summaries = _unitOfWork.AttendanceSummaries
                    .GetAll(a => a.ComId == companyId && a.dtYear == reportYear && a.dtMonth == reportMonth,
                            includeProperties: "Employee")
                    .ToList();

                // Get Salary data
                salaries = _unitOfWork.Salaries
                    .GetAll(s => s.ComId == companyId && s.dtYear == reportYear && s.dtMonth == reportMonth,
                            includeProperties: "Employee")
                    .ToList();

                ViewBag.SelectedCompanyName = _unitOfWork.Companies.Get(c => c.ComId == companyId)?.ComName;
            }

            ViewBag.SelectedYear = reportYear;
            ViewBag.SelectedMonth = reportMonth;
            ViewBag.SalaryReport = salaries; // Pass the salary list to the view

            return View(summaries); // The main model is still the attendance summary
        }


        // POST: /Report/Generate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(int year, int month)
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                // This is how we call the PostgreSQL function
                await _context.Database.ExecuteSqlRawAsync(
                    "SELECT sp_summarize_attendance(@p_company_id, @p_year, @p_month)",
                    new Npgsql.NpgsqlParameter("p_company_id", companyId),
                    new Npgsql.NpgsqlParameter("p_year", year),
                    new Npgsql.NpgsqlParameter("p_month", month)
                );
            }

            // Redirect back to the Index page to show the new data
            return RedirectToAction(nameof(Index), new { year = year, month = month });
        }

        /// POST: /Report/GenerateSalary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateSalary(int year, int month)
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                // Call the new salary calculation function
                await _context.Database.ExecuteSqlRawAsync(
                    "SELECT sp_calculate_salary(@p_company_id, @p_year, @p_month)",
                    new Npgsql.NpgsqlParameter("p_company_id", companyId),
                    new Npgsql.NpgsqlParameter("p_year", year),
                    new Npgsql.NpgsqlParameter("p_month", month)
                );
            }

            // Redirect back to the Index page to show the reports
            return RedirectToAction(nameof(Index), new { year = year, month = month });
        }
    }
}