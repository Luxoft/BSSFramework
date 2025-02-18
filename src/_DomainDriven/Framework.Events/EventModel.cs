namespace Framework.Events;

public record EventModel(Type DomainType, IReadOnlyList<Guid> DomainObjectIdents, EventOperation EventOperation, long? Revision);
