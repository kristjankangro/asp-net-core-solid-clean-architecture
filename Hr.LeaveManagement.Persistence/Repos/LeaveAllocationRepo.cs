using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.Domain;
using Hr.LeaveManagement.Persistence.DatabaseContext;

namespace Hr.LeaveManagement.Persistence.Repos;

public class LeaveAllocationRepo : GenericRepo<LeaveAllocation>, ILeaveAllocationRepo
{
	public LeaveAllocationRepo(HrDbContext context) : base(context)
	{
	}
}