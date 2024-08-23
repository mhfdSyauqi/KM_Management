namespace KM_Management.EndPoint.RateAndFeedback.Models
{
    public class RequestExcelRateAndFeedback
    {
        public string? Filter_Date { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string? Rating { get; set; }
    }
}
