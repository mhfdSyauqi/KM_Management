namespace KM_Management.EndPoint.Category.Models
{
    public class EntityPatchCategoryList
    {
        public Guid? Uid { get; set; }
        public string? Name { get; set; }
        public bool Is_Active { get; set; }
        public string? Modified_By { get; set; }
        public DateTime Modified_At { get; set; }
    }
}
