namespace KM_Management.EndPoint.List.Models
{
    public class RequestPostCategoryList
    {
        public string? Uid { get; set; }
        public string? Name { get; set; }
        public int Layer { get; set; }
        public string? Uid_Reference { get; set; }
        public bool Is_Active { get; set; }
        public string? Create_By { get; set; }
        public DateTime Create_At { get; set; }
    }
}
