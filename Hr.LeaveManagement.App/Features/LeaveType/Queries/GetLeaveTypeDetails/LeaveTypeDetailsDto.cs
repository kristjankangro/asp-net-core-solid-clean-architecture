namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetLeaveTypeDetails;

public class LeaveTypeDetailsDto
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int DefaultDays { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime ModifiedDate { get; set; }
}