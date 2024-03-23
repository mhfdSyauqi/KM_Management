using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.AssistantProfile;
using KM_Management.EndPoint.AssistantProfile.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.AssistantProfile.Command;

public record PostAssistantProfileCommand(RequestPostAssistantProfile Argument, string Host) : ICommand;

public class PostAssistantProfileValidator : AbstractValidator<PostAssistantProfileCommand>
{
    private readonly IAssistantProfileRepository _assistantProfileRepository;

    public PostAssistantProfileValidator(IAssistantProfileRepository assistantProfileRepository)
    {
        _assistantProfileRepository = assistantProfileRepository;

        RuleFor(x => x.Argument.AppName)
            .NotEmpty().WithMessage("Virtual Name is required.")
            .MinimumLength(5).WithMessage("Virtual Name must be at least 5 characters.")
            .MaximumLength(150).WithMessage("Virtual Name cannot exceed 150 characters.");

        When(key => key.Argument.Files?.Count > 0, () =>
        {
            RuleFor(x => x.Argument.Files)
                .Must(files => files.All(file => file.Length <= 2 * 1024 * 1024)).WithMessage("File size must be less than or equal to 2MB");

            RuleFor(x => x.Argument.Files)
                .Must(files => files.All(file => file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png"))
                .WithMessage("Files must be valid images (PNG or JPG)");
        });
        
    }

    private bool UniqueName(string name)
    {
        
        return _assistantProfileRepository.VerifyAvailableName(name);
    }


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

        var postAssistantProfile = new EntityPostAssistantProfile()
        {
            AppImage = request?.Argument?.AppImage,
            AppName = request.Argument.AppName,
            Files = request?.Argument?.Files?.ToList() 
        };

        await _assistantProfileRepository.PostAssistantProfileAsync(postAssistantProfile, request.Host, cancellationToken);
        return Result.Success();
    }
}
