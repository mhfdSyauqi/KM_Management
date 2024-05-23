namespace KM_Management.EndPoint.Analytic.Models;

public class RequestLeaderboardAnalytic
{
    public DateTime? Start_Date { get; set; }
    public DateTime? End_Date { get; set; }
    public string? Filter { get; set; }
}
