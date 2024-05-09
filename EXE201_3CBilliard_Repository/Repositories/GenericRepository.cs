
using EXE201_3CBilliard_Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace EXE201_3CBilliard_Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MyDBContext _context;
        public GenericRepository(MyDBContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }
        public int CountFilter(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }
        public void Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return query.ToList();
        }

        public T? GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public (IEnumerable<T> items, int totalCount) GetWithCount(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            int totalCount = query.Count(); // Đếm tổng số nhà xuất bản thỏa mãn điều kiện

            // Implementing pagination
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                // Ensure the pageIndex and pageSize are valid
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return (query.ToList(), totalCount);
        }


        public void Insert(T obj)
        {
            _context.Set<T>().Add(obj);
        }

        public void Update(T obj)
        {
            _context.Set<T>().Update(obj);
        }
    }
}
