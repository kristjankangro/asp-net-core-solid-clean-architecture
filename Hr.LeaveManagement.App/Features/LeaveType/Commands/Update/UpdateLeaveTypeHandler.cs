using AutoMapper;
using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.App.DataAccessContracts.Persistence;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.Update;

public class UpdateLeaveTypeHandler : LeaveTypeHandlerBase, IRequestHandler<UpdateLeaveTypeCommand, Unit>
{
	public UpdateLeaveTypeHandler(IMapper mapper, ILeaveTypeRepo leaveTypeRepo) : base(mapper, leaveTypeRepo)
	{
	}

	public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
	{
		var leaveType = _mapper.Map<Domain.LeaveType>(request);
		await _leaveTypeRepo.UpdateAsync(leaveType);
		return Unit.Value;
	}
}