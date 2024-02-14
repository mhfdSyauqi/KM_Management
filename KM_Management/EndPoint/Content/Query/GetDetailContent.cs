using FluentValidation;
using KM_Management.Commons.FluentExtention;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Content.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Content.Query;

public record GetDetailContentQuery(string ContentId) : IQuery<ResponseDetailContent>;

public class GetDetailContentValidator : AbstractValidator<GetDetailContentQuery>
{
    public GetDetailContentValidator()
    {
        RuleFor(key => key.ContentId).BeValidGuid();
    }
}

public class GetDetailContentHandler : IQueryHandler<GetDetailContentQuery, ResponseDetailContent>
{
    private readonly IContentRepository _contentRepository;
    private readonly IValidator<GetDetailContentQuery> _validator;

    public GetDetailContentHandler(IContentRepository contentRepository, IValidator<GetDetailContentQuery> validator)
    {
        _contentRepository = contentRepository;
        _validator = validator;
    }

    public async Task<Result<ResponseDetailContent>> Handle(GetDetailContentQuery request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure<ResponseDetailContent>(new(HttpStatusCode.BadRequest, errMsg));
        }

        var contentId = Guid.Parse(request.ContentId);
        var detailContent = await _contentRepository.GetContentByIdAsync(contentId, cancellationToken);

        if (detailContent == null)
        {
            return Result.Failure<ResponseDetailContent>(new(HttpStatusCode.NotFound, $"Content with id : {request.ContentId} not exist"));
        }

        var response = new ResponseDetailContent()
        {
            Id = detailContent.Uid.ToString("N"),
            Title = detailContent.Title,
            Description = detailContent.Description,
            Article = detailContent.Article,
            Additonal_Link = detailContent.Additonal_Link,
            Status = detailContent.Status,
            Is_Active = detailContent.Is_Active,
            Category_Id = detailContent.Uid_Category?.ToString("N"),
            Create_By = detailContent.Create_By,
            Create_At = detailContent.Create_At,
            Modified_By = detailContent.Modified_By,
            Modified_At = detailContent.Modified_At,
            Published_By = detailContent.Published_By,
            Published_At = detailContent.Published_At,
        };

        return Result.Success(response);

    }
}