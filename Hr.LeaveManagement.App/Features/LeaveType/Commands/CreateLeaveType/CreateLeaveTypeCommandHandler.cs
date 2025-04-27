using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler : LeaveTypeHandlerBase, IRequestHandler<CreateLeaveTypeCommand, int>
{
	public CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepo leaveTypeRepo) : base(mapper, leaveTypeRepo)
	{
	}

	public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
	{
		// Validate the request
		
		var leaveType = _mapper.Map<Domain.LeaveType>(request);
		
		await _leaveTypeRepo.CreateAsync(leaveType);
		return leaveType.Id;
	}
}