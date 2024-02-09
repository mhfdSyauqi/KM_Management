using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Content.Moldels;
using KM_Management.Shared;

namespace KM_Management.EndPoint.Content.Query;

public record GetContentsQuery(RequestContent Argument) : IQuery<ResponseContent>;

public class GetContentsValidator : AbstractValidator<GetContentsQuery>
{
    public GetContentsValidator()
    {

    }
}

public class GetContentsHandler : IQueryHandler<GetContentsQuery, ResponseContent>
{
    private readonly IContentRepository _contentRepository;
    private readonly IValidator<GetContentsQuery> _validator;

    public GetContentsHandler(IContentRepository contentRepository, IValidator<GetContentsQuery> validator)
    {
        _contentRepository = contentRepository;
        _validator = validator;
    }

    public async Task<Result<ResponseContent>> Handle(GetContentsQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var filter = new FilterContent()
        {
            Title_Or_Category = request.Argument.Searched_Title_Or_Category,
            Article_Status = request.Argument.Searched_Article_Status?.Count > 1 ? null : request.Argument.Searched_Article_Status?.First(),
            Category_Status = request.Argument.Inactive_Category ? null : true,
            Current_Page = request.Argument.Searched_Page,
        };

        var contents = await _contentRepository.GetContentAsync(filter, cancellationToken);

        var response = new ResponseContent()
        {
            Items = contents.Select(col => new Contents()
            {
                Id = col.Uid.ToString("N"),
                Title = col.Title,
                Category = col.Category,
                Article_Status = col.Article_Status,
                Category_Status = col.Category_Status
            }).ToList(),
            Total_Row = contents.FirstOrDefault()?.Total_Row,
            Curr_Page = contents.FirstOrDefault()?.Curr_Page,
            Next_Page = contents.FirstOrDefault()?.Next_Page,
            Prev_Page = contents.FirstOrDefault()?.Prev_Page,
        };

        return Result.Success(response);
    }
}
