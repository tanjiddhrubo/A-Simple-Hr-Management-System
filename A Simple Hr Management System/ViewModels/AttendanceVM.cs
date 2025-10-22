using System.ComponentModel.DataAnnotations;

namespace A_Simple_Hr_Management_System.ViewModels
{
    public class AttendanceVM
    {
        public Guid EmpId { get; set; }
        public string EmpName { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public TimeOnly? ShiftInTime { get; set; }
        public DateOnly dtDate { get; set; }

        // These are the fields the user will fill in
        [Display(Name = "In Time")]
        public TimeOnly? InTime { get; set; }

        [Display(Name = "Out Time")]
        public TimeOnly? OutTime { get; set; }

        [Display(Name = "Status")]
        public string AttStatus { get; set; } = "A"; // Default to Absent
    }
}