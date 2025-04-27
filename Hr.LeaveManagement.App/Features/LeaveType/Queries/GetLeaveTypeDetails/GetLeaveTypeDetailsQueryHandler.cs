using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.App.Exceptions;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetLeaveTypeDetails;

public class GetLeaveTypeDetailsQueryHandler : LeaveTypeHandlerBase,
	IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
{
	protected GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepo leaveTypeRepo) : base(mapper,
		leaveTypeRepo)
	{
	}

	public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
	{
		var leaveType = await _leaveTypeRepo.GetByIdAsync(request.Id);
		if (leaveType == null)
			throw new NotFoundException(nameof(leaveType), request.Id);
		
		return _mapper.Map<LeaveTypeDetailsDto>(leaveType);
	}
}