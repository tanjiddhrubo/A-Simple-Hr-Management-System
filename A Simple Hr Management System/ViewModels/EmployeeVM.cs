using A_Simple_Hr_Management_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace A_Simple_Hr_Management_System.ViewModels
{
    public class EmployeeVM
    {
        public Employee Employee { get; set; } = new(); // Initialize the Employee object
        public IEnumerable<SelectListItem> DesignationList { get; set; } = Enumerable.Empty<SelectListItem>(); // Initialize the lists
        public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> ShiftList { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}