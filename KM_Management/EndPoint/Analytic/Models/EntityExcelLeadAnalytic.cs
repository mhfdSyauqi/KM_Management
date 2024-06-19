namespace KM_Management.EndPoint.Analytic.Models;

public class EntityExcelLeadAnalytic
{
    public List<EntityExcelGeneralLead> General { get; set; }
    public List<EntityExcelDetailLead> Detail { get; set; }
    public string Period { get; set; }
}

public class EntityExcelGeneralLead
{
    public string User_Name { get; set; }
    public int Usage { get; set; }
    public string Category { get; set; }
    public int Category_Count { get; set; }
}

public class EntityExcelDetailLead
{
    public string User_Name { get; set; }
    public int Usage { get; set; }
    public string Category { get; set; }
    public string Time { get; set; }
}
