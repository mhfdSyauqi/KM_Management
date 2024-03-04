using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Message.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Message.Query;

public record GetActiveMessageQuery(RequestActiveMessage Argument) : IQuery<List<ResponseActiveMessage>>;

public class GetActiveMessageValidator : AbstractValidator<GetActiveMessageQuery>
{
    public GetActiveMessageValidator()
    {

    }
}

public class GetActiveMessageHandler : IQueryHandler<GetActiveMessageQuery, List<ResponseActiveMessage>>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IValidator<GetActiveMessageQuery> _validator;

    public GetActiveMessageHandler(IMessageRepository messageRepository, IValidator<GetActiveMessageQuery> validator)
    {
        _messageRepository = messageRepository;
        _validator = validator;
    }


    public async Task<Result<List<ResponseActiveMessage>>> Handle(GetActiveMessageQuery request, CancellationToken cancellationToken)
    {
        var filter = new FilterActiveMessage()
        {
            Type = request.Argument.Type,
        };

        var messages = await _messageRepository.GetActiveMessageAsync(filter.Type, cancellationToken);

        if (messages == null)
        {
            return Result.Failure<List<ResponseActiveMessage>>(new(HttpStatusCode.NotFound, default));
        }

        var response = messages.Select(item => new ResponseActiveMessage() 
        { 
            Type = item.Type,
            Sequence = item.Sequence,
            Contents = item.Contents,
        }).ToList();
        return Result.Success(response);
    }

    //public async Task<Result<ResponseActiveMessage>> Handle(GetActiveMessageQuery request, CancellationToken cancellationToken)
    //{
    //    await _validator.ValidateAndThrowAsync(request, cancellationToken);

    //    var filter = new FilterActiveMessage()
    //    {
    //        Type = request.Argument.Type,
    //    };

    //    var messages = await _messageRepository.GetActiveMessageAsync(filter.Type, cancellationToken);

    //    var response = new ResponseActiveMessage()
    //    {
    //        Message = messages.Select(col => new Contents()
    //        {
    //            Id = col.Uid.ToString("N"),
    //            Title = col.Title,
    //            Category = col.Category,
    //            Article_Status = col.Article_Status,
    //            Category_Status = col.Category_Status
    //        }).ToList()
    //    };

    //    return Result.Success(response);
    //}
}
