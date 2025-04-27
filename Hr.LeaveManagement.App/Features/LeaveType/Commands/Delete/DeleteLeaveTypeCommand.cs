using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.Delete;

public class DeleteLeaveTypeCommand : IRequest<Unit>
{
	public int Id { get; set; }
}