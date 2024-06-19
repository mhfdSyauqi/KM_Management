namespace KM_Management.EndPoint.Analytic.Models;

public class ResponseExcelLeadAnalytic
{
    public List<EntityExcelGeneralLead> General_Data { get; set; }

    public List<EntityExcelDetailLead> Detail_Data { get; set; }

    public string Period { get; set; }
}
