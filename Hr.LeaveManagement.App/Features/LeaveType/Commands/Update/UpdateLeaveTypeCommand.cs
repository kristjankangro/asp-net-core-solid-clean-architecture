using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.Update;

public class UpdateLeaveTypeCommand : IRequest<Unit>
{
	public int Name { get; set; }
	public int DefaultDays { get; set; }
	
}