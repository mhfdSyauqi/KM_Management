using KM_Management.EndPoint.RateAndFeedback.Models;

namespace KM_Management.EndPoint.RateAndFeedback.Models
{
    public class ResponseRateAndFeedback
    {
        public string? Periode { get; set; }
        public Summary_User_Feedback? Summary { get; set; }
        public List<User_Feedback>? Items { get; set; }
        public int? Total_Row { get; set; }
        public int? Prev_Page { get; set; }
        public int? Curr_Page { get; set; }
        public int? Next_Page { get; set; }
        public int? Max_Page { get; set; }
    }


    public class Summary_User_Feedback
    {
        public int? User_Preview { get; init; }
        public int? Total_Feedback { get; set; }
        public float? Overall_Rating { get; set; }
        public int? Rating_One { get; set; }
        public int? Rating_Two { get; set; }
        public int? Rating_Three { get; set; }
        public int? Rating_Four { get; set; }

    }

    public class User_Feedback
    {
        
        public int? Rating { get; init; }
        public string? Remark { get; init; }
        public string? Uid_Session_Header { get; init; }
        public string? Create_By { get; init; }
        public DateTime? Create_At { get; init; }
        public int? Total_Category { get; init; }
    }

}

