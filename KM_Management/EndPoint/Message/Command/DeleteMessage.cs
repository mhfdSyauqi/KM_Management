using FluentValidation;
using KM_Management.Commons.FluentExtention;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Message.Command;

public record DeleteMessageCommand(RequestDeleteMessage Argument) : ICommand;


public class DeleteMessageValidator : AbstractValidator<DeleteMessageCommand>
{
    private readonly IMessageRepository _messageRepository;

    public DeleteMessageValidator(IMessageRepository MessageRepository, ICategoryRepository categoryRepository)
    {
        _messageRepository = MessageRepository;

    }
}

public class DeleteMessageHandler : ICommandHandler<DeleteMessageCommand>
{
    private readonly IMessageRepository _messageRepository;

    public DeleteMessageHandler(IMessageRepository messageRepository, IValidator<DeleteMessageCommand> validator)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Result> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {

        var DeleteMessage = new EntityDeleteMessage()
        {
            Type = request.Argument.Type,
            Sequence = request.Argument.Sequence,
            Modified_By = request.Argument.Modified_By,
            Modified_At = request.Argument.Modified_At

        };

        await _messageRepository.DeleteMessageAsync(DeleteMessage, cancellationToken);
        return Result.Success();
    }
}