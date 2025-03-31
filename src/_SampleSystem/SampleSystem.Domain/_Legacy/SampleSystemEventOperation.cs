using Framework.Events;

namespace SampleSystem.Domain;

public static class SampleSystemEventOperation
{
    public static EventOperation CustomAction { get; } = new (nameof(CustomAction));
}
