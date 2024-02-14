namespace KM_Management.EndPoint.Content.Models;

public class EntityDetailContent
{
    public Guid Uid { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Article { get; init; }
    public string? Additonal_Link { get; init; }
    public string Status { get; init; }
    public bool? Is_Active { get; init; }
    public Guid? Uid_Category { get; init; }
    public string Create_By { get; init; }
    public DateTime Create_At { get; set; }
    public string? Modified_By { get; init; }
    public DateTime? Modified_At { get; set; }
    public string? Published_By { get; init; }
    public DateTime? Published_At { get; set; }
}
