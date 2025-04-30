using Hr.LeaveManagement.Domain;
using Hr.LeaveManagement.Domain.Common;
using Hr.LeaveManagement.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Hr.LeaveManagement.Persistence.DatabaseContext;

public class HrDbContext : DbContext
{
	public HrDbContext(DbContextOptions<HrDbContext> options) : base(options)
	{
	}

	public DbSet<LeaveType> LeaveTypes { get; set; }
	public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
	public DbSet<LeaveRequest> LeaveRequests { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//same is done below modelBuilder.ApplyConfiguration(new LeaveTypeConf());
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrDbContext).Assembly);
		
		
		base.OnModelCreating(modelBuilder);
	}
	
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var entry in ChangeTracker.Entries<BaseEntity>()
			         .Where(q =>
				         q.State is EntityState.Added
					         or EntityState.Modified))
		{
			entry.Entity.ModifiedDate = DateTime.UtcNow;

			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedDate = DateTime.UtcNow;
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}