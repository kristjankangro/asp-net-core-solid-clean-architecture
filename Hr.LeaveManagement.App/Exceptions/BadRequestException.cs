namespace Hr.LeaveManagement.App.Exceptions;

public class BadRequestException : Exception
{
	public BadRequestException(string message) : base(message)
	{
	}
}