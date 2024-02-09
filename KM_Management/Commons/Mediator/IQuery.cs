using KM_Management.Shared;
using MediatR;

namespace KM_Management.Commons.Mediator;

public interface IQuery<TResposne> : IRequest<Result<TResposne>>
{
}
