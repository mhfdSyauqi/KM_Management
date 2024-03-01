using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.AssistantProfile;
using KM_Management.EndPoint.AssistantProfile.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.AssistantProfile.Command;

public record PostAssistantProfileCommand(RequestPostAssistantProfile Argument) : ICommand;

public class PostAssistantProfileValidator : AbstractValidator<PostAssistantProfileCommand>
{
    private readonly IAssistantProfileRepository _assistantProfileRepository;

    public PostAssistantProfileValidator(IAssistantProfileRepository assistantProfileRepository)
    {
        _assistantProfileRepository = assistantProfileRepository;

        //RuleFor(key => key.Argument.AppName).NotEmpty().WithMessage("Title is required.").Must(UniqueTitle).WithMessage("Title already used.");
        RuleFor(key => key.Argument.AppName).NotEmpty().WithMessage("App Name Required");
        RuleFor(key => key.Argument.AppName)
            .Length(5, 150).WithMessage("App Name must be between 5 and 150 characters");

        RuleFor(key => key.Argument.Files).NotNull().NotEmpty();

        RuleFor(key => key.Argument.Files.Length)
        .LessThanOrEqualTo(2 * 1024 * 1024)
        .WithMessage("File size must be less than or equal to 2MB")
        .When(key => key.Argument.Files != null);

        RuleFor(key => key.Argument.Files.ContentType).Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
        .WithMessage("Type of file must be a valid image (PNG or JPG or IMG)")
        .When(key => key.Argument.Files != null && key.Argument.Files.ContentType != null);

        //When(prop => !string.IsNullOrEmpty(prop.Argument.Category_Id), () =>
        //{
        //    RuleFor(key => key.Argument.Category_Id).Must(ValidCategory).WithMessage("Category is not valid");
        //});
    }

    //private bool UniqueTitle(string title)
    //{
    //    return _contentRepository.VerifyAvailableTitle(title);
    //}

    //private bool ValidCategory(string? categoryId)
    //{
    //    bool isValidId = Guid.TryParse(categoryId, out Guid result);
    //    if (!isValidId) return false;
    //    return _categoryRepository.VerifyValidCategory(result);
    //}
}

public class PostAssistantProfileHandler : ICommandHandler<PostAssistantProfileCommand>
{
    private readonly IAssistantProfileRepository _assistantProfileRepository;
    private readonly IValidator<PostAssistantProfileCommand> _validator;

    public PostAssistantProfileHandler(IAssistantProfileRepository assistantProfileRepository, IValidator<PostAssistantProfileCommand> validator)
    {
        _assistantProfileRepository = assistantProfileRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PostAssistantProfileCommand request,  CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var postAssistantProfile= new EntityPostAssistantProfile()
        {
            AppImage = request.Argument.AppImage,
            AppName = request.Argument.AppName,
            Files = new List<IFormFile> { request.Argument.Files }
        };

        await _assistantProfileRepository.PostAssistantProfileAsync(postAssistantProfile, cancellationToken);
        return Result.Success();
    }
}
