using System.Text.Json.Serialization;

namespace KM_Management.EndPoint.Category.Models
{
    public class EntityCategoryTopIssueSelected
    {
        public Guid Uid { get; set; }
        public string? Name { get; set; } 
        public bool? Is_Active { get; set; }
        public int? Sequence { get; set; }
        public Guid Uid_Bot_Category { get; set; }
    }
}
