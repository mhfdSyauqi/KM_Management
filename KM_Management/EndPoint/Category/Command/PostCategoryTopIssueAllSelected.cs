using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Command;

public record PostCategoryTopIssueAllSelectedCommand(RequestPostCategoryTopIssueAllSelected Argument) : ICommand;

public class PostCategoryTopIssueAllSelectedValidator : AbstractValidator<PostCategoryTopIssueAllSelectedCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public PostCategoryTopIssueAllSelectedValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

    }
}

public class PostCategoryTopIssueAllSelectedHandler : ICommandHandler<PostCategoryTopIssueAllSelectedCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<PostCategoryTopIssueAllSelectedCommand> _validator;

    public PostCategoryTopIssueAllSelectedHandler(ICategoryRepository categoryRepository, IValidator<PostCategoryTopIssueAllSelectedCommand> validator)
    {
        _categoryRepository = categoryRepository;
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
            Create_At = request.Argument.Create_At  
        };

        await _categoryRepository.PostCategoryTopIssueAllSelectedAsync(PostCategoryTopIssueAllSelected, cancellationToken);
        return Result.Success();
    }
}
