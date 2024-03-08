using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Command;

public record PostCategoryTopIssueSelectedCommand(RequestPostCategoryTopIssueSelected Argument) : ICommand;

public class PostCategoryTopIssueSelectedValidator : AbstractValidator<PostCategoryTopIssueSelectedCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public PostCategoryTopIssueSelectedValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

    }
}

public class PostCategoryTopIssueSelectedHandler : ICommandHandler<PostCategoryTopIssueSelectedCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<PostCategoryTopIssueSelectedCommand> _validator;

    public PostCategoryTopIssueSelectedHandler(ICategoryRepository categoryRepository, IValidator<PostCategoryTopIssueSelectedCommand> validator)
    {
        _categoryRepository = categoryRepository;
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

        await _categoryRepository.PostCategoryTopIssueSelectedAsync(PostCategoryTopIssueSelected, cancellationToken);
        return Result.Success();
    }
}
