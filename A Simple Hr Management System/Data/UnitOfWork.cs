using A_Simple_Hr_Management_System.Interfaces;
using A_Simple_Hr_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace A_Simple_Hr_Management_System.Data
{
    // Generic Repository Implementation
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet; 

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>(); 
        }

        public T? Get(Expression<Func<T, bool>> filter)
        {
            return _dbSet.FirstOrDefault(filter);
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }

    // Unit of Work Implementation
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IRepository<Company> Companies { get; private set; }
        public IRepository<Designation> Designations { get; private set; }
        public IRepository<Department> Departments { get; private set; }
        public IRepository<Shift> Shifts { get; private set; }
        public IRepository<Employee> Employees { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Companies = new Repository<Company>(_db);
            Designations = new Repository<Designation>(_db);
            Departments = new Repository<Department>(_db);
            Shifts = new Repository<Shift>(_db);
            Employees = new Repository<Employee>(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        
    }
  

    }