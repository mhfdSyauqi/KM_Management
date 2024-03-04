using FluentValidation;
using KM_Management.Commons.FluentExtention;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Message.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Message.Command;

public record PatchSequenceMessageCommand(RequestPatchSequenceMessage Argument) : ICommand;


public class PatchSequenceMessageValidator : AbstractValidator<PatchSequenceMessageCommand>
{
    private readonly IMessageRepository _messageRepository;

    public PatchSequenceMessageValidator(IMessageRepository MessageRepository, ICategoryRepository categoryRepository)
    {
        _messageRepository = MessageRepository;
    }
}

public class PatchSequenceMessageHandler : ICommandHandler<PatchSequenceMessageCommand>
{
    private readonly IMessageRepository _messageRepository;

    public PatchSequenceMessageHandler(IMessageRepository MessageRepository, IValidator<PatchSequenceMessageCommand> validator)
    {
        _messageRepository = MessageRepository;
    }

    public async Task<Result> Handle(PatchSequenceMessageCommand request, CancellationToken cancellationToken)
    {

        var patchSequenceMessage = new EntityPatchSequenceMessage()
        {
            Type = request.Argument.Type,
            Current_Sequence =request.Argument.Current_Sequence,    
            New_Sequence = request.Argument.New_Sequence,
            Modified_By = request.Argument.Modified_By, 
            Modified_At = request.Argument.Modified_At,
        };

        await _messageRepository.PatchSequenceMessageAsync(patchSequenceMessage, cancellationToken);
        return Result.Success();
    }
}