using KM_Management.Shared;
using MediatR;

namespace KM_Management.Commons.Mediator;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
