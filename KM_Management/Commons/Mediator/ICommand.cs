using KM_Management.Shared;
using MediatR;

namespace KM_Management.Commons.Mediator;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
