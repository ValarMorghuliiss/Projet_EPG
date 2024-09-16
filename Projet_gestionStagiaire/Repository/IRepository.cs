using System.Linq.Expressions;

namespace Projet_gestionStagiaire.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync();
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

    }
}
