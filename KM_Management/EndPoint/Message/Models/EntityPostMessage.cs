namespace KM_Management.EndPoint.Message.Models
{
    public class EntityPostMessage
    {
        public string? Type { get; set; }
        public int Sequence { get; set; }
        public string? Contents { get; set; }
        public bool Is_Active { get; set; }
        public string? Create_By { get; set; }
        public DateTime Create_At { get; set; }
    }
}
