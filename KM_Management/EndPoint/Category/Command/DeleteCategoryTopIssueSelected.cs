using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Command;

public record DeleteCategoryTopIssueSelectedCommand(RequestDeleteCategoryTopIssueSelected Argument) : ICommand;

public class DeleteCategoryTopIssueSelectedValidator : AbstractValidator<DeleteCategoryTopIssueSelectedCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryTopIssueSelectedValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

    }
}

public class DeleteCategoryTopIssueSelectedHandler : ICommandHandler<DeleteCategoryTopIssueSelectedCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<DeleteCategoryTopIssueSelectedCommand> _validator;

    public DeleteCategoryTopIssueSelectedHandler(ICategoryRepository categoryRepository, IValidator<DeleteCategoryTopIssueSelectedCommand> validator)
    {
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(DeleteCategoryTopIssueSelectedCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var DeleteCategoryTopIssueSelected = new EntityDeleteCategoryTopIssueSelected()
        {
            Sequence = request.Argument.Sequence,
            Modified_By = request.Argument.Modified_By,
            Modified_At = request.Argument.Modified_At,
        };

        await _categoryRepository.DeleteCategoryTopIssueSelectedAsync(DeleteCategoryTopIssueSelected, cancellationToken);
        return Result.Success();
    }
}
