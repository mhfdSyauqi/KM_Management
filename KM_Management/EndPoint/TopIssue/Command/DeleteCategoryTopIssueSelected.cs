using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.TopIssue;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.TopIssue.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.TopIssue.Command;

public record DeleteCategoryTopIssueSelectedCommand(RequestDeleteCategoryTopIssueSelected Argument) : ICommand;

public class DeleteCategoryTopIssueSelectedValidator : AbstractValidator<DeleteCategoryTopIssueSelectedCommand>
{
    private readonly ITopIssueRepository _TopIssueRepository;

    public DeleteCategoryTopIssueSelectedValidator(ITopIssueRepository TopIssueRepository)
    {
        _TopIssueRepository = TopIssueRepository;

    }
}

public class DeleteCategoryTopIssueSelectedHandler : ICommandHandler<DeleteCategoryTopIssueSelectedCommand>
{
    private readonly ITopIssueRepository _TopIssueRepository;
    private readonly IValidator<DeleteCategoryTopIssueSelectedCommand> _validator;

    public DeleteCategoryTopIssueSelectedHandler(ITopIssueRepository TopIssueRepository, IValidator<DeleteCategoryTopIssueSelectedCommand> validator)
    {
        _TopIssueRepository = TopIssueRepository;
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

        await _TopIssueRepository.DeleteCategoryTopIssueSelectedAsync(DeleteCategoryTopIssueSelected, cancellationToken);
        return Result.Success();
    }
}
