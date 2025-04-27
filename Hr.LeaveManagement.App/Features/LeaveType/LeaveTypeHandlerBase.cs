using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;

namespace Hr.LeaveManagement.App.Features.LeaveType;

public class LeaveTypeHandlerBase
{
	protected readonly ILeaveTypeRepo _leaveTypeRepo;
	protected readonly IMapper _mapper;

	public LeaveTypeHandlerBase(IMapper mapper, ILeaveTypeRepo leaveTypeRepo)
	{
		_leaveTypeRepo = leaveTypeRepo;
		_mapper = mapper;
	}
}