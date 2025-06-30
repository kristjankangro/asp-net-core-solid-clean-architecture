using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.App.DataAccessContracts.Persistence;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetAllLeaveTypes;

public class GetLeaveTypesQueryHandler : LeaveTypeHandlerBase, IRequestHandler<GetLeaveTypesQuery, List<LeaveTypeDto>>
{
	protected GetLeaveTypesQueryHandler(IMapper mapper, ILeaveTypeRepo leaveTypeRepo) : base(mapper, leaveTypeRepo)
	{
	}

	public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypesQuery request, CancellationToken cancellationToken)
	{
		var list = await _leaveTypeRepo.GetAsync();

		return _mapper.Map<List<LeaveTypeDto>>(list);
	}
}