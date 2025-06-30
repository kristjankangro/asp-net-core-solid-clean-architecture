using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.App.DataAccessContracts.Persistence;
using Hr.LeaveManagement.Persistence.DatabaseContext;
using Hr.LeaveManagement.Persistence.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.LeaveManagement.Persistence;

public static class PersistenceServiceRegistration
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<HrDbContext>(options => options
			.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
		);
		
		//two ways to do it
		services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
		services.AddScoped<ILeaveRequestRepo, LeaveRequestRepo>();
		services.AddScoped<ILeaveTypeRepo, LeaveTypeRepo>();
		services.AddScoped<ILeaveAllocationRepo, LeaveAllocationRepo>();
		
		return services;
	}
}