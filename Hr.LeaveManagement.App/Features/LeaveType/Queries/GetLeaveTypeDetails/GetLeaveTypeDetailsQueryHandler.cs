using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetLeaveTypeDetails;

public class GetLeaveTypeDetailsQueryHandler : IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
{
	private readonly IMapper _mapper;
	private readonly ILeaveTypeRepo _repo;

	public GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepo repo)
	{
		_mapper = mapper;
		_repo = repo;
	}

	public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery request, CancellationToken cancellationToken)
	{
		var leaveType = await _repo.GetByIdAsync(request.Id);
		return _mapper.Map<LeaveTypeDetailsDto>(leaveType);
	}
}
