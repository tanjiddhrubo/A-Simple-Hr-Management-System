using A_Simple_Hr_Management_System.Models; 
using Microsoft.EntityFrameworkCore;

namespace A_Simple_Hr_Management_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        // This constructor allows the configuration from Program.cs to be passed in.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // This property creates the "Companies" table in your database.
        public DbSet<Company> Companies { get; set; }

        public DbSet<Designation> Designations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Shift> Shifts { get; set; }
    }
}