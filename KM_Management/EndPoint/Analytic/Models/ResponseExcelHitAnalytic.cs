namespace KM_Management.EndPoint.Analytic.Models;

public class ResponseExcelHitAnalytic
{
    public List<EntityExcelHitAnalytic> Match_Data { get; set; }
    public List<EntityExcelHitAnalytic> Unmatch_Data { get; set; }
    public string Period { get; set; }
}
