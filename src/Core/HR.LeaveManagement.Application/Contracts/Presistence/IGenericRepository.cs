using HR.LeaveManagement.Domain.Common;

namespace HR.LeaveManagement.Application.Contracts.Presistence;
public interface IGenericRepository<T> where T : BaseEntity
{
        Task<IReadOnlyList<T>> GetAsync();
        Task<T?> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
}
