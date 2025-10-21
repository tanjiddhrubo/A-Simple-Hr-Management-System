using A_Simple_Hr_Management_System.Data;
using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace A_Simple_Hr_Management_System.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var companies = _unitOfWork.Companies.GetAll();
            return View(companies);
        }
        
        public IActionResult Create()
        {
            return PartialView("_Create", new Company());
        }

        // this method to handle the Ajax POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAjax(Company company)
        {
            if (ModelState.IsValid)
            {
                company.ComId = Guid.NewGuid(); 
                _unitOfWork.Companies.Add(company);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            // If there are validation errors, we can handle sending them back, but for now, we'll keep it simple.
            return Json(new { success = false, message = "Validation error." });
        }

        public IActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var company = _unitOfWork.Companies.Get(c => c.ComId == id);
            if (company == null)
            {
                return NotFound();
            }

            return PartialView("_Edit", company); 
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAjax(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Companies.Update(company);
                _unitOfWork.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Validation Error." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAjax(Guid id)
        {
            var company = _unitOfWork.Companies.Get(c => c.ComId == id);
            if (company == null)
            {
                return Json(new { success = false, message = "Error: Company not found." });
            }

            _unitOfWork.Companies.Remove(company);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful." });
        }
    }

    }