namespace KM_Management.EndPoint.Roles.Models;

public class FilterUsersRole
{
	public string? Search_Keyword { get; set; }

	public int Current_Page { get; set; } = 1;
	public int Page_Limit { get; set; } = 10;
	public string? Sort_Order { get; set; } = null;
	public string? Sort_Column { get; set; } = null;
}
