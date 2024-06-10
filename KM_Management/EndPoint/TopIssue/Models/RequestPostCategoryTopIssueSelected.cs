namespace KM_Management.EndPoint.TopIssue.Models
{
    public class RequestPostCategoryTopIssueSelected
    {
        public string Uid_Bot_Category { get; set; }
        public string? Create_By { get; set; }
        public DateTime Create_At { get; set; }
        public string? Modified_By { get; set; }
        public DateTime Modified_At { get; set; }
    }
}
