namespace KM_Management.EndPoint.Content.Models;

public class ResponseContent
{
    public List<Contents> Items { get; set; }
    public int? Total_Row { get; set; }
    public int? Prev_Page { get; set; }
    public int? Curr_Page { get; set; }
    public int? Next_Page { get; set; }
}

public class Contents
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public string Article_Status { get; set; }
    public bool Category_Status { get; set; }
}
