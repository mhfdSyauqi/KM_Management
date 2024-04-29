using KM_Management.EndPoint.AssistantProfile.Models;
using KM_Management.EndPoint.Category.Models;
using KM_Management.EndPoint.Content.Models;
using KM_Management.EndPoint.Message.Models;
using KM_Management.EndPoint.RateAndFeedback.Models;
using KM_Management.EndPoint.Roles.Models;
using KM_Management.EndPoint.TopIssue.Models;
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
    public ResponseAssistantProfile? Assistant_Profile { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ResponseActiveMessage>? Active_Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ResponseCategoryList>? Category_List { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ResponseCategoryTopIssueSelected>? Category_Top_Issue_Selected { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ResponseCategoryTopIssueAvailable>? Category_Top_Issue_Available { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ResponseCategoriesReference>? Category_Reference { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ResponseRateAndFeedback? Rate_And_Feedback { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public ResponseUserRole? User_Information { get; set; }
}