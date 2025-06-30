using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.Domain;

namespace Hr.LeaveManagement.App.DataAccessContracts.Persistence;

public interface ILeaveRequestRepo : IGenericRepo<LeaveRequest>
{
	Task<LeaveRequest> GetLeaveRequestWithDetails(int id);
	Task<List<LeaveRequest>> GetLeaveRequestsWithDetails();
	Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId);
}