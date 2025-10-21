using A_Simple_Hr_Management_System.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace A_Simple_Hr_Management_System.ViewComponents
{
    public class CompanySelectorViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanySelectorViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            var companies = _unitOfWork.Companies.GetAll();
            return View(companies);
        }
    }
}