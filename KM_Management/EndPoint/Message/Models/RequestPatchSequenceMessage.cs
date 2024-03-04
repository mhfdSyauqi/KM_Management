namespace KM_Management.EndPoint.Message.Models
{
    public class RequestPatchSequenceMessage
    {
        public string? Type { get; set; }
        public int Current_Sequence { get; set; }
        public int New_Sequence { get; set; }
        public string? Modified_By { get; set; }
        public DateTime Modified_At { get; set; }
    }
}
