using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.TopIssue;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.TopIssue.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.TopIssue.Command;

public record PatchCategoryTopIssueSelectedSequenceCommand(RequestPatchCategoryTopIssueSelectedSequence Argument) : ICommand;

public class PatchCategoryTopIssueSelectedSequenceValidator : AbstractValidator<PatchCategoryTopIssueSelectedSequenceCommand>
{
    private readonly ITopIssueRepository _TopIssueRepository;

    public PatchCategoryTopIssueSelectedSequenceValidator(ITopIssueRepository TopIssueRepository)
    {
        _TopIssueRepository = TopIssueRepository;

    }
}

public class PatchCategoryTopIssueSelectedSequenceHandler : ICommandHandler<PatchCategoryTopIssueSelectedSequenceCommand>
{
    private readonly ITopIssueRepository _TopIssueRepository;
    private readonly IValidator<PatchCategoryTopIssueSelectedSequenceCommand> _validator;

    public PatchCategoryTopIssueSelectedSequenceHandler(ITopIssueRepository TopIssueRepository, IValidator<PatchCategoryTopIssueSelectedSequenceCommand> validator)
    {
        _TopIssueRepository = TopIssueRepository;
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

        await _TopIssueRepository.PatchCategoryTopIssueSelectedSequenceAsync(PatchCategoryTopIssueSelectedSequence, cancellationToken);
        return Result.Success();
    }
}
