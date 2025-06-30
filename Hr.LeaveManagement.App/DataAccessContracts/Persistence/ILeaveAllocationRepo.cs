using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.Domain;

namespace Hr.LeaveManagement.App.DataAccessContracts.Persistence;

public interface ILeaveAllocationRepo : IGenericRepo<LeaveAllocation>
{
	Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id);
	Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails();
	Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId);
	Task<bool> AllocationExists(string userId, int leaveTypeId, int period);
	Task AddAllocations(List<LeaveAllocation> allocations);
	Task<List<LeaveAllocation>> GetUserAllocations(string userId, int leaveTypeId);
}