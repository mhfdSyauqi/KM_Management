using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.Category.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Category.Command;

public record PostCategoryListCommand(RequestPostCategoryList Argument) : ICommand;

public class PostCategoryListValidator : AbstractValidator<PostCategoryListCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public PostCategoryListValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(key => key.Argument.Name)
                .Must(UniqueCategoryName).WithMessage("Category Name Already Taken!")
                .NotEmpty().WithMessage("Category Name Cannot be Null or Empty")
                .NotNull().WithMessage("Category Name Cannot be Null or Empty")
                .MaximumLength(120).WithMessage("Category Name must be max 120 characters");

    }
    private bool UniqueCategoryName(string name)
    {
        return _categoryRepository.VerifyAvailableCategoryName(name);
    }

}

public class PostCategoryListHandler : ICommandHandler<PostCategoryListCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<PostCategoryListCommand> _validator;

    public PostCategoryListHandler(ICategoryRepository categoryRepository, IValidator<PostCategoryListCommand> validator)
    {
        _categoryRepository = categoryRepository;
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
            Uid = request.Argument.Uid!= null ? Guid.Parse(request.Argument.Uid) : null,
            Name = request.Argument.Name,   
            Layer = request.Argument.Layer,
            Uid_Reference = request.Argument.Uid_Reference != null ? Guid.Parse(request.Argument.Uid_Reference) : null,
            Is_Active = request.Argument.Is_Active,
            Create_By = request.Argument.Create_By,
            Create_At = request.Argument.Create_At  
        };

        await _categoryRepository.PostCategoryListAsync(PostCategoryList, cancellationToken);
        return Result.Success();
    }
}
