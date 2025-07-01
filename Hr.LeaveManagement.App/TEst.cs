namespace Hr.LeaveManagement.App;

public class TEst
{
	/// <summary>
	/// Prints a message to the console indicating whether both, one, or neither of the first two boolean parameters are true.
	/// </summary>
	/// <param name="isA">First boolean value to evaluate.</param>
	/// <param name="isB">Second boolean value to evaluate.</param>
	/// <param name="isC">Unused parameter.</param>
	/// <param name="isD">Unused parameter.</param>
	/// <param name="isE">Unused parameter.</param>
	public void Execute(bool isA, bool isB, bool isC, bool isD, bool isE)
	{
		if (isA && isB)
		{
			Console.WriteLine("Both are true");
		}
		else if (isA || isB)
		{
			Console.WriteLine("One of them is true");
		}
		else
		{
			Console.WriteLine("Neither is true");
		}
	}
}