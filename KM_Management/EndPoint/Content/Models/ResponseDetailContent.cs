﻿namespace KM_Management.EndPoint.Content.Models;

public class ResponseDetailContent
{
    public string Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string Article { get; init; }
    public string? Additional_Link { get; init; }
    public string Status { get; init; }
    public bool? Is_Active { get; init; }
    public string? Category_Id { get; init; }
    public string? Category_Name { get; init; }
    public string Create_By { get; init; }
    public DateTime Create_At { get; set; }
    public string? Modified_By { get; init; }
    public DateTime? Modified_At { get; set; }
    public string? Published_By { get; init; }
    public DateTime? Published_At { get; set; }
}
