namespace KM_Management.EndPoint.Analytic.Models;

public class RequestHitAnalytic
{
    public DateTime? Start_Date { get; set; }
    public DateTime? End_Date { get; set; }
    public string? Type { get; set; }
    public string? Filter { get; set; }
}
