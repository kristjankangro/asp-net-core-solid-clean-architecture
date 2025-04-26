namespace Hr.LeaveManagement.App.Contracts.Persistence;

public interface IGenericRepo<T> where T : class
{
	Task<IReadOnlyList<T>> GetAsync();
	Task<T> GetAsync(int id);
	Task<T> CreateAsync(T entity);
	Task UpdateAsync(T entity);
	Task DeleteAsync(T entity);
}