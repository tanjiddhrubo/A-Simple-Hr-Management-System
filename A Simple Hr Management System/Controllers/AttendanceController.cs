using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using A_Simple_Hr_Management_System.ViewModels; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace A_Simple_Hr_Management_System.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Attendance
        public IActionResult Index(DateOnly? date) // <-- 1. Accept a date
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            List<Attendance> attendances = new List<Attendance>();

            // 2. Default to today if no date is provided
            DateOnly attendanceDate = date ?? DateOnly.FromDateTime(DateTime.Today);
            ViewBag.SelectedDate = attendanceDate; // Pass the date to the view

            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                // 3. Filter by Company ID AND the selected date
                attendances = _unitOfWork.Attendances
                    .GetAll(a => a.ComId == companyId && a.dtDate == attendanceDate, includeProperties: "Employee")
                    .ToList();

                ViewBag.SelectedCompanyName = _unitOfWork.Companies.Get(c => c.ComId == companyId)?.ComName;
            }

            return View(attendances);
        }

        // This is the new "Bulk Operation" page
        // GET: /Attendance/BulkAttendance
        public IActionResult BulkAttendance(DateOnly? date)
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            if (!Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                return RedirectToAction(nameof(Index));
            }

            // Default to today if no date is provided
            DateOnly attendanceDate = date ?? DateOnly.FromDateTime(DateTime.Today);

            // Get all employees for this company
            var employees = _unitOfWork.Employees.GetAll(e => e.ComId == companyId, includeProperties: "Shift");

            // Get all attendance records that *already exist* for this day
            var existingRecords = _unitOfWork.Attendances
                .GetAll(a => a.ComId == companyId && a.dtDate == attendanceDate)
                .ToDictionary(a => a.EmpId);

            var viewModelList = new List<AttendanceVM>();

            foreach (var emp in employees)
            {
                var vm = new AttendanceVM
                {
                    EmpId = emp.EmpId,
                    EmpName = emp.EmpName,
                    ShiftName = emp.Shift?.ShiftName ?? "No Shift",
                    ShiftInTime = emp.Shift?.In,
                    dtDate = attendanceDate
                };

                // Check if a record already exists
                if (existingRecords.TryGetValue(emp.EmpId, out var existingRecord))
                {
                    vm.AttStatus = existingRecord.AttStatus;
                    vm.InTime = existingRecord.InTime;
                    vm.OutTime = existingRecord.OutTime;
                }

                viewModelList.Add(vm);
            }

            ViewBag.AttendanceDate = attendanceDate;
            return View(viewModelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveBulkAttendance(List<AttendanceVM> attendanceData)
        {
            if (attendanceData == null || !attendanceData.Any())
            {
                return RedirectToAction(nameof(BulkAttendance));
            }

            // Get the ComId from the first item (all are from the same company)
            var comId = _unitOfWork.Employees.Get(e => e.EmpId == attendanceData[0].EmpId)?.ComId;
            if (comId == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Get all existing records for this day in one database call
            var attendanceDate = attendanceData[0].dtDate;
            var existingRecords = _unitOfWork.Attendances
                .GetAll(a => a.ComId == comId && a.dtDate == attendanceDate)
                .ToDictionary(a => a.EmpId);

            foreach (var item in attendanceData)
            {
                // --- Business Logic ---
                if (item.AttStatus == "P") // If marked as "Present"
                {
                    if (item.InTime.HasValue && item.ShiftInTime.HasValue && item.InTime > item.ShiftInTime)
                    {
                        item.AttStatus = "L"; // Automatically set to "Late"
                    }
                }
                else if (item.AttStatus == "A") // If marked as "Absent"
                {
                    item.InTime = null; // Clear times if absent
                    item.OutTime = null;
                }

                // --- Upsert Logic (Update or Insert) ---
                if (existingRecords.TryGetValue(item.EmpId, out var existingRecord))
                {
                    // UPDATE existing record
                    existingRecord.AttStatus = item.AttStatus;
                    existingRecord.InTime = item.InTime;
                    existingRecord.OutTime = item.OutTime;
                    _unitOfWork.Attendances.Update(existingRecord);
                }
                else
                {
                    // INSERT new record
                    var newAttendance = new Attendance
                    {
                        Id = Guid.NewGuid(),
                        EmpId = item.EmpId,
                        ComId = (Guid)comId,
                        dtDate = item.dtDate,
                        AttStatus = item.AttStatus,
                        InTime = item.InTime,
                        OutTime = item.OutTime
                    };
                    _unitOfWork.Attendances.Add(newAttendance);
                }
            }

            _unitOfWork.Save(); // Save all changes at once

            // Redirect back to the same date
            return RedirectToAction(nameof(BulkAttendance), new { date = attendanceDate.ToString("yyyy-MM-dd") });
        }
    }
}