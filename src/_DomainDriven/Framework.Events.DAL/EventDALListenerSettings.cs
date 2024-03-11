namespace Framework.Events;

public class EventDALListenerSettings<TPersistentDomainObjectBase>
{
    public TypeEvent[] TypeEvents { get; init; }

    public TypeEventDependency[] Dependencies { get; init; }
}
