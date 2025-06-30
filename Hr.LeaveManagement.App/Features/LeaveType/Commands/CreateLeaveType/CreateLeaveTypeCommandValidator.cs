using System.Data;
using FluentValidation;
using Hr.LeaveManagement.App.Contracts.Persistence;
using Hr.LeaveManagement.App.DataAccessContracts.Persistence;

namespace Hr.LeaveManagement.App.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
{
	private readonly ILeaveTypeRepo _repo;

	public CreateLeaveTypeCommandValidator(ILeaveTypeRepo repo)
	{
		_repo = repo;
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Leave type name is required. {PropertyName}")
			.MaximumLength(100).WithMessage("Leave type name must not exceed 100 characters.");
		
			RuleFor(x => x.DefaultDays)
			.LessThan(100).WithMessage("{PropertyName} must be less than 100.")
			.GreaterThan(1).WithMessage("{PropertyName} must be greater than 1.");

			RuleFor(x => x)
				.MustAsync(LeaveTypeNameMustBeUnique)
				.WithMessage("Leave type name must be unique.");
	}

	private async Task<bool> LeaveTypeNameMustBeUnique(CreateLeaveTypeCommand command, CancellationToken cancellationToken) 
		=> await _repo.IsLeaveTypeUnique(command.Name);
}