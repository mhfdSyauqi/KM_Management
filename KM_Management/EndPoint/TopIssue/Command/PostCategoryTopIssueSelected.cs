using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.TopIssue;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.TopIssue.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.TopIssue.Command;

public record PostCategoryTopIssueSelectedCommand(RequestPostCategoryTopIssueSelected Argument) : ICommand;

public class PostCategoryTopIssueSelectedValidator : AbstractValidator<PostCategoryTopIssueSelectedCommand>
{
    private readonly ITopIssueRepository _TopIssueRepository;

    public PostCategoryTopIssueSelectedValidator(ITopIssueRepository TopIssueRepository)
    {
        _TopIssueRepository = TopIssueRepository;

    }
}

public class PostCategoryTopIssueSelectedHandler : ICommandHandler<PostCategoryTopIssueSelectedCommand>
{
    private readonly ITopIssueRepository _TopIssueRepository;
    private readonly IValidator<PostCategoryTopIssueSelectedCommand> _validator;

    public PostCategoryTopIssueSelectedHandler(ITopIssueRepository TopIssueRepository, IValidator<PostCategoryTopIssueSelectedCommand> validator)
    {
        _TopIssueRepository = TopIssueRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PostCategoryTopIssueSelectedCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var PostCategoryTopIssueSelected = new EntityPostCategoryTopIssueSelected()
        {
            Uid = Guid.Parse(request.Argument.Uid_Bot_Category),
            Create_By = request.Argument.Create_By,
            Create_At = request.Argument.Create_At  
        };

        await _TopIssueRepository.PostCategoryTopIssueSelectedAsync(PostCategoryTopIssueSelected, cancellationToken);
        return Result.Success();
    }
}
