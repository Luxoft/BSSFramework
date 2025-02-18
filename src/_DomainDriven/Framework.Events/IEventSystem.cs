using Framework.Core;

namespace Framework.Events;

public interface IEventSystem
{
    ITypeResolver<string> TypeResolver { get; }

    IDomainObjectEventMetadata DomainObjectEventMetadata { get; }

    Task ForceEventAsync(EventModel eventModel, CancellationToken cancellationToken = default);
}
