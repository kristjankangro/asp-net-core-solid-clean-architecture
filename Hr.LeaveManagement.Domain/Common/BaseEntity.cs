namespace Hr.LeaveManagement.Domain.Common;

/// <summary>
/// Test
/// </summary>
public abstract class BaseEntity
{
	public int Id { get; set; }
	public DateTime? CreatedDate { get; set; } = DateTime.Now;
	public DateTime? ModifiedDate { get; set; } = DateTime.Now;
}