namespace KM_Management.EndPoint.Category.Models
{
    public class EntityPostCategoryList
    {
        public Guid? Uid { get; set; }
        public string? Name { get; set; }
        public int Layer { get; set; }
        public Guid? Uid_Reference { get; set; }
        public bool Is_Active { get; set; }
        public string? Create_By { get; set; }
        public DateTime Create_At { get; set; }
    }
}
