using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CarStore.Business
{
    public abstract class GenericRepository<T, TContext> : IRepository<T> where T : class
        where TContext : DbContext
    {


        protected readonly TContext _context;

        protected GenericRepository(TContext context)
        {
            _context = context;
        }


        public T FindOne(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }


        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {

            return _context.Set<T>().Where(predicate).ToList();
        }


        public void Insert(T entity)
        {
            PropertyInfo propertyStatusInfo = entity.GetType().GetProperty("Status");
            propertyStatusInfo?.SetValue(entity, Convert.ToByte(1));

            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            PropertyInfo propertyDeletedInfo = entity.GetType().GetProperty("Deleted");
            propertyDeletedInfo?.SetValue(entity, DateTime.Now);

            Update(entity);
        }

    }
}
