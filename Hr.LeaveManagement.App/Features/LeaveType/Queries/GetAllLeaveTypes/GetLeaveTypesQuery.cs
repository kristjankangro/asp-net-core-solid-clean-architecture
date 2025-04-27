using MediatR;

namespace Hr.LeaveManagement.App.Features.LeaveType.Queries.GetAllLeaveTypes;

public record GetLeaveTypesQuery : IRequest<List<LeaveTypeDto>>;