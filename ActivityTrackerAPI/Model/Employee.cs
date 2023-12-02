namespace ActivityTrackerAPI.Model;

public class Employee
{
    public int EmployeeId { get; set; }
    public string? Name { get; set; }
	public string? PaternalLastName { get; set; }
	public string? MaternalLastName { get; set; }
	public string? Email { get; set; }
    public int Status { get; set; }
    public int TeamId { get; set; }
}