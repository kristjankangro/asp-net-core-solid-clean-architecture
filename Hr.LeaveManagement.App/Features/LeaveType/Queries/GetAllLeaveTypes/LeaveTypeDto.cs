namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetAllLeaveTypes;

public class LeaveTypeDto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int DefaultDays { get; set; }
}

public class LeaveType2Dto: LeaveTypeDto
{
	public string Description { get; set; } = string.Empty;
}