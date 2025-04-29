using FluentValidation.Results;

namespace Hr.LeaveManagement.App.Exceptions;

public class BadRequestException : Exception
{
	public List<string> Errors { get; set; } = new List<string>();

	public BadRequestException(string message) : base(message)
	{
	}

	public BadRequestException(string message, ValidationResult validationResult) : base(message)
	{
		Errors = validationResult.Errors
			.Select(x => x.ErrorMessage)
			.ToList();
	}
}