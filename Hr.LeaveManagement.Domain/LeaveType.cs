using Hr.LeaveManagement.Domain.Common;

namespace Hr.LeaveManagement.Domain;

public class LeaveType : BaseEntity
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public int DefaultDays { get; set; }
}