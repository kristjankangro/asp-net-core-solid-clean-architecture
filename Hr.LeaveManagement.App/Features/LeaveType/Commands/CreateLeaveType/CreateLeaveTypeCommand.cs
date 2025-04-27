using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommand : IRequest<int>
{
	public string Name { get; set; } = string.Empty;
	public int DefaultDays { get; set; }
}
