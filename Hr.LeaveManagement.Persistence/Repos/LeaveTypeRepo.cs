using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.Domain;
using Hr.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Hr.LeaveManagement.Persistence.Repos;

public class LeaveTypeRepo : GenericRepo<LeaveType>, ILeaveTypeRepo
{
	public LeaveTypeRepo(HrDbContext context) : base(context)
	{
	}

	public async Task<bool> IsLeaveTypeUnique(string name)
	{
		return await _context.Set<LeaveType>().AnyAsync(x => x.Name == name);
	}
}