using Hr.LeaveManagement.App.Contracts.Persistence;
using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.Delete;

public class DeleteLeaveTypeHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
{
	private ILeaveTypeRepo _leaveTypeRepo;

	public DeleteLeaveTypeHandler(ILeaveTypeRepo leaveTypeRepo) => _leaveTypeRepo = leaveTypeRepo;

	public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
	{
		var leaveType = await _leaveTypeRepo.GetByIdAsync(request.Id);

		if (leaveType == null)
			throw new InvalidOperationException($"LeaveType with id {request.Id} not found");

		await _leaveTypeRepo.DeleteAsync(leaveType);
		return Unit.Value;
	}
}