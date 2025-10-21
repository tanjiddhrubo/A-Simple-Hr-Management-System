using System.Linq.Expressions;

namespace A_Simple_Hr_Management_System.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // Gets a single item based on a filter (e.g., by ID)
        T? Get(Expression<Func<T, bool>> filter);

        // Gets all items
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null);
        void Add(T entity);
        void Update(T entity);

        void Remove(T entity);
    }
}