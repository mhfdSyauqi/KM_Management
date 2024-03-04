namespace KM_Management.EndPoint.Message.Models
{
    public class RequestPatchMessage
    {
        public string? Type { get; set; }
        public int Sequence { get; set; }
        public string? Contents { get; set; }
        public bool Is_Active { get; set; }
        public string? Modified_By { get; set; }
        public DateTime Modified_At { get; set; }
    }
}
