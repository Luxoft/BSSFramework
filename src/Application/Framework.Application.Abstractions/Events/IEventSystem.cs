using Framework.Core;
using Framework.Core.TypeResolving;

namespace Framework.Application.Events;

public interface IEventSystem
{
    ITypeResolver<TypeNameIdentity> TypeResolver { get; }

    IDomainObjectEventMetadata DomainObjectEventMetadata { get; }

    Task ForceEventAsync(EventModel eventModel, CancellationToken cancellationToken = default);
}
