using A_Simple_Hr_Management_System.Data;
using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index(int? year, int? month, Guid? deptId, DateOnly? dtFrom, DateOnly? dtTo, bool? isPaid) // <-- 1. Added isPaid
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            List<AttendanceSummary> summaries = new List<AttendanceSummary>();
            List<Salary> salaries = new List<Salary>();
            List<Employee> employees = new List<Employee>();
            List<Attendance> attendanceList = new List<Attendance>();

            // Set default filters
            int reportYear = year ?? DateTime.Now.Year;
            int reportMonth = month ?? DateTime.Now.Month;
            DateOnly fromDate = dtFrom ?? DateOnly.FromDateTime(new DateTime(reportYear, reportMonth, 1));
            DateOnly toDate = dtTo ?? DateOnly.FromDateTime(DateTime.Today);

            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                // ... (Employee, Attendance, and AttendanceSummary queries remain the same) ...

                // 2. Update Salary query to use the isPaid filter
                salaries = _unitOfWork.Salaries
                    .GetAll(s => s.ComId == companyId && s.dtYear == reportYear && s.dtMonth == reportMonth &&
                                 (!deptId.HasValue || s.Employee.DeptId == deptId.Value) &&
                                 (!isPaid.HasValue || s.IsPaid == isPaid.Value), // <-- Filter by payment status
                            includeProperties: "Employee")
                    .ToList();

                // ... (Get Employee list and Attendance list queries remain the same) ...
                employees = _unitOfWork.Employees
               .GetAll(e => e.ComId == companyId && (!deptId.HasValue || e.DeptId == deptId.Value),
                       includeProperties: "Department,Designation,Shift")
               .ToList();

                attendanceList = _unitOfWork.Attendances
                .GetAll(a => a.ComId == companyId &&
                             (!deptId.HasValue || a.Employee.DeptId == deptId.Value) &&
                             a.dtDate >= fromDate && a.dtDate <= toDate,
                        includeProperties: "Employee")
                .ToList();

                ViewBag.SelectedCompanyName = _unitOfWork.Companies.Get(c => c.ComId == companyId)?.ComName;
                ViewBag.DepartmentList = _unitOfWork.Departments.GetAll(d => d.ComId == companyId)
                                             .Select(i => new SelectListItem { Text = i.DeptName, Value = i.DeptId.ToString() });
            }

            ViewBag.SelectedYear = reportYear;
            ViewBag.SelectedMonth = reportMonth;
            ViewBag.SelectedDeptId = deptId;
            ViewBag.SelectedDtFrom = fromDate;
            ViewBag.SelectedDtTo = toDate;
            ViewBag.SelectedIsPaid = isPaid; // <-- 3. Pass filter value to view
            ViewBag.SalaryReport = salaries;
            ViewBag.EmployeeReport = employees;
            ViewBag.AttendanceReport = attendanceList;

            return View(summaries);
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
        // POST: /Report/MarkAsPaid
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAsPaid(Guid id)
        {
            var salaryRecord = _unitOfWork.Salaries.Get(s => s.Id == id);
            if (salaryRecord == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            salaryRecord.IsPaid = true;
            salaryRecord.PaidAmount = salaryRecord.PayableAmount; // Set PaidAmount
            _unitOfWork.Salaries.Update(salaryRecord);
            _unitOfWork.Save();

            return Json(new { success = true, paidAmount = salaryRecord.PaidAmount });
        }
    }
}