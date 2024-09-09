namespace Domain.Interfaces
{
    // Interface representing a repository for performing CRUD operations on entities
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> ListAsync(ISpecification<T> spec);
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveChangesAsync();
    }
}
