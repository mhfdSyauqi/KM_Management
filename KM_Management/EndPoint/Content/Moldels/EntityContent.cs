namespace KM_Management.EndPoint.Content.Moldels;

public class EntityContent
{
    public Guid Uid { get; init; }
    public string Title { get; init; }
    public string Category { get; init; }
    public string Article_Status { get; init; }
    public bool Category_Status { get; init; }
    public int? Total_Row { get; set; }
    public int? Prev_Page { get; set; }
    public int? Curr_Page { get; set; }
    public int? Next_Page { get; set; }
}
