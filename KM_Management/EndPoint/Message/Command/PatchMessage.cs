using FluentValidation;
using KM_Management.Commons.FluentExtention;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Message.Command;

public record PatchMessageCommand(RequestPatchMessage Argument) : ICommand;


public class PatchMessageValidator : AbstractValidator<PatchMessageCommand>
{
    private readonly IMessageRepository _messageRepository;

    public PatchMessageValidator(IMessageRepository MessageRepository, ICategoryRepository categoryRepository)
    {
        _messageRepository = MessageRepository;

        RuleFor(key => key.Argument.Contents).NotEmpty().WithMessage("Message must not be empty.");
        When(key => key.Argument.Contents.Length > 0, () =>
        {
            RuleFor(key => key.Argument.Contents).MinimumLength(5).WithMessage("Message must be at least 5 characters.")
                .MaximumLength(150).WithMessage("Message cannot exceed 150 characters.");
            RuleFor(key => key.Argument.Contents)
                .Must(ValidateInput).WithMessage("Invalid Format! Please use the following format: '@username @fullname @category");
        });
    }

    private bool ValidateInput(string contents)
    {
        // Rule 1: Check if '@' is present
        if (contents.Contains("@"))
        {
            // Rule 2: Check if valid tags are present after '@'
            var validTags = new[] { "@fullname", "@username", "@category" };
            var tags = contents.Split(' ');

            foreach (var tag in tags)
            {
                var tagTrimmed = tag.Trim();

                if (tagTrimmed.StartsWith("@") && validTags.Any(validTag => tagTrimmed == validTag))
                {
                    continue;
                }
                else if (!tagTrimmed.Contains("@"))
                {

                    continue;
                }
                else
                {

                    return false;
                }
            }

            return true;
        }

        return true;
    }

}

public class PatchMessageHandler : ICommandHandler<PatchMessageCommand>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IValidator<PatchMessageCommand> _validator;

    public PatchMessageHandler(IMessageRepository MessageRepository, IValidator<PatchMessageCommand> validator)
    {
        _messageRepository = MessageRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PatchMessageCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var patchMessage = new EntityPatchMessage()
        {
            Type = request.Argument.Type,
            Sequence = request.Argument.Sequence,
            Contents = request.Argument.Contents,
            Is_Active = request.Argument.Is_Active,
            Modified_By = request.Argument.Modified_By,
            Modified_At = request.Argument.Modified_At
            
        };

        await _messageRepository.PatchMessageAsync(patchMessage, cancellationToken);
        return Result.Success();
    }
}