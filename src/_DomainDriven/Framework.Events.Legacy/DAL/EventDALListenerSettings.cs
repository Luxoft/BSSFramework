namespace Framework.Events;

public class EventDALListenerSettings<TPersistentDomainObjectBase>
{
    public TypeEvent[] TypeEvents { get; init; } = Array.Empty<TypeEvent>();

    public TypeEventDependency[] Dependencies { get; init; } = Array.Empty<TypeEventDependency>();
}
