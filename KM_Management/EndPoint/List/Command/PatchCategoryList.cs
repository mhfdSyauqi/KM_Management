using FluentValidation;
using KM_Management.Commons.FluentExtention;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.List;
using KM_Management.EndPoint.List.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.List.Command;

public record PatchCategoryListCommand(RequestPatchCategoryList Argument) : ICommand;


public class PatchCategoryListValidator : AbstractValidator<PatchCategoryListCommand>
{
    private readonly IListRepository _ListRepository;

    public PatchCategoryListValidator(IListRepository ListRepository)
    {
        _ListRepository = ListRepository;


        RuleFor(key => key.Argument.Name)
                .NotEmpty().WithMessage("Category Name Cannot be Null or Empty")
                .NotNull().WithMessage("Category Name Cannot be Null or Empty")
                .MaximumLength(120).WithMessage("Category Name must be max 120 characters");

        RuleFor(key => key.Argument).Must((Category) => UniqueCategoryName(Category.Name, Category.Uid)).WithMessage("Category Name Already Taken!");

    }

    private bool UniqueCategoryName(string name, string uid)
    {
        bool isValidId = Guid.TryParse(uid, out Guid result);
        if (!isValidId) return false;
        return _ListRepository.VerifyAvailableCategoryName(name, result);
    }

}

public class PatchCategoryListHandler : ICommandHandler<PatchCategoryListCommand>
{
    private readonly IListRepository _ListRepository;
    private readonly IValidator<PatchCategoryListCommand> _validator;

    public PatchCategoryListHandler(IListRepository ListRepository, IValidator<PatchCategoryListCommand> validator)
    {
        _ListRepository = ListRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PatchCategoryListCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var PatchCategoryList = new EntityPatchCategoryList()
        {
            Uid = Guid.Parse(request.Argument.Uid),
            Name = request.Argument.Name,
            Is_Active = request.Argument.Is_Active,
            Modified_By = request.Argument.Modified_By,
            Modified_At = request.Argument.Modified_At
        };

        await _ListRepository.PatchCategoryListAsync(PatchCategoryList, cancellationToken);
        return Result.Success();
    }
}