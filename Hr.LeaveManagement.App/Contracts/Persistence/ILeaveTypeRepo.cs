using Hr.LeaveManagement.Domain;

namespace Hr.LeaveManagement.App.Contracts.Persistence;

public interface ILeaveTypeRepo : IGenericRepo<LeaveType>
{
	Task<bool> IsLeaveTypeUnique(string name);
}