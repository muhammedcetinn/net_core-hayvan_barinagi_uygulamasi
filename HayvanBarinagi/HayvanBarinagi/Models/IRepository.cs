using System.Linq.Expressions;

namespace HayvanBarinagi.Models
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProps = null);
        T Get(Expression<Func<T, bool>> filter, string? includeProps = null);
        void Insert(T entity);
        void Delete(T entity);
        void DeleteTo(IEnumerable<T> entities);

    }
}
