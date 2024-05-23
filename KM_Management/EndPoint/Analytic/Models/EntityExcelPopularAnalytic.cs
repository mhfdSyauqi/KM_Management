namespace KM_Management.EndPoint.Analytic.Models;

public class EntityExcelPopularAnalytic
{
    public List<EntityExcelGenPopularAnalytic> General { get; set; }
    public List<EntityExcelDetPopularAnalytic> Detail { get; set; }
    public string Period { get; set; }
}
