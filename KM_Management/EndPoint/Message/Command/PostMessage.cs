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

        RuleFor(key => key.Argument.Contents).NotEmpty().WithMessage("Message must not be empty.");
       

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
            Contents = request.Argument.Contents,
            Create_By = request.Argument.Create_By,
            Create_At = request.Argument.Create_At,
        };

        await _messageRepository.PostMessageAsync(PostMessage, cancellationToken);
        return Result.Success();
    }
}
