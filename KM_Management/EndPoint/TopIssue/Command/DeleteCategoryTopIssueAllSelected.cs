using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.TopIssue;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.TopIssue.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.TopIssue.Command;

public record DeleteCategoryTopIssueAllSelectedCommand(RequestDeleteCategoryTopIssueAllSelected Argument) : ICommand;

public class DeleteCategoryTopIssueAllSelectedValidator : AbstractValidator<DeleteCategoryTopIssueAllSelectedCommand>
{
    private readonly ITopIssueRepository _topIssueRepository;

    public DeleteCategoryTopIssueAllSelectedValidator(ITopIssueRepository TopIssueRepository)
    {
        _topIssueRepository = TopIssueRepository;

    }
}

public class DeleteCategoryTopIssueAllSelectedHandler : ICommandHandler<DeleteCategoryTopIssueAllSelectedCommand>
{
    private readonly ITopIssueRepository _topIssueRepository;
    private readonly IValidator<DeleteCategoryTopIssueAllSelectedCommand> _validator;

    public DeleteCategoryTopIssueAllSelectedHandler(ITopIssueRepository TopIssueRepository, IValidator<DeleteCategoryTopIssueAllSelectedCommand> validator)
    {
        _topIssueRepository = TopIssueRepository;
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

        await _topIssueRepository.DeleteCategoryTopIssueAllSelectedAsync(DeleteCategoryTopIssueAllSelected, cancellationToken);
        return Result.Success();
    }
}
