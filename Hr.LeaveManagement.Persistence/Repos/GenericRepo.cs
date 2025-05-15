using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.Domain.Common;
using Hr.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Hr.LeaveManagement.Persistence.Repos;

public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
{
	protected readonly HrDbContext _context;
	public GenericRepo(HrDbContext context) => _context = context;

	public async Task<IReadOnlyList<T>> GetAsync()
		=> await _context.Set<T>().AsNoTracking().ToListAsync();

	public async Task<T> GetByIdAsync(int id)
		=> await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

	public async Task CreateAsync(T entity)
	{
		await _context.AddAsync(entity);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(T entity)
	{
		// _context.Update(entity);
		// await _context.SaveChangesAsync();
		// or
		_context.Entry(entity).State = EntityState.Modified;
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(T entity)
	{
		_context.Remove(entity);
		await _context.SaveChangesAsync();
	}
}