namespace KM_Management.EndPoint.RateAndFeedback.Models
{
    public class FilterRateAndFeedback
    {
        public string? Filter_Date { get; set; }
        public DateTime? Start_Date { get; set; }
        public DateTime? End_Date { get; set; }
        public string Rating { get; set; }
        public int Current_Page { get; set; }
        public int Page_Limit { get; set; }
    }
}
