using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using A_Simple_Hr_Management_System.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace A_Simple_Hr_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            List<Employee> employees = new List<Employee>();

            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                // Use the new `includeProperties` to load related data
                employees = _unitOfWork.Employees.GetAll(e => e.ComId == companyId, includeProperties: "Designation,Department,Shift").ToList();
                ViewBag.SelectedCompanyName = _unitOfWork.Companies.Get(c => c.ComId == companyId)?.ComName;
            }

            return View(employees);
        }

        // GET: Employee/Create
        // GET: Returns the form as a partial view for the modal
        public IActionResult Create()
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            if (!Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                // This should not happen if called from the UI, but it's good practice
                return RedirectToAction(nameof(Index));
            }

            EmployeeVM employeeVM = new()
            {
                Employee = new Employee { ComId = companyId },
                DesignationList = _unitOfWork.Designations.GetAll(d => d.ComId == companyId).Select(i => new SelectListItem { Text = i.DesigName, Value = i.DesigId.ToString() }),
                DepartmentList = _unitOfWork.Departments.GetAll(d => d.ComId == companyId).Select(i => new SelectListItem { Text = i.DeptName, Value = i.DeptId.ToString() }),
                ShiftList = _unitOfWork.Shifts.GetAll(s => s.ComId == companyId).Select(i => new SelectListItem { Text = i.ShiftName, Value = i.ShiftId.ToString() })
            };

            return PartialView("_Create", employeeVM);
        }

        // POST: Handles the Ajax form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAjax(EmployeeVM employeeVM)
        {
            // We only need to validate the Employee part of the ViewModel
            if (ModelState.IsValid)
            {
                var company = _unitOfWork.Companies.Get(c => c.ComId == employeeVM.Employee.ComId);
                if (company != null)
                {
                    employeeVM.Employee.Basic = employeeVM.Employee.Gross * (company.Basic / 100);
                    employeeVM.Employee.HRent = employeeVM.Employee.Gross * (company.Hrent / 100);
                    employeeVM.Employee.Medical = employeeVM.Employee.Gross * (company.Medical / 100);
                    employeeVM.Employee.Others = employeeVM.Employee.Gross - (employeeVM.Employee.Basic + employeeVM.Employee.HRent + employeeVM.Employee.Medical);
                }

                employeeVM.Employee.EmpId = Guid.NewGuid();
                _unitOfWork.Employees.Add(employeeVM.Employee);
                _unitOfWork.Save();
                return Json(new { success = true });
            }

            // If validation fails, return an error
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors = errors });
        }

        // GET: Employee/Edit/5
        public IActionResult Edit(Guid id)
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            if (!Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                return RedirectToAction(nameof(Index));
            }

            EmployeeVM employeeVM = new()
            {
                Employee = _unitOfWork.Employees.Get(e => e.EmpId == id),
                DesignationList = _unitOfWork.Designations.GetAll(d => d.ComId == companyId).Select(i => new SelectListItem { Text = i.DesigName, Value = i.DesigId.ToString() }),
                DepartmentList = _unitOfWork.Departments.GetAll(d => d.ComId == companyId).Select(i => new SelectListItem { Text = i.DeptName, Value = i.DeptId.ToString() }),
                ShiftList = _unitOfWork.Shifts.GetAll(s => s.ComId == companyId).Select(i => new SelectListItem { Text = i.ShiftName, Value = i.ShiftId.ToString() })
            };

            if (employeeVM.Employee == null)
            {
                return NotFound();
            }
            return PartialView("_Edit", employeeVM);
        }

        // POST: Employee/EditAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAjax(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                // Re-calculate salary in case Gross was changed
                var company = _unitOfWork.Companies.Get(c => c.ComId == employeeVM.Employee.ComId);
                if (company != null)
                {
                    employeeVM.Employee.Basic = employeeVM.Employee.Gross * (company.Basic / 100);
                    employeeVM.Employee.HRent = employeeVM.Employee.Gross * (company.Hrent / 100);
                    employeeVM.Employee.Medical = employeeVM.Employee.Gross * (company.Medical / 100);
                    employeeVM.Employee.Others = employeeVM.Employee.Gross - (employeeVM.Employee.Basic + employeeVM.Employee.HRent + employeeVM.Employee.Medical);
                }

                _unitOfWork.Employees.Update(employeeVM.Employee);
                _unitOfWork.Save();
                return Json(new { success = true });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors = errors });
        }

        // POST: Employee/DeleteAjax/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAjax(Guid id)
        {
            var employee = _unitOfWork.Employees.Get(e => e.EmpId == id);
            if (employee == null)
            {
                return Json(new { success = false, message = "Error: Not Found" });
            }

            _unitOfWork.Employees.Remove(employee);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}