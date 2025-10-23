using A_Simple_Hr_Management_System.Models;

namespace A_Simple_Hr_Management_System.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Company> Companies { get; }
        IRepository<Designation> Designations { get; }
        IRepository<Department> Departments { get; }
        IRepository<Shift> Shifts { get; }
        IRepository<Employee> Employees { get; }
        IRepository<Attendance> Attendances { get; }
        IRepository<AttendanceSummary> AttendanceSummaries { get; }

        IRepository<Salary> Salaries { get; }
        void Save();
    }
}