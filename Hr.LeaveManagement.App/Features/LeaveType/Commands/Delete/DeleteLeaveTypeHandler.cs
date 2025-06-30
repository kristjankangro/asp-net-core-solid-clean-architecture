using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.App.DataAccessContracts.Persistence;
using Hr.LeaveManagement.App.Exceptions;
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
			throw new NotFoundException(nameof(leaveType), request.Id);

		await _leaveTypeRepo.DeleteAsync(leaveType);
		return Unit.Value;
	}
}