using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.Domain;

namespace Hr.LeaveManagement.App.DataAccessContracts.Persistence;

public interface ILeaveTypeRepo : IGenericRepo<LeaveType>
{
	Task<bool> IsLeaveTypeUnique(string name);
}