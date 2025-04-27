using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
{
	private readonly IMapper _mapper;
	private readonly ILeaveTypeRepo _leaveTypeRepo;

	public CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepo leaveTypeRepo)
	{
		_mapper = mapper;
		_leaveTypeRepo = leaveTypeRepo;
	}

	public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
	{
		// Validate the request
		
		var leaveType = _mapper.Map<Domain.LeaveType>(request);
		
		await _leaveTypeRepo.CreateAsync(leaveType);
		return leaveType.Id;
	}
}