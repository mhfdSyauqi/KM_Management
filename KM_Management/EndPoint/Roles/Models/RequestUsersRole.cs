namespace KM_Management.EndPoint.Roles.Models;

public class RequestUsersRole
{
	public string? Search_Keyword { get; set; }

	public int Searched_Page { get; set; } = 1;
	public int Limit_Page { get; set; } = 10;
	public string? Order_Sort { get; set; }
	public string? Column_Sort { get; set; }
}
