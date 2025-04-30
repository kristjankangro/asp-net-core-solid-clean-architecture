using Hr.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.LeaveManagement.Persistence.Configurations;

public class LeaveTypeConf : IEntityTypeConfiguration<LeaveType>
{
	public void Configure(EntityTypeBuilder<LeaveType> builder)
	{
		builder.HasData(new LeaveType()
		{
			Id = 1,
			Name = "Vacation",
			DefaultDays = 10,
			CreatedDate = DateTime.UtcNow,
			ModifiedDate = DateTime.UtcNow
		});

		//db validation
		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(100);
	}
}