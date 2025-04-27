using AutoMapper;
using Hr.LeaveManagement.App.Features.LeaveType.Queries.GetAllLeaveTypes;
using Hr.LeaveManagement.App.Features.LeaveType.Queries.GetLeaveTypeDetails;
using Hr.LeaveManagement.Domain;

namespace Hr.LeaveManagement.App.MappingProfiles;

public class LeaveTypeProfile : Profile
{
	public LeaveTypeProfile()
	{
		CreateMap<LeaveTypeDto, LeaveType>().ReverseMap();
		CreateMap<LeaveType, LeaveTypeDetailsDto>().ReverseMap();
	}
}