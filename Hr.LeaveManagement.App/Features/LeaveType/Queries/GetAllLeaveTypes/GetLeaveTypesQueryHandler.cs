using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetAllLeaveTypes;

public class GetLeaveTypesQueryHandler : IRequestHandler<GetLeaveTypesQuery, List<LeaveTypeDto>>
{
	private readonly IMapper _mapper;
	private readonly ILeaveTypeRepo _leaveTypeRepo;

	public GetLeaveTypesQueryHandler(IMapper mapper, ILeaveTypeRepo leaveTypeRepo)
	{
		_mapper = mapper;
		_leaveTypeRepo = leaveTypeRepo;
	}

	public async Task<List<LeaveTypeDto>> Handle(GetLeaveTypesQuery request, CancellationToken cancellationToken)
	{
		var list = await _leaveTypeRepo.GetAsync();

		return _mapper.Map<List<LeaveTypeDto>>(list);
	}
}