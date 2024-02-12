namespace KM_Management.EndPoint.Content.Models;

public class RequestContent
{
    public string? Searched_Title_Or_Category { get; set; }
    public List<string>? Searched_Article_Status { get; set; }
    public bool Inactive_Category { get; set; }
    public int Searched_Page { get; set; } = 1;
}
