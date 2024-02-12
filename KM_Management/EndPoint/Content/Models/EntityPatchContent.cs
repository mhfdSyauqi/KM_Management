namespace KM_Management.EndPoint.Content.Models;

public class EntityPatchContent
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Article { get; set; }
    public string? Additional_Link { get; set; }
    public Guid? Category { get; set; }
    public string Modified_By { get; set; }
}
