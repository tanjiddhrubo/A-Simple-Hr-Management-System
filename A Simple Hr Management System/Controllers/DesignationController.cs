using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace A_Simple_Hr_Management_System.Controllers
{
    public class DesignationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DesignationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Try to read the selected company ID from the cookie
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            IEnumerable<Designation> designations = new List<Designation>();

            if (Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                // If a valid company ID is found, filter the designations
                designations = _unitOfWork.Designations.GetAll(d => d.ComId == companyId);
                // Pass the selected company name to the view for display
                ViewBag.SelectedCompanyName = _unitOfWork.Companies.Get(c => c.ComId == companyId)?.ComName;
            }

            return View(designations);
        }

        // method to return a PartialView
        public IActionResult Create()
        {
            var selectedCompanyIdCookie = Request.Cookies["SelectedCompanyId"];
            if (!Guid.TryParse(selectedCompanyIdCookie, out Guid companyId))
            {
                return RedirectToAction(nameof(Index));
            }

            var designation = new Designation { ComId = companyId };
            return PartialView("_Create", designation); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAjax(Designation designation)
        {
            if (ModelState.IsValid)
            {
                designation.DesigId = Guid.NewGuid();
                _unitOfWork.Designations.Add(designation);
                _unitOfWork.Save();
                return Json(new { success = true });
            }

            // If we got this far, something failed. Let's collect the errors.
            var errors = ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();

            return Json(new { success = false, errors = errors });
        }
        public IActionResult Edit(Guid id)
        {
            var designation = _unitOfWork.Designations.Get(d => d.DesigId == id);
            if (designation == null)
            {
                return NotFound();
            }
            return PartialView("_Edit", designation);
        }

        // POST: Saves changes from the Edit form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAjax(Designation designation)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Designations.Update(designation);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Validation Error" });
        }

        // POST: Deletes a designation via Ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAjax(Guid id)
        {
            var designation = _unitOfWork.Designations.Get(d => d.DesigId == id);
            if (designation == null)
            {
                return Json(new { success = false, message = "Error: Not Found" });
            }

            _unitOfWork.Designations.Remove(designation);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful." });
        }
    }

}