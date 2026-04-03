using Framework.Application.Events;

namespace SampleSystem.EventMetadata;

public static class SampleSystemEventOperation
{
    public static EventOperation CustomAction { get; } = new (nameof(CustomAction));
}
