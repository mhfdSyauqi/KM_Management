using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message;
using KM_Management.EndPoint.Message.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Message.Command;

public record PostMessageCommand(RequestPostMessage Argument) : ICommand;

public class PostMessageValidator : AbstractValidator<PostMessageCommand>
{
    private readonly IMessageRepository _messageRepository;

    public PostMessageValidator(IMessageRepository MessageRepository, ICategoryRepository categoryRepository)
    {
        _messageRepository = MessageRepository;

        RuleFor(key => key.Argument.Contents).NotEmpty().NotNull();
        RuleFor(key => key.Argument.Contents).Length(5, 150).WithMessage("App Name must be between 5 and 150 characters");
        RuleFor(key => key.Argument.Contents)
            .Must(ValidateInput).WithMessage("Invalid Format! Please use the following format: '@username @fullname @category");
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

public class PostMessageHandler : ICommandHandler<PostMessageCommand>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IValidator<PostMessageCommand> _validator;

    public PostMessageHandler(IMessageRepository messageRepository, IValidator<PostMessageCommand> validator)
    {
        _messageRepository = messageRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PostMessageCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var PostMessage = new EntityPostMessage()
        {
            Type = request.Argument.Type,
            Sequence = request.Argument.Sequence,
            Contents = request.Argument.Contents,
            Is_Active = request.Argument.Is_Active,
            Create_By = request.Argument.Create_By,
            Create_At = request.Argument.Create_At,
        };

        await _messageRepository.PostMessageAsync(PostMessage, cancellationToken);
        return Result.Success();
    }
}
