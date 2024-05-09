using System.Linq.Expressions;

namespace EXE201_3CBilliard_Repository.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null);
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(T obj);
        int Count();
        int CountFilter(Expression<Func<T, bool>> filter);

        (IEnumerable<T> items, int totalCount) GetWithCount(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int? pageIndex = null, int? pageSize = null);

    }
}
