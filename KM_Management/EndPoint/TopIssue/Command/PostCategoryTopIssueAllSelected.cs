using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.TopIssue;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.TopIssue.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.TopIssue.Command;

public record PostCategoryTopIssueAllSelectedCommand(RequestPostCategoryTopIssueAllSelected Argument) : ICommand;

public class PostCategoryTopIssueAllSelectedValidator : AbstractValidator<PostCategoryTopIssueAllSelectedCommand>
{
    private readonly ITopIssueRepository _TopIssueRepository;

    public PostCategoryTopIssueAllSelectedValidator(ITopIssueRepository TopIssueRepository)
    {
        _TopIssueRepository = TopIssueRepository;

    }
}

public class PostCategoryTopIssueAllSelectedHandler : ICommandHandler<PostCategoryTopIssueAllSelectedCommand>
{
    private readonly ITopIssueRepository _TopIssueRepository;
    private readonly IValidator<PostCategoryTopIssueAllSelectedCommand> _validator;

    public PostCategoryTopIssueAllSelectedHandler(ITopIssueRepository TopIssueRepository, IValidator<PostCategoryTopIssueAllSelectedCommand> validator)
    {
        _TopIssueRepository = TopIssueRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PostCategoryTopIssueAllSelectedCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var PostCategoryTopIssueAllSelected = new EntityPostCategoryTopIssueAllSelected()
        {
            Create_By = request.Argument.Create_By,
            Create_At = request.Argument.Create_At,
            Modified_By = request.Argument.Modified_By,
            Modified_At = request.Argument.Modified_At,
        };

        await _TopIssueRepository.PostCategoryTopIssueAllSelectedAsync(PostCategoryTopIssueAllSelected, cancellationToken);
        return Result.Success();
    }
}
