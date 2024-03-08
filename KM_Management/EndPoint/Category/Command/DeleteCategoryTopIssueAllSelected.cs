using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Command;

public record DeleteCategoryTopIssueAllSelectedCommand(RequestDeleteCategoryTopIssueAllSelected Argument) : ICommand;

public class DeleteCategoryTopIssueAllSelectedValidator : AbstractValidator<DeleteCategoryTopIssueAllSelectedCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryTopIssueAllSelectedValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

    }
}

public class DeleteCategoryTopIssueAllSelectedHandler : ICommandHandler<DeleteCategoryTopIssueAllSelectedCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<DeleteCategoryTopIssueAllSelectedCommand> _validator;

    public DeleteCategoryTopIssueAllSelectedHandler(ICategoryRepository categoryRepository, IValidator<DeleteCategoryTopIssueAllSelectedCommand> validator)
    {
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(DeleteCategoryTopIssueAllSelectedCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var DeleteCategoryTopIssueAllSelected = new EntityDeleteCategoryTopIssueAllSelected()
        {
            Modified_By = request.Argument.Modified_By,
            Modified_At = request.Argument.Modified_At  
        };

        await _categoryRepository.DeleteCategoryTopIssueAllSelectedAsync(DeleteCategoryTopIssueAllSelected, cancellationToken);
        return Result.Success();
    }
}
