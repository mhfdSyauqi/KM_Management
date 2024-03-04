namespace KM_Management.EndPoint.Message.Models
{
    public class RequestDeleteMessage
    {
        public string? Type { get; set; }
        public int Sequence { get; set; }
        public string? Modified_By { get; set; }
        public DateTime Modified_At { get; set; }
    }
}
