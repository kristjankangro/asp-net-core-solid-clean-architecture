using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.Domain;
using Hr.LeaveManagement.Persistence.DatabaseContext;

namespace Hr.LeaveManagement.Persistence.Repos;

public class LeaveRequestRepo : GenericRepo<LeaveRequest>, ILeaveRequestRepo
{
	public LeaveRequestRepo(HrDbContext context) : base(context)
	{
	}
}