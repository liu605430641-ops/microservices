using MediatR;

namespace Zhaoxi.MSACommerce.SharedKernel.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}