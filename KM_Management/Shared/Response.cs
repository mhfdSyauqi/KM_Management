using KM_Management.EndPoint.Content.Models;
using System.Text.Json.Serialization;

namespace KM_Management.Shared;

public class CustomOkResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CustomOkResponseData? Data { get; }

    public CustomOkResponse(CustomOkResponseData? responseData)
    {
        Data = responseData;
    }
}

public class CustomOkResponseData
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ResponseContent? Contents { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ResponseDetailContent? Content { get; set; }
}