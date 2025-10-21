using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace A_Simple_Hr_Management_System.Controllers
{
    public class ShiftController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShiftController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Shift
        public IActionResult Index()
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            IEnumerable<Shift> shifts = new List<Shift>();

            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                shifts = _unitOfWork.Shifts.GetAll(s => s.ComId == companyId);
                ViewBag.SelectedCompanyName = _unitOfWork.Companies.Get(c => c.ComId == companyId)?.ComName;
            }

            return View(shifts);
        }

        // GET: /Shift/Create
        public IActionResult Create()
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            if (!Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                return RedirectToAction(nameof(Index));
            }

            var shift = new Shift { ComId = companyId };
            return PartialView("_Create", shift);
        }

        // POST: /Shift/CreateAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAjax(Shift shift)
        {
            if (ModelState.IsValid)
            {
                shift.ShiftId = Guid.NewGuid();
                _unitOfWork.Shifts.Add(shift);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Validation Error." });
        }

        // GET: /Shift/Edit/5
        public IActionResult Edit(Guid id)
        {
            var shift = _unitOfWork.Shifts.Get(s => s.ShiftId == id);
            if (shift == null)
            {
                return NotFound();
            }
            return PartialView("_Edit", shift);
        }

        // POST: /Shift/EditAjax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAjax(Shift shift)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Shifts.Update(shift);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Validation Error" });
        }

        // POST: /Shift/DeleteAjax/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAjax(Guid id)
        {
            var shift = _unitOfWork.Shifts.Get(s => s.ShiftId == id);
            if (shift == null)
            {
                return Json(new { success = false, message = "Error: Not Found" });
            }

            _unitOfWork.Shifts.Remove(shift);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }
}