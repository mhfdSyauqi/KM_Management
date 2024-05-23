namespace KM_Management.EndPoint.Analytic.Models;

public class ResponseDetailHitAnalytic
{
    public double Total_Percentage { get; set; }
    public List<ResponseDetailHitItem> Categories { get; set; }
}

public class ResponseDetailHitItem
{
    public int Count { get; set; }
    public string Category { get; set; }
}
