using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetLeaveTypeDetails;

public class GetLeaveTypeDetailsQueryHandler : LeaveTypeHandlerBase, IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
{
	protected GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepo leaveTypeRepo) : base(mapper, leaveTypeRepo)
	{
	}

	public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
	{
		var leaveType = await _leaveTypeRepo.GetByIdAsync(request.Id);
		return _mapper.Map<LeaveTypeDetailsDto>(leaveType);
	}
}
