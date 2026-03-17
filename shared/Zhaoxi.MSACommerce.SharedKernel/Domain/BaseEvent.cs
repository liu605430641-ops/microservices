using MediatR;

namespace Zhaoxi.MSACommerce.SharedKernel.Domain;

public abstract class BaseEvent : INotification
{
    public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.Now;
}
