using Hr.LeaveManagement.App.DataAccessContracts.Persistence;
using Hr.LeaveManagement.Domain;
using Hr.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Hr.LeaveManagement.Persistence.Repos;

public class LeaveAllocationRepo : GenericRepo<LeaveAllocation>, ILeaveAllocationRepo
{
	public LeaveAllocationRepo(HrDbContext context) : base(context)
	{
	}

	public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id) =>
		await _context.LeaveAllocations
			.Include(la => la.LeaveType)
			.FirstOrDefaultAsync(x => x.Id == id);

	public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
		=> await _context.LeaveAllocations
			.Include(x => x.LeaveType)
			.ToListAsync();

	public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId) =>
		await _context.LeaveAllocations
			.Include(x => x.LeaveType)
			.Where(x => x.EmployeeId == userId)
			.ToListAsync();

	public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period) =>
		await _context.LeaveAllocations
			.AnyAsync(x => x.EmployeeId == userId
			               && x.LeaveTypeId == leaveTypeId
			               && x.Period == period);

	public async Task AddAllocations(List<LeaveAllocation> allocations) =>
		await _context.LeaveAllocations.AddRangeAsync(allocations);

	public async Task<List<LeaveAllocation>> GetUserAllocations(string userId, int leaveTypeId) =>
		await _context.LeaveAllocations
			.Where(x => x.EmployeeId == userId
			            && x.LeaveTypeId == leaveTypeId)
			.ToListAsync();
}