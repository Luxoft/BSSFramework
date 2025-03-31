using Framework.Events;

namespace SampleSystem.Domain;

public static class SampleSystemEventOperation
{
    public static EventOperation CustomEmployeeAction { get; } = new (nameof(CustomEmployeeAction));
}
