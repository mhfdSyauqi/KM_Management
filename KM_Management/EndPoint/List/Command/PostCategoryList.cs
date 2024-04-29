using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.List;
using KM_Management.Shared;
using System.Net;
using KM_Management.EndPoint.List.Models;

namespace KM_Management.EndPoint.List.Command;

public record PostCategoryListCommand(RequestPostCategoryList Argument) : ICommand;

public class PostCategoryListValidator : AbstractValidator<PostCategoryListCommand>
{
    private readonly IListRepository _ListRepository;

    public PostCategoryListValidator(IListRepository ListRepository)
    {
        _ListRepository = ListRepository;

        RuleFor(key => key.Argument.Name)
                .Must(UniqueCategoryName).WithMessage("Category Name Already Taken!")
                .NotEmpty().WithMessage("Category Name Cannot be Null or Empty")
                .NotNull().WithMessage("Category Name Cannot be Null or Empty")
                .MaximumLength(120).WithMessage("Category Name must be max 120 characters");

    }
    private bool UniqueCategoryName(string name)
    {
        return _ListRepository.VerifyAvailableCategoryName(name);
    }

}

public class PostCategoryListHandler : ICommandHandler<PostCategoryListCommand>
{
    private readonly IListRepository _ListRepository;
    private readonly IValidator<PostCategoryListCommand> _validator;

    public PostCategoryListHandler(IListRepository ListRepository, IValidator<PostCategoryListCommand> validator)
    {
        _ListRepository = ListRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PostCategoryListCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var PostCategoryList = new EntityPostCategoryList()
        {
            Uid = request.Argument.Uid != null ? Guid.Parse(request.Argument.Uid) : null,
            Name = request.Argument.Name,
            Layer = request.Argument.Layer,
            Uid_Reference = request.Argument.Uid_Reference != null ? Guid.Parse(request.Argument.Uid_Reference) : null,
            Is_Active = request.Argument.Is_Active,
            Create_By = request.Argument.Create_By,
            Create_At = request.Argument.Create_At
        };

        await _ListRepository.PostCategoryListAsync(PostCategoryList, cancellationToken);
        return Result.Success();
    }
}
