namespace Hr.LeaveManagement.App;

public class TEst
{
	public void Execute(bool isA, bool isB, bool isC, bool isD, bool isE)
	{
		if (isA && isB)
			Console.WriteLine("Both are true");
		else if (isA || isB)
			Console.WriteLine("One of them is true");
		else
			Console.WriteLine("Neither is true");
	}

	public bool Execute2() => false;
}