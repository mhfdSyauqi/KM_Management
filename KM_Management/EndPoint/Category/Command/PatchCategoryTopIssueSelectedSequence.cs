using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Command;

public record PatchCategoryTopIssueSelectedSequenceCommand(RequestPatchCategoryTopIssueSelectedSequence Argument) : ICommand;

public class PatchCategoryTopIssueSelectedSequenceValidator : AbstractValidator<PatchCategoryTopIssueSelectedSequenceCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public PatchCategoryTopIssueSelectedSequenceValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

    }
}

public class PatchCategoryTopIssueSelectedSequenceHandler : ICommandHandler<PatchCategoryTopIssueSelectedSequenceCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<PatchCategoryTopIssueSelectedSequenceCommand> _validator;

    public PatchCategoryTopIssueSelectedSequenceHandler(ICategoryRepository categoryRepository, IValidator<PatchCategoryTopIssueSelectedSequenceCommand> validator)
    {
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PatchCategoryTopIssueSelectedSequenceCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var PatchCategoryTopIssueSelectedSequence = new EntityPatchCategoryTopIssueSelectedSequence()
        {
            Current_Sequence = request.Argument.Current_Sequence,
            New_Sequence = request.Argument.New_Sequence,
            Modified_By = request.Argument.Modified_By,
            Modified_At = request.Argument.Modified_At,
        };

        await _categoryRepository.PatchCategoryTopIssueSelectedSequenceAsync(PatchCategoryTopIssueSelectedSequence, cancellationToken);
        return Result.Success();
    }
}
