namespace KM_Management.EndPoint.Roles.Models;

public class ResponseUsersRole
{

	public List<UsersRole> Items { get; set; }
	public int? Total_Row { get; set; }
	public int? Prev_Page { get; set; }
	public int? Curr_Page { get; set; }
	public int? Next_Page { get; set; }
	public int? Max_Page { get; set; }
}

public class UsersRole
{
	public string Login_Name { get; set; }
	public string Full_Name { get; set; }
	public string Role { get; set; }
	public string Email { get; set; }
	public string Job_Title { get; set; }
}
