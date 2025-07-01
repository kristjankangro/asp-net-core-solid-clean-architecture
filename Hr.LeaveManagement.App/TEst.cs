namespace Hr.LeaveManagement.App;

public class TEst
{
	public TEst(bool isA, bool isB)
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