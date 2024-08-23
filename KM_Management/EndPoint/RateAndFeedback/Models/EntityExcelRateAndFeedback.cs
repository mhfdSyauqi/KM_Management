﻿namespace KM_Management.EndPoint.RateAndFeedback.Models
{
    public class EntityExcelRateAndFeedback
    {
        public int? Rating { get; init; }
        public string? Remark { get; init; }
        public Guid? Uid_Session_Header { get; init; }
        public string? Create_By { get; init; }
        public DateTime? Create_At { get; init; }
        public int? Total_Category { get; init; }
        public string? Periode { get; set; }
    }
}
