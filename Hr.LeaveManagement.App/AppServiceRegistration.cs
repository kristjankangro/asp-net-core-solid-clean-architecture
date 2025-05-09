using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.LeaveManagement.App;

public static class AppServiceRegistration
{
	public static IServiceCollection AddAppServices(this IServiceCollection services)
	{
		services.AddAutoMapper(Assembly.GetExecutingAssembly());
		services.AddMediatR(Assembly.GetExecutingAssembly());
		
		return services;
	}
}