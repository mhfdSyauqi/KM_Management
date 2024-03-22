namespace KM_Management.EndPoint.Content.Models;

public class FilterContent
{
	public string? Title_Or_Category { get; set; }
	public string? Article_Status { get; set; }
	public bool? Category_Status { get; set; }
	public int Current_Page { get; set; } = 1;

	public int Page_Limit { get; set; } = 10;
	public string? Sort_Order { get; set; } = null;
	public string? Sort_Column { get; set; } = null;
}
