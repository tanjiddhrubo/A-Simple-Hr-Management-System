using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace A_Simple_Hr_Management_System.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Department
        public IActionResult Index()
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            IEnumerable<Department> departments = new List<Department>();

            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                departments = _unitOfWork.Departments.GetAll(d => d.ComId == companyId);
                ViewBag.SelectedCompanyName = _unitOfWork.Companies.Get(c => c.ComId == companyId)?.ComName;
            }

            return View(departments);
        }

        // GET: /Department/Create
        public IActionResult Create()
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            if (!Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                return RedirectToAction(nameof(Index));
            }

            var department = new Department { ComId = companyId };
            return PartialView("_Create", department);
        }

        // POST: /Department/CreateAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAjax(Department department)
        {
            if (ModelState.IsValid)
            {
                department.DeptId = Guid.NewGuid();
                _unitOfWork.Departments.Add(department);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Validation Error." });
        }

        // GET: /Department/Edit/5
        public IActionResult Edit(Guid id)
        {
            var department = _unitOfWork.Departments.Get(d => d.DeptId == id);
            if (department == null)
            {
                return NotFound();
            }
            return PartialView("_Edit", department);
        }

        // POST: /Department/EditAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAjax(Department department)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Departments.Update(department);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Validation Error" });
        }

        // POST: /Department/DeleteAjax/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAjax(Guid id)
        {
            var department = _unitOfWork.Departments.Get(d => d.DeptId == id);
            if (department == null)
            {
                return Json(new { success = false, message = "Error: Not Found" });
            }

            _unitOfWork.Departments.Remove(department);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}