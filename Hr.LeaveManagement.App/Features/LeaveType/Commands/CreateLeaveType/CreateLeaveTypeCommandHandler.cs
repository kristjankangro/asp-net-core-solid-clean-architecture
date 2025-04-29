using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.App.Exceptions;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler : LeaveTypeHandlerBase, IRequestHandler<CreateLeaveTypeCommand, int>
{
	public CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepo leaveTypeRepo) : base(mapper, leaveTypeRepo)
	{
	}

	public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
	{
		var validator = new CreateLeaveTypeCommandValidator(_leaveTypeRepo);
		
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (validationResult.Errors.Any()) throw new BadRequestException("Validation error", validationResult);
		
		var leaveType = _mapper.Map<Domain.LeaveType>(request);
		
		await _leaveTypeRepo.CreateAsync(leaveType);
		return leaveType.Id;
	}
}