using MediatR;

namespace Zhaoxi.MSACommerce.SharedKernel.Messaging;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}