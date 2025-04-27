using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetLeaveTypeDetails;

public class GetLeaveTypeDetailsQuery : IRequest<LeaveTypeDetailsDto>
{
	public GetLeaveTypeDetailsQuery(int Id)
	{
		this.Id = Id;
	}

	public int Id { get; init; }

}
