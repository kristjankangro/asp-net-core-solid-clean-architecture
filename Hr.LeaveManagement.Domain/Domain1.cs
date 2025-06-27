namespace Hr.LeaveManagement.Domain;

public class Domain1
{
	public string Email { get; set; }
}

public class Domain2
{
	string Name { get; set; }
}

interface IDomain1
{
	string Email { get; set; }
} 

interface IDomain2
{
	string Name { get; set; }
}

public class Domain3 : IDomain2
{
	public string Name { get; set; }
}