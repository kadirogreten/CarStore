using System.Linq.Expressions;

namespace CarStore.Business
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        T FindOne(Expression<Func<T, bool>> predicate);
    }
}
