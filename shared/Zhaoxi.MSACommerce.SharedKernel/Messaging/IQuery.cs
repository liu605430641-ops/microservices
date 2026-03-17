using MediatR;

namespace Zhaoxi.MSACommerce.SharedKernel.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}