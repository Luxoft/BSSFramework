using System.Collections.Immutable;

namespace Framework.Application.Events;

public record EventModel(Type DomainType, ImmutableArray<Guid> DomainObjectIdents, EventOperation EventOperation, long? Revision);
