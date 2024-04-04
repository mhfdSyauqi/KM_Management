using KM_Management.EndPoint.Category.Models;
using KM_Management.EndPoint.Content.Models;
using KM_Management.EndPoint.General.Models;
using KM_Management.EndPoint.Roles.Models;
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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ResponseCategoriesReference>? Category_Reference { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ResponseUserRole? User_Information { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ResponseUsersRole? Users_Role { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ResponseUserReference>? User_Ref { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ResponseConfigGeneral? General { get; set; }
}