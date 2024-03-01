using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.AssistantProfile.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.AssistantProfile.Query;

public record GetAssistantProfileQuery : IQuery<ResponseAssistantProfile>;

public class GetAssistantProfileHandler : IQueryHandler<GetAssistantProfileQuery, ResponseAssistantProfile>
{
    private readonly IAssistantProfileRepository _repository;

    public GetAssistantProfileHandler(IAssistantProfileRepository assistantProfileRepository)
    {
        _repository = assistantProfileRepository;
    }

    public async Task<Result<ResponseAssistantProfile>> Handle(GetAssistantProfileQuery request, CancellationToken cancellationToken)
    {
        var assistant = await _repository.GetAssistantProfileAsync(cancellationToken);
        if (assistant == null)
        {
            return Result.Failure<ResponseAssistantProfile>(new(HttpStatusCode.NotFound, default));
        }

        var response =  new ResponseAssistantProfile()
        {
            APP_NAME = assistant.APP_NAME,
            APP_IMAGE = assistant.APP_IMAGE,
            
        };
        return Result.Success(response);
    }

}
