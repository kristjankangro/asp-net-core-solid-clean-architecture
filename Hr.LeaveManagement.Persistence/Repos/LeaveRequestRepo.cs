using Hr.LeaveManagement.App.DataAccessContracts.Persistence;
using Hr.LeaveManagement.Domain;
using Hr.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Hr.LeaveManagement.Persistence.Repos;

public class LeaveRequestRepo : GenericRepo<LeaveRequest>, ILeaveRequestRepo
{
	public LeaveRequestRepo(HrDbContext context) : base(context)
	{
	}

	public Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
	{
		return _context.LeaveRequests
			.Include(lr => lr.LeaveType)
			.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails() =>
		await _context.LeaveRequests
			.Include(lr => lr.LeaveType)
			.ToListAsync();

	public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId) =>
		await _context.LeaveRequests
			.Where(x => x.RequestingEmployeeId  == userId)
			.Include(lr => lr.LeaveType)
			.ToListAsync();
}