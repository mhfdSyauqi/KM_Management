using System.Text.Json.Serialization;

namespace KM_Management.EndPoint.Category.Models
{
    public class RequestPatchCategoryTopIssueSelectedSequence
    {
        public int Current_Sequence { get; set; }
        public int New_Sequence { get; set; }
        public string? Modified_By { get; set; }
        public DateTime Modified_At { get; set; }
    }
}
