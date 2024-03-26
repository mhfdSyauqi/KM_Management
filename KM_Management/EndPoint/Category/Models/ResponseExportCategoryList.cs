namespace KM_Management.EndPoint.Category.Models
{
    public class ResponseExportCategoryList
    {
        public List<First_Layers>? First_Layer { get; set; }
        public List<Second_Layers>? Second_Layer { get; set; }
        public List<Third_Layers>? Third_Layer { get; set; }
    }
}


//public class EntityExportCategoryListFristLayer
//    {
//        public List<First_Layers>? First_Layer { get; set; }
//        public List<Second_Layers>? Second_Layer { get; set; }
//        public List<Third_Layers>? Third_Layer { get; set; }
//    }

public class First_Layers
{
    public string? Layer_One { get; set; }
    public string? Status { get; set; }
    public string? Publication { get; set; }

}

public class Second_Layers
{
    public string? Layer_One { get; set; }
    public string? Layer_Two { get; set; }
    public string? Status { get; set; }
    public string? Publication { get; set; }
}

public class Third_Layers
{
    public string? Layer_One { get; set; }
    public string? Layer_Two { get; set; }
    public string? Layer_Three { get; set; }
    public string? Status { get; set; }
    public string? Publication { get; set; }
}