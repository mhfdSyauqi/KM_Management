namespace KM_Management.EndPoint.Content.Moldels;

public class FilterContent
{
    public string? Title_Or_Category { get; set; }
    public string? Article_Status { get; set; }
    public bool? Category_Status { get; set; }
    public int Current_Page { get; set; } = 1;
}
