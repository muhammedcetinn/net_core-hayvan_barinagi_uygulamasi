using HayvanBarinagi.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HayvanBarinagi.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _appDbContext;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            this.dbSet = _appDbContext.Set<T>();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void DeleteTo(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            request = request.Where(filter);
            if(!string.IsNullOrEmpty(includeProps))
            {
                foreach(var prop in includeProps.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.FirstOrDefault();
        }
        public IEnumerable<T> GetReq(Expression<Func<T, bool>> filter, string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            request = request.Where(filter);
            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.ToList();
        }

        public IEnumerable<T> GetAll(string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.ToList();
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }
    }
}
