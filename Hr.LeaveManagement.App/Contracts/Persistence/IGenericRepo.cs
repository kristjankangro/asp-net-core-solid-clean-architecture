namespace Hr.LeaveManagement.App.Contracts.Persistence;

public interface IGenericRepo<T> where T : class
{
	Task<IReadOnlyList<T>> GetAsync();
	Task<T> GetByIdAsync(int id);
	Task CreateAsync(T entity);
	Task UpdateAsync(T entity);
	Task DeleteAsync(T entity);
}